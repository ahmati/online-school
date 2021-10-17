using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NLog;
using OnlineSchool.Contract.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Infrastructure.FileUpload
{
    public class FileService : IFileService
    {
        private string AppRootPath { get; set; }
        private string FilesRootPath { get; set; }
        public ILogger<IFileService> Logger { get; set; }

        public FileService(IWebHostEnvironment env)
        {
            AppRootPath = env.ContentRootPath;
            FilesRootPath = Path.Combine(AppRootPath, "wwwroot");
        }

        public bool FileExists(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath));

            return File.Exists(filePath);
        }

        public FileOutput UploadSingle (IFormFile file, params string[] subDirectories)
        {
            var output = new FileOutput();

            if (string.IsNullOrWhiteSpace(AppRootPath))
                throw new ArgumentNullException(nameof(AppRootPath));

            try
            {
                if (file != null)
                {
                    var destinationDir = AppRootPath;

                    foreach (var subDir in subDirectories)
                    {
                        destinationDir += Path.Combine(destinationDir, subDir);
                        if (!Directory.Exists(destinationDir))
                            Directory.CreateDirectory(destinationDir);
                    }

                    var filePath = Path.Combine(destinationDir, file.FileName);

                    if (File.Exists(filePath))
                        File.Delete(filePath);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyToAsync(stream);
                    }
                }
            }
            catch (Exception ex)
            {
                output.AddError(ex.Message);
            }

            return output;
        }

        public async Task<FileOutput> UploadSingleAsync (IFormFile file, string destination)
        {
            var output = new FileOutput();

            if (string.IsNullOrWhiteSpace(AppRootPath))
                throw new ArgumentNullException(nameof(AppRootPath));

            try
            {
                if (file != null)
                {
                    var destinationDir = AppRootPath;

                    destinationDir = Path.Combine(destinationDir, destination);
                    if (!Directory.Exists(destinationDir))
                        Directory.CreateDirectory(destinationDir);

                    var filePath = Path.Combine(destinationDir, file.FileName);

                    if (File.Exists(filePath))
                        File.Delete(filePath);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }
            catch (Exception ex)
            {
                output.AddError(ex.Message);
                return output;
            }

            return output;
        }

        public async Task UploadSingleAsync_Chunked(IEnumerable<IFormFile> chunks, string fullPath)
        {
            try
            {
                using (FileStream stream = new FileStream(fullPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                {
                    foreach(var chunk in chunks)
                    {
                        await chunk.CopyToAsync(stream);
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.LogError(ex.Message);
            }
        }
    }
}
