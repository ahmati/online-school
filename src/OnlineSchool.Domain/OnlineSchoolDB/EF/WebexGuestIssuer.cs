using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSchool.Domain.OnlineSchoolDB.EF
{
    public partial class WebexGuestIssuer
    {
        public WebexGuestIssuer()
        {
        }

        public virtual string Id { get; set; }
        public virtual string Secret { get; set; }
    }
}
