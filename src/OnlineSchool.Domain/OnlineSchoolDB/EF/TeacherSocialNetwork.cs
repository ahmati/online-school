using System;
using System.Collections.Generic;

namespace OnlineSchool.Domain.OnlineSchoolDB.EF
{
    public partial class TeacherSocialNetwork
    {
        public int TeacherId { get; set; }
        public int SocialNetworkId { get; set; }
        public string Link { get; set; }
        public DateTime AuthDate { get; set; }

        public virtual SocialNetwork SocialNetwork { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}
