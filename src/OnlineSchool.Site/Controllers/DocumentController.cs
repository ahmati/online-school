using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnlineSchool.Contract.Documents;
using OnlineSchool.Contract.Infrastructure.FileUpload;
using OnlineSchool.Core.Documents_.Service;
using OnlineSchool.Core.Infrastructure.FileUpload;
using OnlineSchool.Core.Students_.Service;
using OnlineSchool.Core.Teachers_.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FileResult = OnlineSchool.Contract.Infrastructure.FileUpload.FileResult;
using IW = ItalWebConsulting.Infrastructure.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OnlineSchool.Site.Controllers
{
    public class DocumentController : IW.ControllerBase<DocumentController>
    {
        public IDocumentService DocumentService { get; set; }
        public IFileService FileService { get; set; }
        public IStudentService StudentService { get; set; }
        public ITeacherService TeacherService { get; set; } 

        #region StudentDocument

        [HttpGet]
        public IActionResult StudentDocuments()
        {
            return View();
        }

        public async Task<IActionResult> GetStudentDocuments([DataSourceRequest] DataSourceRequest request)
        {
            var result = await DocumentService.GetStudentDocumentsAsync();
            var data = result.Output.ToDataSourceResult(request);
            return Json(data);
        }

        [Route("[controller]/[action]/{studentId}")]
        public async Task<IActionResult> StudentDocument_ChunkUpload(IEnumerable<IFormFile> files, string metaData, int studentId)
        {
            /* 
             * NOTE: The name of the Upload component (in View) in this case is 'files'. So, the name of the parameter must match the name of the component
             * Workflow:
             * 1. First, check if file is empty: (TotalChunks and TotalFileSize will be 0 )
             * 2. If chunkIndex = 0: 
             *      a. Check if student with 'studentId' exists. If exists, continue, else show error.
             *      b. Check in database if student already has a file with this name. If not, continue, else show error.
             * 3. Upload the chunk(s) in a 'Temp' folder first.
             *      a. If chunkIndex = 0, and there already is a file with that name in Temp, remove it
             * 4. If the current chunk is the last (chunkIndex = totalChunks - 1)
             *      a. Save record in database
             *      b. If saving in database succeeds, move the final file out of Temp
            */

            if (metaData == null)
            {
                return BadRequest(JsonConvert.SerializeObject(new { errorMessage = "MetaData is missing." }));
            }

            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(metaData));

            JsonSerializer serializer = new JsonSerializer();
            ChunkMetaData chunkData;
            using (StreamReader streamReader = new StreamReader(ms))
            {
                chunkData = (ChunkMetaData)serializer.Deserialize(streamReader, typeof(ChunkMetaData));
            }

            // 1.
            if (chunkData.TotalFileSize == 0)
                return BadRequest(JsonConvert.SerializeObject(new { error = $"File '{chunkData.FileName}' is empty. Files cannot be empty." }));

            // 2.
            if (chunkData.ChunkIndex == 0)
            {
                // a.
                var student = await StudentService.GetByIdAsync(studentId);
                if(student is null)
                    return BadRequest(JsonConvert.SerializeObject(new { error = "Could not find information about student." }));

                // b.
                var existingDocument = await DocumentService.GetStudentDocumentByFileNameAsync(studentId, chunkData.FileName);
                if(existingDocument.HasErrors)
                    return BadRequest(JsonConvert.SerializeObject(new { error = existingDocument.Errors[0] }));
                if (existingDocument.Output != null)
                    return BadRequest(JsonConvert.SerializeObject(new { error = $"Student already has a document with name '{chunkData.FileName}'" }));
            }

            if (files != null)
            {
                var finalDir = $"{WebHostEnvironment.Value.ContentRootPath}\\private_uploads\\documents\\students\\{studentId}";
                var tempDir = $"{finalDir}\\temp";

                Directory.CreateDirectory(finalDir);
                Directory.CreateDirectory(tempDir);

                var tempPath = $"{tempDir}\\{chunkData.FileName}";
                var finalPath = $"{finalDir}\\{chunkData.FileName}";

                // 3.
                if (chunkData.ChunkIndex == 0 && System.IO.File.Exists(tempPath))
                    System.IO.File.Delete(tempPath);

                using (FileStream stream = new FileStream(tempPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                {
                    foreach (var chunk in files)
                    {
                        await chunk.CopyToAsync(stream);
                    }
                }

                // 4. 
                if(chunkData.ChunkIndex == (chunkData.TotalChunks - 1))
                {
                    var documentModel = new CreateStudentDocumentModel();
                    documentModel.StudentId = studentId;
                    documentModel.FileName = chunkData.FileName;
                    documentModel.MimeType = chunkData.ContentType;
                    documentModel.FileSize = (int)chunkData.TotalFileSize;

                    var opResult = await DocumentService.CreateStudentDocumentAsync(documentModel);
                    if (opResult.HasErrors)
                        return BadRequest(JsonConvert.SerializeObject(new { error = opResult.Errors[0] }));

                    if(System.IO.File.Exists(finalPath))
                        System.IO.File.Delete(finalPath);
                    System.IO.File.Move(tempPath, finalPath, true);
                }
            }

            FileResult fileBlob = new FileResult();
            fileBlob.uploaded = chunkData.TotalChunks - 1 <= chunkData.ChunkIndex;
            fileBlob.fileUid = chunkData.UploadUid;

            return Json(fileBlob);
        }

        [HttpPost][HttpDelete]
        [Route("[controller]/[action]/{studentDocumentId}")]
        public async Task<IActionResult> DeleteStudentDocument(int studentDocumentId)
        {
            var result = await DocumentService.DeleteStudentDocumentAsync(studentDocumentId);
            var studentDocument = result.Output;

            if(!result.HasErrors)
            {
                try {
                    string docPath = $"{WebHostEnvironment.Value.ContentRootPath}\\private_uploads\\documents\\students\\{studentDocument.StudentId}\\{studentDocument.FileName}";
                    System.IO.File.Delete(docPath);
                }
                catch {}
            }

            return Json(result);
        }

        [HttpGet("[controller]/[action]/{studentDocumentId}")]
        public async Task<IActionResult> DownloadStudentDocument(int studentDocumentId)
        {
            var data = await DocumentService.GetStudentDocumentByIdAsync(studentDocumentId);
            var document = data.Output;

            if (data.HasErrors || document is null)
                return new EmptyResult();

            var docPath = $"{WebHostEnvironment.Value.ContentRootPath}\\private_uploads\\documents\\students\\{document.StudentId}\\{document.FileName}";
            return PhysicalFile(docPath, document.MimeType, document.FileName);
        }

        [HttpGet("[controller]/[action]")]
        public async Task<IActionResult> DownloadStudentDocuments([FromQuery] int[] ids)
        {
            var data = await DocumentService.GetStudentDocumentsByIdsAsync(ids);
            var document = data.Output;

            if (data.HasErrors || document is null)
                return new EmptyResult();

            var zipName = $"archive-{DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss")}.zip";

            using(var memoryStream = new MemoryStream())
            {
                using(var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    data.Output.Each(document =>
                    {
                        var currEntryContentPath = $"{WebHostEnvironment.Value.ContentRootPath}\\private_uploads\\documents\\students\\{document.StudentId}\\{document.FileName}";

                        var currZipEntry = archive.CreateEntry($"{document.Student.Email}\\{document.FileName}");
                        using (var writer = new BinaryWriter(currZipEntry.Open()))
                        {
                            var docContent = System.IO.File.ReadAllBytes(currEntryContentPath);
                            writer.Write(docContent);
                        }
                    });
                }

                return File(memoryStream.ToArray(), "application/zip", zipName);
            }            
        }

        #endregion

        #region TeacherDocument

        [HttpGet]
        public IActionResult TeacherDocuments()
        {
            return View();
        }

        public async Task<IActionResult> GetTeacherDocuments([DataSourceRequest] DataSourceRequest request)
        {
            var result = await DocumentService.GetTeacherDocumentsAsync();
            var data = result.Output.ToDataSourceResult(request);
            return Json(data);
        }

        [Route("[controller]/[action]/{teacherId}")]
        public async Task<IActionResult> TeacherDocument_ChunkUpload(IEnumerable<IFormFile> files, string metaData, int teacherId)
        {
            /* 
             * NOTE: The name of the Upload component (in View) in this case is 'files'. So, the name of the parameter must match the name of the component
             * Workflow:
             * 1. First, check if file is empty: (TotalChunks and TotalFileSize will be 0 )
             * 2. If chunkIndex = 0: 
             *      a. Check if teacher with 'teacherId' exists. If exists, continue, else show error.
             *      b. Check in database if teacher already has a file with this name. If not, continue, else show error.
             * 3. Upload the chunk(s) in a 'Temp' folder first.
             *      a. If chunkIndex = 0, and there already is a file with that name in Temp, remove it
             * 4. If the current chunk is the last (chunkIndex = totalChunks - 1)
             *      a. Save record in database
             *      b. If saving in database succeeds, move the final file out of Temp
            */

            if (metaData == null)
            {
                return BadRequest(JsonConvert.SerializeObject(new { errorMessage = "MetaData is missing." }));
            }

            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(metaData));

            JsonSerializer serializer = new JsonSerializer();
            ChunkMetaData chunkData;
            using (StreamReader streamReader = new StreamReader(ms))
            {
                chunkData = (ChunkMetaData)serializer.Deserialize(streamReader, typeof(ChunkMetaData));
            }

            // 1.
            if (chunkData.TotalFileSize == 0)
                return BadRequest(JsonConvert.SerializeObject(new { error = $"File '{chunkData.FileName}' is empty. Files cannot be empty." }));

            // 2.
            if (chunkData.ChunkIndex == 0)
            {
                // a.
                var teacher = await TeacherService.GetByIdAsync(teacherId);
                if (teacher is null)
                    return BadRequest(JsonConvert.SerializeObject(new { error = "Could not find information about teacher." }));

                // b.
                var existingDocument = await DocumentService.GetTeacherDocumentByFileNameAsync(teacherId, chunkData.FileName);
                if (existingDocument.HasErrors)
                    return BadRequest(JsonConvert.SerializeObject(new { error = existingDocument.Errors[0] }));
                if (existingDocument.Output != null)
                    return BadRequest(JsonConvert.SerializeObject(new { error = $"Teacher already has a document with name '{chunkData.FileName}'" }));
            }

            if (files != null)
            {
                var finalDir = $"{WebHostEnvironment.Value.ContentRootPath}\\private_uploads\\documents\\teachers\\{teacherId}";
                var tempDir = $"{finalDir}\\temp";

                Directory.CreateDirectory(finalDir);
                Directory.CreateDirectory(tempDir);

                var tempPath = $"{tempDir}\\{chunkData.FileName}";
                var finalPath = $"{finalDir}\\{chunkData.FileName}";

                // 3.
                if (chunkData.ChunkIndex == 0 && System.IO.File.Exists(tempPath))
                    System.IO.File.Delete(tempPath);

                using (FileStream stream = new FileStream(tempPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                {
                    foreach (var chunk in files)
                    {
                        await chunk.CopyToAsync(stream);
                    }
                }

                // 4. 
                if (chunkData.ChunkIndex == (chunkData.TotalChunks - 1))
                {
                    var documentModel = new CreateTeacherDocumentModel();
                    documentModel.TeacherId = teacherId;
                    documentModel.FileName = chunkData.FileName;
                    documentModel.MimeType = chunkData.ContentType;
                    documentModel.FileSize = (int)chunkData.TotalFileSize;

                    var opResult = await DocumentService.CreateTeacherDocumentAsync(documentModel);
                    if (opResult.HasErrors)
                        return BadRequest(JsonConvert.SerializeObject(new { error = opResult.Errors[0] }));

                    if (System.IO.File.Exists(finalPath))
                        System.IO.File.Delete(finalPath);
                    System.IO.File.Move(tempPath, finalPath, true);
                }
            }

            FileResult fileBlob = new FileResult();
            fileBlob.uploaded = chunkData.TotalChunks - 1 <= chunkData.ChunkIndex;
            fileBlob.fileUid = chunkData.UploadUid;

            return Json(fileBlob);
        }

        [HttpPost]
        [HttpDelete]
        [Route("[controller]/[action]/{teacherDocumentId}")]
        public async Task<IActionResult> DeleteTeacherDocument(int teacherDocumentId)
        {
            var result = await DocumentService.DeleteTeacherDocumentAsync(teacherDocumentId);
            var teacherDocument = result.Output;

            if (!result.HasErrors)
            {
                try
                {
                    string docPath = $"{WebHostEnvironment.Value.ContentRootPath}\\private_uploads\\documents\\teachers\\{teacherDocument.TeacherId}\\{teacherDocument.FileName}";
                    System.IO.File.Delete(docPath);
                }
                catch { }
            }

            return Json(result);
        }

        [HttpGet("[controller]/[action]/{teacherDocumentId}")]
        public async Task<IActionResult> DownloadTeacherDocument(int teacherDocumentId)
        {
            var data = await DocumentService.GetTeacherDocumentByIdAsync(teacherDocumentId);
            var document = data.Output;

            if (data.HasErrors || document is null)
                return new EmptyResult();

            var docPath = $"{WebHostEnvironment.Value.ContentRootPath}\\private_uploads\\documents\\teachers\\{document.TeacherId}\\{document.FileName}";
            return PhysicalFile(docPath, document.MimeType, document.FileName);
        }

        [HttpGet("[controller]/[action]")]
        public async Task<IActionResult> DownloadTeacherDocuments([FromQuery] int[] ids)
        {
            var data = await DocumentService.GetTeacherDocumentsByIdsAsync(ids);
            var document = data.Output;

            if (data.HasErrors || document is null)
                return new EmptyResult();

            var zipName = $"archive-{DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss")}.zip";

            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    data.Output.Each(document =>
                    {
                        var currEntryContentPath = $"{WebHostEnvironment.Value.ContentRootPath}\\private_uploads\\documents\\teachers\\{document.TeacherId}\\{document.FileName}";

                        var currZipEntry = archive.CreateEntry($"{document.Teacher.Email}\\{document.FileName}");
                        using (var writer = new BinaryWriter(currZipEntry.Open()))
                        {
                            var docContent = System.IO.File.ReadAllBytes(currEntryContentPath);
                            writer.Write(docContent);
                        }
                    });
                }

                return File(memoryStream.ToArray(), "application/zip", zipName);
            }
        }

        #endregion
    }
}
