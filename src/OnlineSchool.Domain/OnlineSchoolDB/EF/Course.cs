using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace OnlineSchool.Domain.OnlineSchoolDB.EF
{
    public class Course
    {
        public Course()
        {
            Sessions = new HashSet<Session>();
            BookedCourses = new HashSet<BookedCourse>();
        }

        public virtual int Id { get; set; }
        public virtual int TeacherSubjectId { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }
        public virtual double Price { get; set; }
        public virtual bool IsPublished { get; set; }
        public virtual DateTime AuthDate { get; set; }
        public string ImagePath { get; set; }
        public virtual int AvailableSpots { get; set; }

        public virtual TeacherSubject TeacherSubject { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
        public virtual ICollection<BookedCourse> BookedCourses { get; set; }
    }
}
