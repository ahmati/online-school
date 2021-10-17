using System;
using System.Collections.Generic;
using System.Text;

namespace ItalWebConsulting.Infrastructure.Newsletter
{
    public class NewsletterInput
    {
        public string ApiKey { get; set; }
        public List<string> From { get; set; }
        public Dictionary<string, Object> To { get; set; }
        public List<string> Attachment { get; set; }
        public string Subject { get; set; }
        public string Html { get; set; }
     
    }
}
