using System;
using System.Collections.Generic;

namespace OnlineSchool.Domain.OnlineSchoolDB.EF
{
    public partial class SocialNetwork
    {
        public SocialNetwork()
        {
            TeacherSocialNetworks = new HashSet<TeacherSocialNetwork>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public string IconPath { get; set; }
        public bool Deleted { get; set; }
        public DateTime AuthDate { get; set; }

        public virtual ICollection<TeacherSocialNetwork> TeacherSocialNetworks { get; set; }
    }
}
