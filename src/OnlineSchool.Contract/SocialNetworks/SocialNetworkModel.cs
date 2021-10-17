using OnlineSchool.Contract.Teachers;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSchool.Contract.SocialNetworks
{
  public  class SocialNetworkModel
    {
        public SocialNetworkModel()
        {
            TeacherSocialNetwork = new HashSet<TeacherSocialNetworkModel>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public string IconPath { get; set; }
        public bool Deleted { get; set; }
        public DateTime AuthDate { get; set; }

        public virtual ICollection<TeacherSocialNetworkModel> TeacherSocialNetwork { get; set; }
    }
}
