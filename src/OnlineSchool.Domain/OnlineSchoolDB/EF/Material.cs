using OnlineSchool.Contract.Infrastructure.FileUpload;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineSchool.Domain.OnlineSchoolDB.EF
{
    public partial class Material : FileBase
    {
        public Material()
        {
            LessonMaterials = new HashSet<LessonMaterial>();
            TeacherSubjectMaterials = new HashSet<TeacherSubjectMaterial>();
            SpotMeetingMaterials = new HashSet<SpotMeetingMaterial>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime AuthDate { get; set; }

        public virtual ICollection<LessonMaterial> LessonMaterials { get; set; }
        public virtual ICollection<TeacherSubjectMaterial> TeacherSubjectMaterials { get; set; }
        public virtual ICollection<SpotMeetingMaterial> SpotMeetingMaterials { get; set; }

    }
}
