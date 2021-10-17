using OnlineSchool.Contract.Infrastructure.FileUpload;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSchool.Contract.Documents
{
    public class CreateStudentDocumentModel : FileBase
    {
        public int StudentId { get; set; }
    }
}
