using ItalWebConsulting.Infrastructure.Composition;
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using Xunit;

namespace ItalWebConsulting.Infrastructure.Comunication
{
    
    //public static class DefaultEmailConfiguration
    //{
    //    public static EmailSender DefaultSender { get {
    //            return new EmailSender
    //            {
    //                EmailAdress = "",
    //                Name = ""
    //            };
    //        } }
    //}
    public class SmtpSettings
    {
       
        public string Server { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public bool UseSsl { get; set; }
        public EmailSender DefaultSender { get; set; }
        public string SendInfoTo { get; set; }


        //public string SenderName { get; set; }
        //public string Sender { get; set; }
    }

    public class EmailInput
    {
        public IEnumerable<string> AdressEmailTo { get; set; }
        public IEnumerable<string> AdressEmailCc { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public IEnumerable<string> Attachments { get; set; } 

        public EmailSender Sender { get; set; }
    }
    public struct EmailSender
    {
        public string Name { get; set; }
        public string EmailAdress { get; set; }
    }
}
