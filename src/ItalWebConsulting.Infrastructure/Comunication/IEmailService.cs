using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ItalWebConsulting.Infrastructure.Comunication
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(EmailInput input);
        Task<bool> SendEmailAsync(string emailTo, string subject, string message);
        Task<bool> SendCalendarEmailAsync(System.Net.Mail.MailMessage mailMessage);
        Task<bool> SendComunicationEmailAsync(string emailTo, string message);
        Task<bool> SendComunicationEmailAsync(string emailTo, string message, string subject);
     
    }
}
