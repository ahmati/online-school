using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace OnlineSchool.Contract.Upload
{
    [DataContract]
    public class ChunkMetaData
    {
        [DataMember(Name = "uploadUid")]
        public string UploadUid { get; set; }
        [DataMember(Name = "fileName")]
        public string FileName { get; set; }
        [DataMember(Name = "relativePath")]
        public string RelativePath { get; set; }
        [DataMember(Name = "contentType")]
        public string ContentType { get; set; }
        [DataMember(Name = "chunkIndex")]
        public long ChunkIndex { get; set; }
        [DataMember(Name = "totalChunks")]
        public long TotalChunks { get; set; }
        [DataMember(Name = "totalFileSize")]
        public long TotalFileSize { get; set; }
    }
    public class FileResult
    {
        public bool Uploaded { get; set; }
        public string FileUid { get; set; }
        public string FileName { get; set; }
        public bool Exist { get; set; }
    }
}
