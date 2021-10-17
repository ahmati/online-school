using Microsoft.AspNetCore.Http;
using OnlineSchool.Contract.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Infrastructure.FileUpload
{
    /// <summary>
    /// This is a .NET Core 3+ service, that manages file uploads under the location: <code> wwwroot/uploads </code>
    /// </summary>
    public interface IFileService
    {
        FileOutput UploadSingle (IFormFile file, params string[] subDirectories);
        Task<FileOutput> UploadSingleAsync (IFormFile file, string directory);
        Task UploadSingleAsync_Chunked(IEnumerable<IFormFile> files, string fullPath);
        bool FileExists(string filePath);
    }
}
