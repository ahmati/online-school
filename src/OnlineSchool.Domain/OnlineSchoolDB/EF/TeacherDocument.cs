using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineSchool.Domain.OnlineSchoolDB.EF
{
    public partial class TeacherDocument
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? DocumentTypeId { get; set; }
        public int TeacherId { get; set; }
        public string FileName { get; set; }
        public string MimeType { get; set; }
        public int FileSize { get; set; }
        public DateTime AuthDate { get; set; }

        public virtual DocumentType DocumentType { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}
