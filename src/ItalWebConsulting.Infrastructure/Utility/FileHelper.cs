using OnlineSchool.Contract.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ItalWebConsulting.Infrastructure.Utility
{
  public static  class FileHelper
    {
        //get list file bu id add suela
        public static FileOutput Save_File(IEnumerable<IFormFile> Files, string rootPath, int? Id)
        {
            var output = new FileOutput();
            try
            {
                if (Files != null)
                {
                    foreach (var file in Files)
                    {
                        var pathDirectory = Path.Combine(rootPath, @"uploads", "Document_" + Id);
                        if (!System.IO.Directory.Exists(pathDirectory))
                            System.IO.Directory.CreateDirectory(pathDirectory);
                        var imgPath = Path.Combine(pathDirectory, file.FileName);
                        if (System.IO.File.Exists(imgPath))
                            System.IO.File.Delete(imgPath);
                        using (var stream = new FileStream(imgPath, FileMode.Create))
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
            return output;
        }

        
        public static FileOutput Delete_File(IEnumerable<string> filename, string rootPath, int? Id)
        {
            var output = new FileOutput();
            try
            {
                if (filename != null)
                {
                    foreach (var fileName in filename)
                    {

                        var pathDirectory = Path.Combine(rootPath, @"uploads");
                        if (!System.IO.Directory.Exists(pathDirectory))
                            System.IO.Directory.CreateDirectory(pathDirectory);
                        var imgPath = Path.Combine(pathDirectory, fileName);

                        if (System.IO.File.Exists(imgPath))
                            System.IO.File.Delete(imgPath);

                    }
                }
            }
            catch (Exception ex)
            {
                output.AddError(ex.Message);
            }
            return output;
        }

        public static List<string> Get_Files(string rootPath, int? Id)
        {
            var list_files = new List<string>();
            var fileDirectory = Path.Combine(@"uploads", "Document_" + Id);
            var pathDirectory = Path.Combine(rootPath, fileDirectory);
            if (!System.IO.Directory.Exists(pathDirectory))
                return list_files;
            var files = Directory.GetFiles(pathDirectory).ToList();
            foreach (var file in files)
            {
                list_files.Add(Path.Combine( Path.GetFileName(file)));

            }
            return list_files;
        }
   }
}
