using System;
using System.Collections.Generic;

namespace OnlineSchool.Domain.OnlineSchoolDB.EF
{
    public partial class TeacherSubject
    {
        public TeacherSubject()
        {
            Lessons = new HashSet<Lesson>();
            TeacherSubjectMaterials = new HashSet<TeacherSubjectMaterial>();
            Courses = new HashSet<Course>();
        }

        public virtual int Id { get; set; }
        public virtual int TeacherId { get; set; }
        public virtual int SubjectId { get; set; }
        public virtual DateTime AuthDate { get; set; }
        public virtual bool Deleted { get; set; }

        public virtual Subject Subject { get; set; }
        public virtual Teacher Teacher { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
        public virtual ICollection<TeacherSubjectMaterial> TeacherSubjectMaterials { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}
