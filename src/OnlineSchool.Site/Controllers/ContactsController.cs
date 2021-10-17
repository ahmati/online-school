using ItalWebConsulting.Infrastructure.Composition;
using ItalWebConsulting.Infrastructure.Comunication;
using Microsoft.AspNetCore.Mvc;
using OnlineSchool.Contract.Contacts;
using OnlineSchool.Contract.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineSchool.Site.Controllers
{
    public class ContactsController : Controller
    {
        private readonly SmtpSettings SmptSettings;

        protected readonly IConfigurationManager Configuration;
        public Lazy<IEmailService> EmailService { get; set; }

        public ContactsController(IConfigurationManager configuration)
        {
            Configuration = configuration;
            SmptSettings = Configuration.SmtpSettings;
        }

        [HttpPost]
        public async Task<ActionResult> Create_ContactRequest(string message, string fullName, string email)
        {
            if (ModelState.IsValid)
            {
                
                var mail = SmptSettings.SendInfoTo;
                    
               // var mail = await ContactService.GetAgencyMailAsync(Id);
                var subject = SharedResources.Contact;
                
                var userMessage = EmailSharedResources.ContactMessage;
                userMessage = userMessage.Replace("fullName", fullName);
                userMessage = userMessage.Replace("email", email);
                userMessage = userMessage.Replace("message", message);

                await EmailService.Value.SendComunicationEmailAsync(mail, userMessage, subject);
            }
            // contactRequestModel.Message = "Thankyou that you contact us!";
            return Json(true);
        }
    }
}
