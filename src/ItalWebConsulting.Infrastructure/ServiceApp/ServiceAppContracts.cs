using Autofac;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ItalWebConsulting.Infrastructure.ServiceApp
{
    public class ConfigAppInput
    {
        /// <summary>
        /// Method for checking if this service is enable or disable. The method receive the keyService in input and return true if the service is enable.  
        /// </summary>
        public Func<bool,string> CheckIfBatchIsActive { get; set; }
        public Func<Type,IContainer> ConfigureStandardAutofact { get; set; }
        public bool AddNLogConfiguration { get; set; }
        public bool AddSmtpSettingsConfiguration { get; set; }
        public string[] BatchArgs { get; set; }
        public IEnumerable<Assembly> AssemblyContainerBatch { get; set; }
    }

    public enum ServiceAppResult
    {
        Ok = 0,
        Warn = 1,
        Error = -1,
        ServiceDisable = -10
    }
}
