using OnlineSchool.Contract.Students;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineSchool.Contract.Documents
{
    public class StudentDocumentModel : DocumentModel
    {
        public int StudentId { get; set; }
        public StudentModel Student { get; set; }
    }
}
