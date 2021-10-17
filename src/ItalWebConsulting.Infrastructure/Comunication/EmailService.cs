using ItalWebConsulting.Infrastructure.Composition;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace ItalWebConsulting.Infrastructure.Comunication
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings smptSettings;
        public ILogger<EmailService> Logger { get; set; }
        public IWebHostEnvironment WebHostEnvironment { get; set; }
        public virtual bool IsDevelopment { get; set; } = true;
        protected readonly IConfigurationManager configuration;
        public EmailService(IConfigurationManager configuration)
        {
            this.configuration = configuration;
            this.smptSettings = this.configuration.SmtpSettings;
           
        }
        public async Task<bool> SendComunicationEmailAsync(string emailTo, string message)
        {
            return await SendComunicationEmailAsync(emailTo, message, "Comunicazione");
        }

        public async Task<bool> SendComunicationEmailAsync(string emailTo, string message, string subject)
        {
            return await SendEmailAsync(new EmailInput
            {
                AdressEmailTo = new[] { emailTo },
                Message = message,
                Subject = subject
            });
        }

        public async Task<bool> SendEmailAsync(string emailTo, string subject, string message)
        {
            return await SendEmailAsync(new EmailInput
            {
                AdressEmailTo = new[] { emailTo },
                Message = message,
                Subject = subject,
                Sender = new EmailSender { EmailAdress = smptSettings.DefaultSender.EmailAdress, Name = smptSettings.DefaultSender.Name}
            });
        }
        public async Task<bool> SendEmailAsync(EmailInput input)
        {

            try
            {
                if (input == null)
                    throw new ArgumentNullException(nameof(input));
                if (input.AdressEmailTo == null || !input.AdressEmailTo.Any())
                    throw new ArgumentNullException(nameof(input.AdressEmailTo));

                var mimeMessage = new MimeMessage();
                var sender = GetSender(input.Sender);
                mimeMessage.From.Add(new MailboxAddress(sender.Name, sender.EmailAdress));

                foreach (var to in input.AdressEmailTo)
                    if (string.IsNullOrWhiteSpace(to))
                        throw new ArgumentNullException(nameof(input.AdressEmailTo), "Attenzione, la lista in input contiene elementi vuoti. Rimuovere gli elementi e riprovare");
                    else
                        mimeMessage.To.Add(new MailboxAddress(to));

                if (input.AdressEmailCc != null)
                    foreach (var cc in input.AdressEmailCc)
                        if (string.IsNullOrWhiteSpace(cc))
                            throw new ArgumentNullException(nameof(input.AdressEmailTo), "Attenzione, la lista in input contiene elementi vuoti. Rimuovere gli elementi e riprovare");
                        else
                            mimeMessage.To.Add(new MailboxAddress(cc));

                mimeMessage.Subject = input.Subject;

                var builder = GetAttachments(input.Attachments);
                if (builder != null)
                {
                    //builder.TextBody = input.Message;
                    builder.HtmlBody = input.Message;
                    mimeMessage.Body = builder.ToMessageBody();
                }
                else
                    mimeMessage.Body = new TextPart("html")
                    {
                        Text = input.Message
                    };
         
                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
#if (DEBUG)
                    await client.ConnectAsync(smptSettings.Server, smptSettings.Port);
#else

                    await client.ConnectAsync(smptSettings.Server, smptSettings.Port, smptSettings.UseSsl);
#endif
                    ////smptSettings.User = smptSettings.User;
                    ////smptSettings.Password = smptSettings.Password;
                    //// Note: only needed if the SMTP server requires authentication
                    await  client.AuthenticateAsync(smptSettings.User,smptSettings.Password);

                    await client.SendAsync(mimeMessage);

                    await client.DisconnectAsync(true);

                }
                return true;
            }
            catch (Exception ex)
            {
                if (Logger != null)
                    Logger.LogError(ex, "Impossibile inviare la mail");
                else
                    throw ex;
            }

            return false;
        }

        public async Task<bool> SendCalendarEmailAsync(System.Net.Mail.MailMessage mailMessage)
        {
            using (var smtpClient = new System.Net.Mail.SmtpClient())
            {
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential(smptSettings.User, smptSettings.Password);
                await smtpClient.SendMailAsync(mailMessage);
            }
            return true;
        }
        private static BodyBuilder GetAttachments(IEnumerable<string> pathFileAttachments)
        {
            if (pathFileAttachments == null || !pathFileAttachments.Any())
                return null;

            var builder = new BodyBuilder();
            foreach (var path in pathFileAttachments)
            {
                if (!File.Exists(path))
                    throw new FileNotFoundException("Nessun file trovato per il path specificato.", path);
                builder.Attachments.Add(path);
            }

            return builder;
        }

        private EmailSender GetSender(EmailSender input)
        {
            var senderName = smptSettings.DefaultSender.Name;
            if (!string.IsNullOrWhiteSpace(input.Name))
                senderName = input.Name;
            if (string.IsNullOrWhiteSpace(senderName))
                throw new ArgumentNullException("input.Sender.Name", "Nessun nome mittente email specificato in input e nessun valore di default impostato nell'appsettings file.");


            var senderEmail = smptSettings.DefaultSender.EmailAdress;
            if (!string.IsNullOrWhiteSpace(input.EmailAdress))
                senderEmail = input.EmailAdress;
            if (string.IsNullOrWhiteSpace(senderEmail))
                throw new ArgumentNullException("input.Sender.EmailAdress", "Nessun indirizzo email mittente specificato in input e nessun valore di default impostato nell'appsettings file.");

            return new EmailSender
            {
                EmailAdress = senderEmail,
                Name = senderName
            };
        }



    }
}
