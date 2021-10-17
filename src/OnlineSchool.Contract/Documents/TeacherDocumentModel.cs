using OnlineSchool.Contract.Teachers;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSchool.Contract.Documents
{
   public class TeacherDocumentModel : DocumentModel
    {
        public int TeacherId { get; set; }
        public TeacherModel Teacher { get; set; }
    }
}
