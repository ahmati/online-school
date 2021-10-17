using OnlineSchool.Contract.Lessons;
using OnlineSchool.Contract.Material;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSchool.Contract.Lessons
{
    public class LessonMaterialModel
    {
        public  int Id { get; set; }
        public  int LessonId { get; set; }
        public  int MaterialId { get; set; }
        public  DateTime AuthDate { get; set; }

        public virtual LessonModel Lesson { get; set; }
        public virtual MaterialModel Material { get; set; }
    }
}
