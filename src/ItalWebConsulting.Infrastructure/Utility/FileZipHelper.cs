using OnlineSchool.Contract.Utility;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using OnlineSchool.Contract;
using System.IO.Compression;

namespace ItalWebConsulting.Infrastructure.Utility
{
    public static class FileZipHelper
    {
        public static CreateZipFileOutput CreateZipFile(CreateZipFileInput input)
        {
            var retVal = new CreateZipFileOutput();
            try
            {
                if (input == null)
                    throw new ArgumentNullException(nameof(input));
                if (input.FilesName == null || !input.FilesName.Any())
                    throw new ArgumentNullException(nameof(input.FilesName));
                if (string.IsNullOrWhiteSpace(input.ContentRootPath))
                    throw new ArgumentNullException(nameof(input.ContentRootPath));
                var folderTmp = Path.Combine(input.ContentRootPath, ConfigurationConsts.TempFolder);
                if (!System.IO.Directory.Exists(folderTmp))
                    System.IO.Directory.CreateDirectory(folderTmp);
                var fileZip = Guid.NewGuid().ToString() + ".zip";
                var tmpFile = Path.Combine(folderTmp, fileZip);
                if (System.IO.File.Exists(tmpFile))
                    System.IO.File.Delete(tmpFile);
                retVal.ZipPathFile = tmpFile;
               // Directory.EnumerateFiles(folderTmp).ToList().ForEach(f => System.IO.File.Delete(f));
                using (var zip = ZipFile.Open(tmpFile, ZipArchiveMode.Create))
                    foreach (var fileName in input.FilesName)
                    {
                        var fullPath = Path.Combine(input.ContentRootPath, ConfigurationConsts.WwwrootFolder, ConfigurationConsts.UploadFolder, fileName);
                        if (!File.Exists(fullPath))
                            throw new FileNotFoundException(fullPath);
                        zip.CreateEntryFromFile(fullPath, fileName);
                    }
            }
            catch (Exception ex)
            {
                retVal.AddError(ex.ToString());
            }
            return retVal;
        }
    }

   
    //filename + dataAquizzacione add suela
    public static class FileExtensions
    {
        public static string FileAppendTimeStamp(this string fileName)
        {
            return string.Concat(
                Path.GetFileNameWithoutExtension(fileName),
                DateTime.Now.ToString(" yyyy-MM-dd_HH-mm-ss"),
                Path.GetExtension(fileName)
                );
        }
    }
}
