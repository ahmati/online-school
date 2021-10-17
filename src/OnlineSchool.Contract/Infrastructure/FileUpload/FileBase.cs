using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSchool.Contract.Infrastructure.FileUpload
{
    public class FileBase
    {
        public string FileName { get; set; }
        public string MimeType { get; set; }
        public int FileSize { get; set; }
    }
}
