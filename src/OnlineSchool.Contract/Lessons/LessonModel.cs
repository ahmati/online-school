using OnlineSchool.Contract.TeacherSubject;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace OnlineSchool.Contract.Lessons
{
    public class LessonModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime AuthDate { get; set; }
        public  int TeacherSubjectId { get; set; }
        public virtual TeacherSubjectModel TeacherSubject { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public ICollection<LessonMaterialModel> LessonMaterials { get; set; }
    }
}
