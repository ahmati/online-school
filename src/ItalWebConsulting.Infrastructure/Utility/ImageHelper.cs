using OnlineSchool.Contract.Utility;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Runtime.Serialization.Json;
using OnlineSchool.Contract.Upload;

namespace ItalWebConsulting.Infrastructure.Utility
{
    public static class ImageHelper
    {
        public static int index = 0;

        public static ImageOutput Save_File(IEnumerable<IFormFile> files, string rootPath)
        {
            var output = new ImageOutput();
            try
            {
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        var pathDirectory = Path.Combine(rootPath, @"Subject_"+ file.FileName);
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
            // Return an empty string to signify success
            return output;
        }

        public static FileResult ChunkFileSave(IEnumerable<IFormFile> Images, string rootPath, int id, string metaData)
        { 

            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(metaData)))
            {
                var serializer = new DataContractJsonSerializer(typeof(ChunkMetaData));
                ChunkMetaData somemetaData = serializer.ReadObject(ms) as ChunkMetaData;
                string path = String.Empty;
                foreach (var file in Images)
                {
                    index++;
                    path = Path.Combine(rootPath, @"uploads", "Material" + id);
                    if (!System.IO.Directory.Exists(path))
                        System.IO.Directory.CreateDirectory(path);
                    var imgPath = Path.Combine(path);
                    if (index == 1)
                        if (System.IO.File.Exists(imgPath))
                            System.IO.File.Delete(imgPath);
                    var ms2 = new MemoryStream();
                    file.CopyTo(ms2);
                    var fileBytes = ms2.ToArray();
                    using (FileStream filestream = new FileStream(imgPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                    {
                        using (var stream = new MemoryStream(fileBytes))
                        {
                            stream.CopyTo(filestream);
                        }
                    }
                }
                FileResult fileBlob = new FileResult();
                fileBlob.Uploaded = somemetaData.TotalChunks - 1 <= somemetaData.ChunkIndex;
                fileBlob.FileUid = somemetaData.UploadUid;
                fileBlob.FileName = somemetaData.FileName;
                if (fileBlob.Uploaded)
                    index = 0;
                return fileBlob;
            }
        }
        public static AgencyImageOutput Save_ImageAgency(IEnumerable<IFormFile> Images, string rootPath, int? Id)
        {
            var output = new AgencyImageOutput();
            try
            {
                if (Images != null)
                {
                    foreach (var Image in Images)
                    {

                        var pathDirectory = Path.Combine(rootPath, @"AgencyImages", "image_" + Id);
                        if (!System.IO.Directory.Exists(pathDirectory))
                            System.IO.Directory.CreateDirectory(pathDirectory);
                        var imgPath = Path.Combine(pathDirectory, Image.FileName);

                        if (System.IO.File.Exists(imgPath))
                            System.IO.File.Delete(imgPath);
                        using (var stream = new FileStream(imgPath, FileMode.Create))
                        {
                            Image.CopyTo(stream);
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
        public static AgencyImageOutput Save_AgentImage(IEnumerable<IFormFile> Images, string rootPath, int? Id)
        {
            var output = new AgencyImageOutput();
            try
            {
                if (Images != null)
                {
                    foreach (var Image in Images)
                    {

                        var pathDirectory = Path.Combine(rootPath, @"AgentImages", "image_" + Id);
                        if (!System.IO.Directory.Exists(pathDirectory))
                            System.IO.Directory.CreateDirectory(pathDirectory);
                        var imgPath = Path.Combine(pathDirectory, Image.FileName);

                        if (System.IO.File.Exists(imgPath))
                            System.IO.File.Delete(imgPath);
                        using (var stream = new FileStream(imgPath, FileMode.Create))
                        {
                            Image.CopyTo(stream);
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

        public static AgencyImageOutput Delete_AgencyImage(IEnumerable<string> ImagesName, string rootPath, int? Id)
        {
            var output = new AgencyImageOutput();
            try
            {
                if (ImagesName != null)
                {
                    foreach (var ImageName in ImagesName)
                    {

                        var pathDirectory = Path.Combine(rootPath, @"AgentImages", "image_" + Id);
                        if (!System.IO.Directory.Exists(pathDirectory))
                            System.IO.Directory.CreateDirectory(pathDirectory);
                        var imgPath = Path.Combine(pathDirectory, ImageName);

                        if (System.IO.File.Exists(imgPath))
                            System.IO.File.Delete(imgPath);

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

        public static ImageOutput Delete_Image(string ImagesName, string rootPath, int? Id)
        {
            var output = new ImageOutput();
            try
            {
                if (ImagesName != null)
                {
                    var pathDirectory = Path.Combine(rootPath, @"upload", "Material" + Id);
                    if (!System.IO.Directory.Exists(pathDirectory))
                        System.IO.Directory.CreateDirectory(pathDirectory);
                    var imgPath = Path.Combine(pathDirectory, ImagesName);
                    if (System.IO.File.Exists(imgPath))
                        System.IO.File.Delete(imgPath);
                }
            }
            catch (Exception ex)
            {
                output.AddError(ex.Message);
            }
            return output;
        }

        public static AgencyImageOutput Delete_AgentImage(IEnumerable<string> ImagesName, string rootPath, int? Id)
        {
            var output = new AgencyImageOutput();
            try
            {
                if (ImagesName != null)
                {
                    foreach (var ImageName in ImagesName)
                    {

                        var pathDirectory = Path.Combine(rootPath, @"AgencyImages", "image_" + Id);
                        if (!System.IO.Directory.Exists(pathDirectory))
                            System.IO.Directory.CreateDirectory(pathDirectory);
                        var imgPath = Path.Combine(pathDirectory, ImageName);

                        if (System.IO.File.Exists(imgPath))
                            System.IO.File.Delete(imgPath);

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


        public static List<string> Get_Images(string rootPath, int? Id)
        {
            var list_files = new List<string>();
            var imgDirectory= Path.Combine(@"upload", "PropertyImage_" + Id);
            var pathDirectory = Path.Combine(rootPath, imgDirectory);
            if (!System.IO.Directory.Exists(pathDirectory))
                return list_files;
                //throw new FileNotFoundException(nameof(pathDirectory));

            var files = Directory.GetFiles(pathDirectory).ToList();
            foreach(var file in files)
            {
                list_files.Add(Path.Combine(imgDirectory,Path.GetFileName(file)));

            }
            return list_files;
        }
        public static List<string> Get_ImagesFile(string rootPath, int? Id)
        {
            var list_files = new List<string>();
            var imgDirectory = Path.Combine(@"upload", "PropertyImage_" + Id);
            var pathDirectory = Path.Combine(rootPath, imgDirectory);
            if (!System.IO.Directory.Exists(pathDirectory))
                return list_files;
            //throw new FileNotFoundException(nameof(pathDirectory));
            //var extension = Path.GetExtension(pathDirectory);
            var files = Directory.GetFiles(pathDirectory).ToList();
            foreach (var file in files)
            {
                var extension = Path.GetExtension(file);
                if (extension!=".mp4")
                {
                    list_files.Add(System.IO.Path.GetFileName(file));
                }
            }
            return list_files;
        }
        
        public static List<string> Get_VideosFile(string rootPath, int? Id)
        {
            var list_files = new List<string>();
            var imgDirectory = Path.Combine(@"upload", "PropertyImage_" + Id);
            var pathDirectory = Path.Combine(rootPath, imgDirectory);
            if (!System.IO.Directory.Exists(pathDirectory))
                return list_files;
            //throw new FileNotFoundException(nameof(pathDirectory));
            //var extension = Path.GetExtension(pathDirectory);
            var files = Directory.GetFiles(pathDirectory).ToList();
            foreach (var file in files)
            {
                var extension = Path.GetExtension(file);
                if (extension == ".mp4")
                {
                    list_files.Add(System.IO.Path.GetFileName(file));
                }
            }
            return list_files;
        }
        public static List<string> Get_AgencyImages(string rootPath, int? Id)
        {
            var list_files = new List<string>();
            var imgDirectory = Path.Combine(@"AgencyImages", "image_" + Id);
            var pathDirectory = Path.Combine(rootPath, imgDirectory);
            if (!System.IO.Directory.Exists(pathDirectory))
                return list_files;

            var files = Directory.GetFiles(pathDirectory).ToList();
            foreach (var file in files)
            {
                list_files.Add(System.IO.Path.GetFileName(file));

            }
            return list_files;
        }

        public static List<string> Get_AgentImages(string rootPath, int? Id)
        {
            var list_files = new List<string>();
            var imgDirectory = Path.Combine(@"AgentImages", "image_" + Id);
            var pathDirectory = Path.Combine(rootPath, imgDirectory);
            if (!System.IO.Directory.Exists(pathDirectory))
                return list_files;

            var files = Directory.GetFiles(pathDirectory).ToList();
            foreach (var file in files)
            {
                list_files.Add(System.IO.Path.GetFileName(file));

            }
            return list_files;
        }


    }
}
