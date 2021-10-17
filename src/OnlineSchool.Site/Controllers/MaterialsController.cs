using ItalWebConsulting.Infrastructure.Mvc.Caching;
using ItalWebConsulting.Infrastructure.Utility;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OnlineSchool.Contract;
using OnlineSchool.Contract.Material;
using OnlineSchool.Contract.Subject;
using OnlineSchool.Core.Documents_.Service;
using OnlineSchool.Core.Materials_.Services;
using OnlineSchool.Core.Teachers_.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace OnlineSchool.Site.Controllers
{
    public class MaterialsController : Controller
    {
        private const string SelectedDocumentEmailSessionKey = "SelectedDocumentEmailSessionKey";
        public Lazy<IMvcInHttpSessionCache> CacheSessionManager { get; set; }
        public Lazy<IWebHostEnvironment> WebHostEnvironment { get; set; }
        public string RootPath
        {
            get => WebHostEnvironment.Value.WebRootPath;
        }
        public ISubjectService SubjectService { get; set; }
        public ITeacherService TeacherService { get; set; }
        public IDocumentService DocumentiService { get; set; }
        public IMaterialService MaterialService { get; set; }

        public ActionResult DocumentViewer(int Id)
        {
            var FilePath = Path.Combine(WebHostEnvironment.Value.WebRootPath, ConfigurationConsts.UploadFolder);
            DirectoryInfo dir = new DirectoryInfo(FilePath);
            List<FileInfo> files = dir.GetFiles().ToList();
            return View(files);
        }

        [HttpPost]
        public ActionResult PrepareListEmailFiles(IEnumerable<string> selectedFiles)
        {
            CacheSessionManager.Value.Add(SelectedDocumentEmailSessionKey, selectedFiles);
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> PrepareListDowloadFiles(IEnumerable<string> selectedFiles)
        {
            CacheSessionManager.Value.Add(SelectedDocumentEmailSessionKey, selectedFiles);
            //var selectedfiles = CacheSessionManager.Value.Get<IEnumerable<string>>(SelectedDocumentEmailSessionKey);
            //var inputCreateZip = new CreateZipFileInput
            //{
            //    FilesName = selectedfiles,
            //    ContentRootPath = WebHostEnvironment.Value.WebRootPath
            //};
            var folderTmp = Path.Combine(WebHostEnvironment.Value.ContentRootPath, ConfigurationConsts.TempFolder);
            if (!System.IO.Directory.Exists(folderTmp))
                System.IO.Directory.CreateDirectory(folderTmp);
            var fileZip = Guid.NewGuid().ToString() + ".zip";
            var tmpFile = Path.Combine(folderTmp, fileZip);
            if (System.IO.File.Exists(tmpFile))
                System.IO.File.Delete(tmpFile);
            //Directory.EnumerateFiles(folderTmp).ToList().ForEach(f => System.IO.File.Delete(f));
            using (var zip = ZipFile.Open(tmpFile, ZipArchiveMode.Create)) ;
            //foreach (string fileName in selectedFiles)
            //{
            //    //var fullPath = Path.Combine(WebHostEnvironment.Value.WebRootPath, ConfigurationConsts.UploadFolder, fileName);
            //    //var file = zip.CreateEntryFromFile(fullPath, fileName);
            //}
            byte[] finalResult = System.IO.File.ReadAllBytes(tmpFile);
            return File(finalResult, "application/zip");
        }

        public async Task<IActionResult> Async_Save(IEnumerable<IFormFile> files, string metaData, int Id)
        {
            if (metaData == null)
            {
                foreach (var file in files)
                {
                    if (file == null || file.Length == 0)
                        return Content("file not selected");
                    var fileContent = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    //string renameFile = Convert.ToString(documentType.Replace(' ', '_') + "_" + Id + "." + fileContent.Split('.').Last());
                    var path = Path.Combine(WebHostEnvironment.Value.WebRootPath, ConfigurationConsts.UploadFolder);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    return Ok("");
                }
            }
            JsonSerializerSettings jsonSerializer = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            //documentType = await DocumentoService.GetDocumentTypeDescAsync(Id);
            var fileAppend = ImageHelper.ChunkFileSave(files, RootPath, Id, metaData);
            return Json(fileAppend, jsonSerializer);

        }

        [HttpPost]
        public ActionResult DeleteFile(string[] fileNames, int? Id)
        {
            if (fileNames != null && Id.HasValue)
            {
                var output = FileHelper.Delete_File(fileNames, RootPath, Id);
                if (output.HasErrors)
                {
                    foreach(var err in output.Errors)
                    {
                        ModelState.AddModelError(string.Empty, err);
                    }
                }
            }
            return Content("");
        }

        public async Task<IActionResult> CreateComment(MaterialModel model, string name, string body, int id, string teacherName)
        {
            await TeacherService.CreateCommentAsync(model, name, body, id, teacherName);
            return View("Material");
        }

        public async Task<IActionResult> GetTeacherComment()
        {
            var data = await TeacherService.GetTeacherComment();
            //return PartialView("_CommentPartial", data);
            return PartialView("Comment", data);
        }

    }
}