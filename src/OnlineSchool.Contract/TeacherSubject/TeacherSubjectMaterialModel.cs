using OnlineSchool.Contract.Material;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace OnlineSchool.Contract.TeacherSubject
{
    public class TeacherSubjectMaterialModel
    {
        public  int Id { get; set; }
        public  int TeacherSubjectId { get; set; }
        public  int MaterialId { get; set; }
        public  DateTime AuthDate { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual TeacherSubjectModel TeacherSubject { get; set; }
        public virtual MaterialModel Material { get; set; }
    }
}
