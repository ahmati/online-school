using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSchool.Contract.Infrastructure.FileUpload
{
    public class ChunkMetaData
    {
        public string UploadUid { get; set; }
        public string FileName { get; set; }
        public string RelativePath { get; set; }
        public string ContentType { get; set; }
        public long ChunkIndex { get; set; }
        public long TotalChunks { get; set; }
        public long TotalFileSize { get; set; }
    }
}
