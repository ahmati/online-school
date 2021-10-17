using OnlineSchool.Contract.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OnlineSchool.Contract.Utility
{
    public class CreateZipFileInput
    {
        public IEnumerable<string> FilesName { get; set; }
        public string ContentRootPath { get; set; }
    }

    public class CreateZipFileOutput: ResponseBase<bool>
    {
        public string ZipPathFile { get; set; }
    }
    
    public class ImageOutput: ResponseBase<bool>
    { 
    }
    public class AgencyImageOutput : ResponseBase<bool>
    {
    }
    public class FileOutput : ResponseBase<bool>
    {
    }
}
