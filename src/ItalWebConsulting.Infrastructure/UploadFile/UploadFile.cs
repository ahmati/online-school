using Microsoft.AspNetCore.Http;
using OnlineSchool.Contract.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Text;
using System.Reflection;

namespace ItalWebConsulting.Infrastructure.UploadFile
{

    public static class UploadFile
    {
        public static FileOutput Save_File(IEnumerable<IFormFile> files, string rootPath,int? subjectId)
        {
            var output = new FileOutput();
            if (string.IsNullOrWhiteSpace(rootPath))
                throw new ArgumentNullException(nameof(rootPath));

            // The Name of the Upload component is "files"
            try
            {
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        var pathDirectory = Path.Combine(rootPath, @"upload", "SubjectPdf_" + subjectId);
                        if (!System.IO.Directory.Exists(pathDirectory))
                            System.IO.Directory.CreateDirectory(pathDirectory);
                        var filePath = Path.Combine(pathDirectory, file.FileName);


                        if (System.IO.File.Exists(filePath))
                            System.IO.File.Delete(filePath);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                output.AddError(ex.Message);
            }
            // Return an empty string to signify success
            return output;
        }

        public static FileOutput Save_Video(IEnumerable<IFormFile> files, string rootPath, int? subjectId)
        {
            var output = new FileOutput();
            if (string.IsNullOrWhiteSpace(rootPath))
                throw new ArgumentNullException(nameof(rootPath));

            // The Name of the Upload component is "files"
            try
            {
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        var pathDirectory = Path.Combine(rootPath, @"upload", "SubjectVideo_" + subjectId);
                        if (!System.IO.Directory.Exists(pathDirectory))
                            System.IO.Directory.CreateDirectory(pathDirectory);
                        var filePath = Path.Combine(pathDirectory, file.FileName);


                        if (System.IO.File.Exists(filePath))
                            System.IO.File.Delete(filePath);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                output.AddError(ex.Message);
            }
            // Return an empty string to signify success
            return output;
        }

        public static FileOutput Save_Images(IEnumerable<IFormFile> files, string rootPath, int? subjectId, int? id)
        {
            var output = new FileOutput();
            if (string.IsNullOrWhiteSpace(rootPath))
                throw new ArgumentNullException(nameof(rootPath));

            // The Name of the Upload component is "files"
            try
            {
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        var pathDirectory = Path.Combine(rootPath, @"upload", "Subject_" + subjectId, "Images_" + id);
                        if (!System.IO.Directory.Exists(pathDirectory))
                            System.IO.Directory.CreateDirectory(pathDirectory);
                        var filePath = Path.Combine(pathDirectory, file.FileName);


                        if (System.IO.File.Exists(filePath))
                            System.IO.File.Delete(filePath);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                output.AddError(ex.Message);
            }
            // Return an empty string to signify success
            return output;
        }
    }
}


