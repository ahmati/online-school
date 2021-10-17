using OnlineSchool.Contract.Subject;
using OnlineSchool.Contract.TeacherSubject;
using OnlineSchool.Contract.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineSchool.Contract.Teachers
{
   public class TeacherModel : UserModel
    {
        public TeacherModel()
        {
            TeacherSocialNetwork = new HashSet<TeacherSocialNetworkModel>();
            TeacherSubject = new HashSet<TeacherSubjectModel>();
        }

        public DateTime AuthDate { get; set; }

        public ICollection<TeacherSocialNetworkModel> TeacherSocialNetwork { get; set; }
        public ICollection<TeacherSubjectModel> TeacherSubject { get; set; }

    }
}
