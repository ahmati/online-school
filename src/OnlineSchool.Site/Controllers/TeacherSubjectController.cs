using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnlineSchool.Contract.Infrastructure;
using OnlineSchool.Contract.Infrastructure.FileUpload;
using OnlineSchool.Contract.Lessons;
using OnlineSchool.Contract.Material;
using OnlineSchool.Contract.TeacherSubject;
using OnlineSchool.Core.Lessons_.Service;
using OnlineSchool.Core.Materials_.Services;
using OnlineSchool.Core.Teachers_.Service;
using OnlineSchool.Core.TeacherSubject_.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileResult = OnlineSchool.Contract.Infrastructure.FileUpload.FileResult;
using IW = ItalWebConsulting.Infrastructure.Mvc;

namespace OnlineSchool.Site.Controllers
{
    public class TeacherSubjectController: IW.ControllerBase<TeacherSubjectController>
    {
        public ITeacherSubjectService TeacherSubjectService { get; set; }
        public IMaterialService MaterialService { get; set; }
        public ILessonService  LessonService { get; set; }

        public async Task<IActionResult> Index (int id)
        {
            var model = await TeacherSubjectService.GetByIdAsync(id);

            return View(model);
        }

        [HttpGet]
        public async Task<JsonResult> GetAll()
        {
            var data = await TeacherSubjectService.GetAllAsync();
            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetBySubjectId ([DataSourceRequest] DataSourceRequest request, int subjectId)
        {
            var data = await TeacherSubjectService.GetBySubjectIdAsync(subjectId);
            var result = data.ToDataSourceResult(request);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TeacherSubjectModel model)
        {
            if (!ModelState.IsValid)
                return Json(model);

            var result = await TeacherSubjectService.CreateAsync(model);
            if (result.HasErrors)
            {
                foreach (var err in result.Errors)
                    ModelState.AddModelError(string.Empty, err);
                return Json(result);
            }

            return Json(result);

        }

        [HttpPost]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] TeacherSubjectModel model)
        {
            var result = await TeacherSubjectService.DeleteAsync(model.SubjectId, model.TeacherId);
            return Json(result);
        }

        #region ----------------------------------------------- TeacherSubject Materials -----------------------------------------------

        [HttpGet]
        public async Task<IActionResult> GetMaterials([DataSourceRequest] DataSourceRequest request, int id)
        {
            var data = await TeacherSubjectService.GetMaterialsAsync(id);
            var result = data.ToDataSourceResult(request);
            return Json(result);
        }

        [Route("[controller]/[action]/{teacherSubjectId}")]
        public async Task<IActionResult> SubjectMaterial_ChunkUpload(IEnumerable<IFormFile> files, string metaData, int teacherSubjectId)
        {
            /* 
             * NOTE: The name of the Upload component (in View) in this case is 'files'. So, the name of the parameter must match the name of the component
             * Workflow:
             * 1. First, check if file is empty: (TotalChunks and TotalFileSize will be 0 )
             * 2. If chunkIndex = 0: 
             *      a. Check if TeacherSubject with 'teacherSubjectId' exists. If exists, continue, else show error.
             *      b. Check in database if there already is a TeacherSubjectMaterial with that fileName
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
                var teacherSubject = await TeacherSubjectService.GetByIdAsync(teacherSubjectId);
                if (teacherSubject is null)
                    return BadRequest(JsonConvert.SerializeObject(new { error = "Could not find information about teacher's subject." }));

                // b.
                var existingMaterial = await TeacherSubjectService.GetTeacherSubjectMaterialByFileNameAsync(teacherSubjectId, chunkData.FileName);
                if (existingMaterial.HasErrors)
                    return BadRequest(JsonConvert.SerializeObject(new { error = existingMaterial.Errors[0] }));
                if (existingMaterial.Output != null)
                    return BadRequest(JsonConvert.SerializeObject(new { error = $"Material with name '{chunkData.FileName}' already exists" }));
            }

            if (files != null)
            {
                var finalDir = $"{WebHostEnvironment.Value.ContentRootPath}\\private_uploads\\materials\\teachersubjects\\{teacherSubjectId}";
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
                    var materialModel = new MaterialModel();
                    materialModel.FileName = chunkData.FileName;
                    materialModel.MimeType = chunkData.ContentType;
                    materialModel.FileSize = (int)chunkData.TotalFileSize;

                    var opResult = await MaterialService.CreateAsync(materialModel);
                    
                    if (opResult.HasErrors)
                        return BadRequest(JsonConvert.SerializeObject(new { error = opResult.Errors[0] }));


                    var teacherSubjectModel = new TeacherSubjectMaterialModel();
                    teacherSubjectModel.TeacherSubjectId = teacherSubjectId;
                    teacherSubjectModel.MaterialId = opResult.Output.Id;
                    
                    var teacherSubjectResult = await TeacherSubjectService.CreateTeacherSubjectMaterialAsync(teacherSubjectModel);

                    if (teacherSubjectResult.HasErrors)
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
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> DeleteMaterial(int id)
        {
            var res1 = await TeacherSubjectService.GetTeacherSubjectMaterialByIdAsync(id);
            int materialId =  res1.Output.Material.Id;

            var res2 = await TeacherSubjectService.DeleteMaterialAsync(id);
            var res3 = await MaterialService.DeleteAsync(materialId);
            if (!res2.HasErrors && !res3.HasErrors)
            {
                try
                {
                    string docPath = $"{WebHostEnvironment.Value.ContentRootPath}\\private_uploads\\materials\\teachersubjects\\{res1.Output.TeacherSubjectId}\\{res1.Output.Material.FileName}";
                    System.IO.File.Delete(docPath);
                }
                catch { }
            }

            return Json(res3);
        }

        [HttpGet("[controller]/[action]/{id}")]
        public async Task<IActionResult> DownloadTeacherSubjectMaterial(int id)
        {
            var data = await TeacherSubjectService.GetTeacherSubjectMaterialByIdAsync(id);
            var material = data.Output;

            if (data.HasErrors || material is null)
                return new EmptyResult();

            var docPath = $"{WebHostEnvironment.Value.ContentRootPath}\\private_uploads\\materials\\teachersubjects\\{material.TeacherSubjectId}\\{material.Material.FileName}";
            return PhysicalFile(docPath, material.Material.MimeType, material.Material.FileName);
        }


        [HttpGet]
        public async Task<IActionResult> DownloadTeacherSubjectMaterials([FromQuery] int[] ids)
        {
            var data = await TeacherSubjectService.GetTeacherSubjectMaterialsByIdsAsync(ids);
            var material = data.Output;
            if (data.HasErrors || material is null)
                return new EmptyResult();
            var zipName = $"archive-{DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss")}.zip";
            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    data.Output.Each(material =>
                    {
                        var currEntryContentPath = $"{WebHostEnvironment.Value.ContentRootPath}\\private_uploads\\materials\\teachersubjects\\{material.TeacherSubjectId}\\{material.Material.FileName}";

                        var currZipEntry = archive.CreateEntry($"{material.TeacherSubject.Subject.Name}\\{material.Material.FileName}");
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

        #region ----------------------------------------------- TeacherSubject Lessons -----------------------------------------------

        [Route("[controller]/[action]/{teacherSubjectId}/{lessonId}")]
        public async Task<IActionResult> LessonMaterial_ChunkUpload(IEnumerable<IFormFile> files, string metaData, int teacherSubjectId, int lessonId)
        {
            /* 
             * NOTE: The name of the Upload component (in View) in this case is 'files'. So, the name of the parameter must match the name of the component
             * Workflow:
             * 1. First, check if file is empty: (TotalChunks and TotalFileSize will be 0 )
             * 2. If chunkIndex = 0: 
             *      a. Check if Lesson with 'lessonId' exists. If exists, continue, else show error.
             *      b. Check in database if there already is a LessonMaterial with the same file name being uploaded
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
                var lesson = await LessonService.GetByIdAsync(lessonId);
                if (lesson is null)
                    return BadRequest(JsonConvert.SerializeObject(new { error = "Could not find information about requested lesson." }));

                // b.
                var existingMaterial = await TeacherSubjectService.GetLessonMaterialByFileNameAsync(lesson.Id, chunkData.FileName);
                if (existingMaterial.HasErrors)
                    return BadRequest(JsonConvert.SerializeObject(new { error = existingMaterial.Errors[0] }));
                if (existingMaterial.Output != null)
                    return BadRequest(JsonConvert.SerializeObject(new { error = $"Material with name '{chunkData.FileName}' already exists" }));
            }
            
            if (files != null)
            {
                var finalDir = $"{WebHostEnvironment.Value.ContentRootPath}\\private_uploads\\materials\\teachersubjects\\{teacherSubjectId}\\lessons\\{lessonId}";

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
                    // a.
                    var materialModel = new MaterialModel();
                    materialModel.FileName = chunkData.FileName;
                    materialModel.MimeType = chunkData.ContentType;
                    materialModel.FileSize = (int)chunkData.TotalFileSize;

                    var opResult = await MaterialService.CreateAsync(materialModel);

                    if (opResult.HasErrors)
                        return BadRequest(JsonConvert.SerializeObject(new { error = opResult.Errors[0] }));


                    var lessonModel = new LessonMaterialModel();
                    lessonModel.LessonId = lessonId;
                    lessonModel.MaterialId = opResult.Output.Id;

                    var lessonResult = await TeacherSubjectService.CreateLessonMaterialAsync(lessonModel);

                    if (lessonResult.HasErrors)
                        return BadRequest(JsonConvert.SerializeObject(new { error = opResult.Errors[0] }));

                    // b.
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

        [HttpGet]
        public async Task<IActionResult> GetLessonsByTeacherSubject([DataSourceRequest] DataSourceRequest request, int id)
        {
            var data = await TeacherSubjectService.GetLessonsByTeacherSubjectAsync(id);
            var result = data.ToDataSourceResult(request);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetLessonMaterials([DataSourceRequest] DataSourceRequest request, int id)
        {
            var data = await TeacherSubjectService.GetLessonMaterialsAsync(id);
            var result = data.ToDataSourceResult(request);
            return Json(result);
        }

        [HttpPost]
        [HttpDelete]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> DeleteLessonMaterial(int id)
        {
            var res1 = await TeacherSubjectService.GetLessonMaterialByIdAsync(id);
            int materialId = res1.Output.Material.Id;

            var res2 = await TeacherSubjectService.DeleteLessonMaterialAsync(id);
            var res3 = await MaterialService.DeleteAsync(materialId);
            if (!res2.HasErrors && !res3.HasErrors)
            {
                try
                {
                    string docPath = $"{WebHostEnvironment.Value.ContentRootPath}\\private_uploads\\materials\\teachersubjects\\{res1.Output.Lesson.TeacherSubjectId}\\lessons\\{res1.Output.Lesson.Id}\\{res1.Output.Material.FileName}";
                    System.IO.File.Delete(docPath);
                }
                catch { }
            }

            return Json(res3);
        }

        [HttpGet("[controller]/[action]/{id}")]
        public async Task<IActionResult> DownloadLessonMaterial(int id)
        {
            var data = await TeacherSubjectService.GetLessonMaterialByIdAsync(id);
            var material = data.Output;

            if (data.HasErrors || material is null)
                return new EmptyResult();

            var docPath = $"{WebHostEnvironment.Value.ContentRootPath}\\private_uploads\\materials\\teachersubjects\\{data.Output.Lesson.TeacherSubjectId}\\lessons\\{data.Output.Lesson.Id}\\{data.Output.Material.FileName}";

            return PhysicalFile(docPath, material.Material.MimeType, material.Material.FileName);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadLessonMaterials([FromQuery] int[] ids)
        {
            var data = await TeacherSubjectService.GetLessonMaterialsByIdsAsync(ids);
            var material = data.Output;
            if (data.HasErrors || material is null)
                return new EmptyResult();
            var zipName = $"archive-{DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss")}.zip";
            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    data.Output.Each(material =>
                    {
                        var currEntryContentPath = $"{WebHostEnvironment.Value.ContentRootPath}\\private_uploads\\materials\\teachersubjects\\{material.Lesson.TeacherSubjectId}\\lessons\\{material.Lesson.Id}\\{material.Material.FileName}";
                        var currZipEntry = archive.CreateEntry($"{material.Lesson.TeacherSubject.Subject.Name}\\{material.Material.FileName}");
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
