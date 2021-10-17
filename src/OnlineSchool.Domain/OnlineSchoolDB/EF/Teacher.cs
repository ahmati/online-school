using System;
using System.Collections.Generic;

namespace OnlineSchool.Domain.OnlineSchoolDB.EF
{
    public partial class Teacher
    {
        public Teacher()
        {
            TeacherDocuments = new HashSet<TeacherDocument>();
            Schedules = new HashSet<Schedule>();
            TeacherSocialNetworks = new HashSet<TeacherSocialNetwork>();
            TeacherSubjects = new HashSet<TeacherSubject>();
            SpotMeetingTeachers = new HashSet<SpotMeetingTeacher>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public char Gender { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public DateTime AuthDate { get; set; }

        public virtual ICollection<TeacherDocument> TeacherDocuments { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
        public virtual ICollection<TeacherSocialNetwork> TeacherSocialNetworks { get; set; }
        public virtual ICollection<TeacherSubject> TeacherSubjects { get; set; }
        public virtual ICollection<SpotMeetingTeacher> SpotMeetingTeachers { get; set; }
    }
}
