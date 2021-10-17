using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OnlineSchool.Domain.OnlineSchoolDB.EF
{
    public class TeacherSubjectMaterial
    {
        public TeacherSubjectMaterial()
        {
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }
        public virtual int TeacherSubjectId { get; set; }
        public virtual int MaterialId { get; set; }
        public virtual DateTime AuthDate { get; set; }

        public virtual TeacherSubject TeacherSubject { get; set; }
        public virtual Material Material { get; set; }
    }
}
