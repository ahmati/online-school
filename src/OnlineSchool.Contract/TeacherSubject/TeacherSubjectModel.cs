using OnlineSchool.Contract.Lessons;
using OnlineSchool.Contract.Subject;
using OnlineSchool.Contract.Teachers;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace OnlineSchool.Contract.TeacherSubject
{
   public class TeacherSubjectModel
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public int SubjectId { get; set; }
        public DateTime AuthDate { get; set; }
        public bool Deleted { get; set; } 

        public TeacherModel Teacher { get; set; }
        public SubjectModel Subject { get; set; }
        public LessonModel Lesson { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public ICollection<TeacherSubjectMaterialModel> TeacherSubjectMaterials { get; set; }
    }
}
