using AutoMapper.Configuration;
using ItalWebConsulting.Infrastructure.Composition;
using ItalWebConsulting.Infrastructure.Comunication;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Hosting;

namespace ItalWebConsulting.Infrastructure.Mvc.Comunication
{
    public class EmailService: ItalWebConsulting.Infrastructure.Comunication.EmailService
    {
        
        private readonly IWebHostEnvironment env;
        //private readonly IConfigurationManager configuration;
        public EmailService(IWebHostEnvironment env, IConfigurationManager configuration)
            :base(configuration)
        {
           
            this.env = env;
            //this.configuration = configuration;
        }
        public override bool IsDevelopment { get => env.IsDevelopment(); set => base.IsDevelopment = value; }
        
    }
}
