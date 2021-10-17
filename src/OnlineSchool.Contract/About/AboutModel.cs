using OnlineSchool.Contract.Teachers;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSchool.Contract.About
{
      public class AboutModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string TeachingArea { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string TeacherCv { get; set; }

        public DateTime AuthDate { get; set; }

        public virtual ICollection<TeacherSocialNetworkModel> TeacherSocialNetwork { get; set; }

        public int subjectId { get; set; }
        public string subjectName { get; set; }
        public decimal Price { get; set; }
        public string stripeToken { get; set; }
        public string stripeEmail { get; set; }
        public string Time { get; set; }
        public string subjectDescription { get; set; }
        public string Color { get; set; }
        public DateTime subjectAuthDate { get; set; }
        public int TeacherSubjectId { get; set; }
        public string Semester { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Year { get; set; }
        public int SubjectTermsId { get; set; }
        public string DayOftheWeek { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string MaterialId { get; set; }
    }
    
}
