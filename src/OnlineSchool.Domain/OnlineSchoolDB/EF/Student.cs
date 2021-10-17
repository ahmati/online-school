using System;
using System.Collections.Generic;

namespace OnlineSchool.Domain.OnlineSchoolDB.EF
{
    public partial class Student
    {
        public Student()
        {
            StudentDocuments = new HashSet<StudentDocument>();
            StudentEnrollments = new HashSet<StudentEnrollment>();
            BookedCourses = new HashSet<BookedCourse>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public char Gender { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public DateTime AuthDate { get; set; }

        public virtual ICollection<BookedCourse> BookedCourses { get; set; }
        public virtual ICollection<StudentDocument> StudentDocuments { get; set; }
        public virtual ICollection<StudentEnrollment> StudentEnrollments { get; set; }
    }
}
