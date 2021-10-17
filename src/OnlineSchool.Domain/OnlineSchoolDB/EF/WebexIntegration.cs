using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSchool.Domain.OnlineSchoolDB.EF
{
    public partial class WebexIntegration
    {
        public WebexIntegration()
        {
        }

        public virtual string ClientId { get; set; }
        public virtual string ClientSecret { get; set; }
        public virtual string RedirectUri { get; set; }
        public virtual string AccessToken { get; set; }
        public virtual long ExpiresIn { get; set; }
        public virtual DateTime LastUpdated { get; set; }
    }
}
