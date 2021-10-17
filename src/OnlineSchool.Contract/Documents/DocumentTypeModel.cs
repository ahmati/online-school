using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSchool.Contract.Documents
{
   public  class DocumentTypeModel
    {
        public DocumentTypeModel()
        {
            StudentDocuments = new HashSet<StudentDocumentModel>();
            TeacherDocuments = new HashSet<TeacherDocumentModel>();
        }

        public int Id { get; set; }
        public string Value { get; set; }
        public string  Code { get; set; }

        public ICollection<StudentDocumentModel> StudentDocuments { get; set; }
        public ICollection<TeacherDocumentModel> TeacherDocuments { get; set; }
    }
}
