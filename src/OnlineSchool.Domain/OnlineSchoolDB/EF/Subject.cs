using System;
using System.Collections.Generic;

namespace OnlineSchool.Domain.OnlineSchoolDB.EF
{
    public partial class Subject
    {
        public Subject()
        {
            Schedules = new HashSet<Schedule>();
            TeacherSubjects = new HashSet<TeacherSubject>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte CategoryId { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }
        public DateTime AuthDate { get; set; }

        public virtual SubjectCategory Category { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
        public virtual ICollection<TeacherSubject> TeacherSubjects { get; set; }
    }
}
