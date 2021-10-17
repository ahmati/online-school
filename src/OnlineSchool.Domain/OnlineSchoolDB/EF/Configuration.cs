using System;
using System.Collections.Generic;

namespace OnlineSchool.Domain.OnlineSchoolDB.EF
{
    public partial class Configuration
    {
        public string StripeKeyName { get; set; }
        public string Value { get; set; }
        public DateTime? CreationDate { get; set; }
    }
}
