using System;
using System.Collections.Generic;
using System.Text;

namespace ItalWebConsulting.Infrastructure.Logging
{
   public class NLogConfiguration
    {
        public string LblAmbiente { get; set; }
        public string FileName { get; set; }
        public string MailAddressesOnError { get; set; }
    }
}
