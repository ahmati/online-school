using System;
using System.Collections.Generic;

namespace OnlineSchool.Domain.OnlineSchoolDB.EF
{
    public partial class DocumentType
    {
        public DocumentType()
        {
            StudentDocuments = new HashSet<StudentDocument>();
            TeacherDocuments = new HashSet<TeacherDocument>();
        }

        public int Id { get; set; }
        public string Value { get; set; }
        public string Code { get; set; }

        public virtual ICollection<StudentDocument> StudentDocuments { get; set; }
        public virtual ICollection<TeacherDocument> TeacherDocuments { get; set; }
    }
}
