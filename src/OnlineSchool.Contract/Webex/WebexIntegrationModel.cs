using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSchool.Contract.Webex
{
    public class WebexIntegrationModel
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RedirectUri { get; set; }
        public string AccessToken { get; set; }
        public long ExpiresIn { get; set; }
        public DateTime LastUpdated { get; set; }
        public int DaysUntilExpiration { get => GetDaysUntilExpiration(); }

        private int GetDaysUntilExpiration()
        {
            var daysToExpire = (int)(LastUpdated.AddSeconds(ExpiresIn) - DateTime.Now).TotalDays;
            return (daysToExpire > 0) ? daysToExpire : 0;
        }
    }
}
