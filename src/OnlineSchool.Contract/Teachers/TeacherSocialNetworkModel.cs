using OnlineSchool.Contract.SocialNetworks;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSchool.Contract.Teachers
{
   public class TeacherSocialNetworkModel
    {
        public int TeacherId { get; set; }
        public int SocialNetworkId { get; set; }
        public string Link { get; set; }
        public DateTime AuthDate { get; set; }

        public virtual SocialNetworkModel SocialNetwork { get; set; }
        public virtual TeacherModel Teacher { get; set; }
    }
}
