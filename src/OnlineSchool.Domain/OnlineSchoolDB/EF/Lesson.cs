using System;
using System.Collections.Generic;

namespace OnlineSchool.Domain.OnlineSchoolDB.EF
{
    public partial class Lesson
    {
        public Lesson()
        {
            LessonMaterials = new HashSet<LessonMaterial>();

        }

        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTime AuthDate { get; set; }
        public virtual int TeacherSubjectId { get; set; }
        public virtual TeacherSubject TeacherSubject { get; set; }

        public virtual ICollection<LessonMaterial> LessonMaterials { get; set; }

    }
}
