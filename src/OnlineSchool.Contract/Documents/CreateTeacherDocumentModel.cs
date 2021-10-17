using OnlineSchool.Contract.Infrastructure.FileUpload;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSchool.Contract.Documents
{
    public class CreateTeacherDocumentModel : FileBase
    {
        public int TeacherId { get; set; }
    }
}
