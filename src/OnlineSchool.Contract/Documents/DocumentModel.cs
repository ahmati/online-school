using OnlineSchool.Contract.Infrastructure.FileUpload;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSchool.Contract.Documents
{
    public class DocumentModel : FileBase
    {
        public int Id { get; set; }
        public int? DocumentTypeId { get; set; }
        public DateTime AuthDate { get; set; }

        public DocumentTypeModel DocumentType { get; set; }
    }
}
