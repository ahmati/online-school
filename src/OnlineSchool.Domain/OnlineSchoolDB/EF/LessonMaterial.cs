using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OnlineSchool.Domain.OnlineSchoolDB.EF
{
    public class LessonMaterial
    {
        public LessonMaterial()
        {
        }
        public virtual int Id { get; set; }
        public virtual int LessonId { get; set; }
        public virtual int MaterialId { get; set; }
        public virtual DateTime AuthDate { get; set; }

        public virtual Lesson Lesson { get; set; }
        public virtual Material Material { get; set; }
    }
}
