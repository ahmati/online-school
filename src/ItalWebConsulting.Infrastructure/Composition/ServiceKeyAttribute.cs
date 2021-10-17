using System;
using System.Collections.Generic;
using System.Text;

namespace ItalWebConsulting.Infrastructure.Composition
{
    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class ServiceKeyAttribute : Attribute
    {
        public ServiceKeyAttribute(string serviceKey, string shortServiceDescription)
        {
            if (string.IsNullOrWhiteSpace(serviceKey))
                throw new ArgumentNullException(nameof(serviceKey));
            serviceKey = serviceKey.Trim();

            var infSk = 4;
            var supSk = 50;

            if (serviceKey.Length < infSk || serviceKey.Length > supSk)
                throw new ArgumentException(string.Format("this parameter must be {0} and {1} characters length", infSk, supSk), nameof(serviceKey));

            if (string.IsNullOrWhiteSpace(shortServiceDescription))
                throw new ArgumentNullException(nameof(shortServiceDescription));
            shortServiceDescription = shortServiceDescription.Trim();
            //var infSd = 10;
            //var supSd = 30;
            //if (shortServiceDescription.Length < infSd || shortServiceDescription.Length > supSd)
            //    throw new ArgumentException(string.Format("this parameter must be {0} and {1} characters length", infSd, supSd), nameof(shortServiceDescription));


            ServiceKey = serviceKey;
            ShortServiceDescription = shortServiceDescription;
        }

        public string ServiceKey
        {
            get;
            private set;
        }

        public string ShortServiceDescription
        {
            get;
            private set;
        }

       
      
    }
}
