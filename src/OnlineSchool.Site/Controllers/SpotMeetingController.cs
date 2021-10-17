using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnlineSchool.Contract.Infrastructure.FileUpload;
using OnlineSchool.Contract.Material;
using OnlineSchool.Contract.SpotMeeting;
using OnlineSchool.Core.Identity_.Services;
using OnlineSchool.Core.Materials_.Services;
using OnlineSchool.Core.SpotMeeting_.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;
using IW = ItalWebConsulting.Infrastructure.Mvc;
using FileResult = OnlineSchool.Contract.Infrastructure.FileUpload.FileResult;
using System.IO.Compression;
using OnlineSchool.Contract.Infrastructure.Enums;
using Microsoft.AspNetCore.Authorization;
using OnlineSchool.Core.Students_.Service;
using OnlineSchool.Contract.Contacts;
using OnlineSchool.Contract.Infrastructure;
using OnlineSchool.Core.Webex_.Services;
using OnlineSchool.Core.Teachers_.Service;
using OnlineSchool.Core.IcsCalendar.Service;
using OnlineSchool.Contract.Calendar;
using OnlineSchool.Contract.Resources;

namespace OnlineSchool.Site.Controllers
{
    public class SpotMeetingController : IW.ControllerBase<SpotMeetingController>
    {
        public IIdentityService IdentityService { get; set; }
        public ISpotMeetingService SpotMeetingService { get; set; }
        public IMaterialService MaterialService { get; set; }
        public IStudentService StudentService { get; set; }
        public ITeacherService TeacherService { get; set; }
        public IWebexService WebexService { get; set; }

        public async Task<IActionResult> Index(int? page)
        {
            var spotMeetings = await SpotMeetingService.GetAllPublishedAsync();

            int pageSize = 6;
            int pageNumber = (page ?? 1);

            return View(spotMeetings.ToPagedList(pageNumber, pageSize));
        }

        [Authorize(Roles = "Admin, Teacher")]
        public IActionResult Manage()
        {
            return View();
        }

        [HttpGet("[controller]/{id}/[action]")]
        public async Task<IActionResult> Details(int id)
        {
            var spotMeeting = await SpotMeetingService.GetByIdAsync(id);
            if (spotMeeting is null)
                return RedirectToAction("Error", "Home");

            ViewBag.IsBooked = (await IdentityService.GetExistingBookedSpotMeetingAsync(GetUser().Identity.Name, id)) != null;
            ViewBag.PurchasableItemType = PurchasableItemType.SpotMeeting;
            ViewBag.IsSpotMeetingTeacher = (await SpotMeetingService.GetSpotMeetingTeacherAsync(GetUser().Identity.Name, id)) != null;

            var bookedSpotMeetings = await SpotMeetingService.GetBookedSpotMeetingsBySpotMeetingIdAsync(spotMeeting.Id);
            ViewBag.BookedSpotMeetingsCount = bookedSpotMeetings.Count();

            ViewBag.CurrentAvailableSpots = (spotMeeting.AvailableSpots - (spotMeeting.SpotMeetingTeachers.Count - 1) - ViewBag.BookedSpotMeetingsCount) < 0 ? 0 : (spotMeeting.AvailableSpots - (spotMeeting.SpotMeetingTeachers.Count - 1) - ViewBag.BookedSpotMeetingsCount);

            return View(spotMeeting);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> Create(CreateSpotMeetingModel model)
        {
            model.ImagePath = model.ImageFile?.FileName ?? null;

            var result = await SpotMeetingService.CreateAsync(model);
            if (result.HasErrors)
            {
                TempData["Error"] = result.Errors;
                return View(model);
            }

            TempData["Success"] = SharedResources.MeetingCreateSuccessful;
            return RedirectToAction("Manage", "SpotMeeting");
        }

        [Authorize(Roles = "Admin, Teacher")]
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var spotMeeting = await SpotMeetingService.GetByIdAsync(id);

            var model = Mapper.Value.Map<UpdateSpotMeetingModel>(spotMeeting);
            model.StartTime = spotMeeting.StartDate.TimeOfDay;

            return View(model);
        }

        [Authorize(Roles = "Admin, Teacher")]
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateSpotMeetingModel model)
        {
            if (model.ImageFile != null)
                model.ImagePath = model.ImageFile.FileName;

            var result = await SpotMeetingService.UpdateAsync(model);
            if (result.HasErrors)
            {
                TempData["Error"] = result.Errors;
                return View(model);
            }

            TempData["Success"] = SharedResources.MeetingUpdateSuccessful;
            return RedirectToAction("Manage");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await SpotMeetingService.DeleteAsync(id);
            return Json(result);
        }

        [Authorize(Roles = "Admin, Teacher")]
        [HttpPost]
        public async Task<IActionResult> Publish(int id)
        {
            var result = await SpotMeetingService.PublishAsync(id);
            return Json(result);
        }

        [Authorize(Roles = "Admin, Teacher")]
        [HttpPost]
        public async Task<IActionResult> Unpublish(int id)
        {
            var result = await SpotMeetingService.UnpublishAsync(id);
            return Json(result);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> JoinSpotMeeting(int id)
        {
            var email = GetUser().Identity.Name;

            var isPurchased = await SpotMeetingService.GetExistingPurchasedItemAsync(email, id);
            if (isPurchased.Output)
            {
                return RedirectToAction("Details", new { id = id });
            }

            var result = await SpotMeetingService.JoinAsync(id, email);
            if (result.HasErrors)
            {
                TempData["Error"] = result.Errors;
            }

            TempData["Success"] = SharedResources.JoinMeetingSuccess;

            return RedirectToAction("Details", new { id = id });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSpotMeetings([DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<SpotMeetingModel> list;
            if (User.IsInRole("Admin"))
            {
                list = await SpotMeetingService.GetAllAsync();
            }
            else
            {
                list = await SpotMeetingService.GetTeacherSpotMeetingsAsync(GetUser().Identity.Name);
            }

            var result = list.ToDataSourceResult(request);

            return Json(result);
        }

        public async Task<IActionResult> CheckProgram(int id)
        {
            var spotMeeting = await SpotMeetingService.GetByIdAsync(id);

            var bookedSpotMeetings = await SpotMeetingService.GetBookedSpotMeetingsBySpotMeetingIdAsync(spotMeeting.Id);
            ViewBag.BookedSpotMeetingsCount = bookedSpotMeetings.Count();

            ViewBag.CurrentAvailableSpots = (spotMeeting.AvailableSpots - (spotMeeting.SpotMeetingTeachers.Count - 1) - ViewBag.BookedSpotMeetingsCount) < 0 ? 0 : (spotMeeting.AvailableSpots - (spotMeeting.SpotMeetingTeachers.Count - 1) - ViewBag.BookedSpotMeetingsCount);

            ViewBag.IsSpotMeetingTeacher = (await SpotMeetingService.GetSpotMeetingTeacherAsync(GetUser().Identity.Name, id)) != null;

            return View(spotMeeting);
        }

        #region ----------------------------------------------- SpotMeeting Materials -----------------------------------------------
        [HttpGet]
        public async Task<IActionResult> GetMaterials([DataSourceRequest] DataSourceRequest request, int id)
        {
            var data = await SpotMeetingService.GetMaterialsAsync(id);
            var result = data.ToDataSourceResult(request);
            return Json(result);
        }

        [Route("[controller]/[action]/{spotMeetingId}")]
        public async Task<IActionResult> SpotMeetingMaterial_ChunkUpload(IEnumerable<IFormFile> files, string metaData, int spotMeetingId)
        {
            /* 
             * NOTE: The name of the Upload component (in View) in this case is 'files'. So, the name of the parameter must match the name of the component
             * Workflow:
             * 1. First, check if file is empty: (TotalChunks and TotalFileSize will be 0 )
             * 2. If chunkIndex = 0: 
             *      a. Check if SpotMeeting with 'spotMeetingId' exists. If exists, continue, else show error.
             *      b. Check in database if there already is a SpotMeetingMaterial with that fileName
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
                var spotMeeting = await SpotMeetingService.GetByIdAsync(spotMeetingId);
                if (spotMeeting is null)
                    return BadRequest(JsonConvert.SerializeObject(new { error = "Could not find information about the meeting." }));

                // b.
                var existingMaterial = await SpotMeetingService.GetSpotMeetingMaterialByFileNameAsync(spotMeetingId, chunkData.FileName);
                if (existingMaterial.HasErrors)
                    return BadRequest(JsonConvert.SerializeObject(new { error = existingMaterial.Errors[0] }));
                if (existingMaterial.Output != null)
                    return BadRequest(JsonConvert.SerializeObject(new { error = $"Material with name '{chunkData.FileName}' already exists" }));
            }

            if (files != null)
            {
                var finalDir = $"{WebHostEnvironment.Value.ContentRootPath}\\private_uploads\\materials\\spotmeetings\\{spotMeetingId}";
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


                    var spotMeetingMaterialModel = new SpotMeetingMaterialModel();
                    spotMeetingMaterialModel.SpotMeetingId = spotMeetingId;
                    spotMeetingMaterialModel.MaterialId = opResult.Output.Id;

                    var spotMeetingResult = await SpotMeetingService.CreateSpotMeetingMaterialAsync(spotMeetingMaterialModel);

                    if (spotMeetingResult.HasErrors)
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

        [HttpGet("[controller]/[action]/{id}")]
        public async Task<IActionResult> DownloadSpotMeetingMaterial(int id)
        {
            var data = await SpotMeetingService.GetSpotMeetingMaterialByIdAsync(id);
            var material = data.Output;

            if (data.HasErrors || material is null)
            {
                TempData["Error"] = data.Errors;
                return new EmptyResult();
            }

            var docPath = $"{WebHostEnvironment.Value.ContentRootPath}\\private_uploads\\materials\\spotmeetings\\{material.SpotMeeting.Id}\\{material.Material.FileName}";
            return PhysicalFile(docPath, material.Material.MimeType, material.Material.FileName);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSpotMeetingMaterials([FromQuery] int[] ids)
        {
            var data = await SpotMeetingService.GetSpotMeetingMaterialByIdsAsync(ids);
            var material = data.Output;

            if (data.HasErrors || material is null)
            {
                TempData["Error"] = data.Errors;
                return new EmptyResult();
            }

            var zipName = $"archive-{DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss")}.zip";
            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    data.Output.Each(material =>
                    {
                        var currEntryContentPath = $"{WebHostEnvironment.Value.ContentRootPath}\\private_uploads\\materials\\spotmeetings\\{material.SpotMeeting.Id}\\{material.Material.FileName}";

                        var currZipEntry = archive.CreateEntry($"{material.SpotMeeting.Title}\\{material.Material.FileName}");
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

        [HttpPost]
        [HttpDelete]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> DeleteSpotMeetingMaterial(int id)
        {
            var res1 = await SpotMeetingService.GetSpotMeetingMaterialByIdAsync(id);
            int materialId = res1.Output.Material.Id;

            var res2 = await SpotMeetingService.DeleteSpotMeetingMaterialAsync(id);
            var res3 = await MaterialService.DeleteAsync(materialId);
            if (!res2.HasErrors && !res3.HasErrors)
            {
                try
                {
                    string docPath = $"{WebHostEnvironment.Value.ContentRootPath}\\private_uploads\\materials\\spotmeetings\\{res1.Output.SpotMeetingId}\\{res1.Output.Material.FileName}";
                    System.IO.File.Delete(docPath);
                }
                catch { }
            }

            return Json(res3);
        }
        #endregion

        #region ----------------------------------------------- SpotMeeting Teachers -----------------------------------------------
        [HttpGet]
        public async Task<IActionResult> GetTeachers([DataSourceRequest] DataSourceRequest request, int id)
        {
            var data = await SpotMeetingService.GetTeachersAsync(id);
            var result = data.ToDataSourceResult(request);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> AssignTeacher([FromBody] SpotMeetingTeacherModel model)
        {
            if (!ModelState.IsValid)
                return Json(model);

            var result = await SpotMeetingService.CreateSpotMeetingTeacherAsync(model);
            if (result.HasErrors)
            {
                TempData["Error"] = result.Errors;
                return Json(result);
            }

            return Json(result);
        }

        [HttpPost]
        [HttpDelete]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> UnAssignTeacher(int id)
        {
            var result = await SpotMeetingService.DeleteSpotMeetingTeacherAsync(id);
            return Json(result);
        }
        #endregion

        #region ----------------------------------------------- User SpotMeetings -----------------------------------------------
        [Authorize]
        [HttpGet]
        public IActionResult UserSpotMeetings()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetUserSpotmeetings([DataSourceRequest] DataSourceRequest request)
        {
            var personalInfo = await IdentityService.FindByEmailAsync(GetUser().Identity.Name);
            var mySpotMeetings = await IdentityService.GetUserSpotMeetingsAsync(personalInfo.Id);
            var result = mySpotMeetings.ToDataSourceResult(request);
            return Json(result);
        }
        #endregion

        #region ----------------------------------------------- Live SpotMeeting -----------------------------------------------
        [Authorize(Roles = "Teacher")]
        [HttpGet("[controller]/{spotMeetingId}/Host")]
        public async Task<IActionResult> Host(int spotMeetingId)
        {
            // 1. Check if spot meeting with spotMeetingId exists
            // 2. Check the spot meeting Date/Start/End so that you don't start the spot meeting too early or after it has finished.
            // 3. Pass the ciscoAuthToken & meetingSipAddress

            // 1.
            var spotMeeting = await SpotMeetingService.GetByIdAsync(spotMeetingId);
            if (spotMeeting is null)
                return RedirectToAction("Manage", "SpotMeeting");

            // 2.
            if (this.IsSpotMeetingReadyToStart(spotMeeting) == false)
            {
                TempData["Error"] = SharedResources.MeetingNotReady;
                return RedirectToAction("Manage", "SpotMeeting");
            }

            // 3.
            var integration = await WebexService.GetIntegrationAsync();
            ViewBag.Token = integration.AccessToken;
            ViewBag.MeetingSipAddress = spotMeeting.MeetingSipAddress;

            return View();
        }

        [Authorize]
        [HttpGet("[controller]/{spotMeetingId}/Guest")]
        public async Task<IActionResult> Guest(int spotMeetingId)
        {
            // 1. Checks if the user is a teacher of the spot meeting
                // 1.1. If the user is not a teacher check if spot meeting with spotMeetingId belongs to you (user)
                // 1.2. Pass the ciscoAuthToken & meetingSipAddress
            // 2. Gets spot meeting and passes the ciscoAuthToken & meetingSipAddress

            var personalInfo = await IdentityService.FindByEmailAsync(GetUser().Identity.Name);

            // 1.
            var isTeacher = (await SpotMeetingService.GetSpotMeetingTeacherAsync(personalInfo.Email, spotMeetingId)) != null;

            if (!isTeacher)
            {
                // 1.1.
                var bookedSpotMeeting = await IdentityService.GetExistingBookedSpotMeetingAsync(personalInfo.Email, spotMeetingId);
                if (bookedSpotMeeting is null)
                    return RedirectToAction("UserSpotMeetings", "SpotMeeting");
                // 1.2.
                ViewBag.SpotMeeting = spotMeetingId;
                var guestTokenGen = await WebexService.GenerateGuestTokenAsync(personalInfo.Email, $"{personalInfo.Name} {personalInfo.Surname}");
                if (guestTokenGen.HasErrors)
                {
                    TempData["Error"] = guestTokenGen.Errors[0];
                    return RedirectToAction("UserSpotMeetings", "SpotMeeting");
                }

                ViewBag.Token = guestTokenGen.Output;
                ViewBag.MeetingSipAddress = bookedSpotMeeting.SpotMeeting.MeetingSipAddress;
                ViewBag.MeetingPassword = bookedSpotMeeting.SpotMeeting.MeetingPassword;
            }
            else
            {
                // 2.
                var spotMeeting = await SpotMeetingService.GetByIdAsync(spotMeetingId);
                if (spotMeeting is null)
                    return RedirectToAction("Manage", "SpotMeeting");

                ViewBag.SpotMeeting = spotMeetingId;
                var guestTokenGen = await WebexService.GenerateGuestTokenAsync(personalInfo.Email, $"{personalInfo.Name} {personalInfo.Surname}");
                if (guestTokenGen.HasErrors)
                {
                    TempData["Error"] = guestTokenGen.Errors[0];
                    return RedirectToAction("UserSpotMeetings", "SpotMeeting");
                }

                ViewBag.Token = guestTokenGen.Output;
                ViewBag.MeetingSipAddress = spotMeeting.MeetingSipAddress;
                ViewBag.MeetingPassword = spotMeeting.MeetingPassword;
            }

            return View();
        }

        [Authorize]
        [HttpGet("[controller]/{spotMeetingId}/[action]")]
        public async Task<IActionResult> ReadyToJoin(int spotMeetingId)
        {
            var response = new ResponseBase<bool>();

            var spotMeeting = await SpotMeetingService.GetByIdAsync(spotMeetingId);
            if (spotMeeting is null)
            {
                response.AddError("Could not check the status of the spot meeting. It may be unavailable or it may not exist.");
                return Json(response);
            }

            var isReadyToJoin = await WebexService.CheckHasMeetingStartedAsync(spotMeeting.MeetingId);
            if (isReadyToJoin.HasErrors)
            {
                response.AddErrors(isReadyToJoin.Errors);
                return Json(response);
            }

            response.Output = isReadyToJoin.Output;
            return Json(response);
        }

        // ---------------------------------------------- Helpers -------------------------------------------------------
        private bool IsSpotMeetingReadyToStart(SpotMeetingModel spotMeeting)
        {
            var now = DateTime.Now.AddMinutes(30);

            var start = spotMeeting.StartDate;
            var end = spotMeeting.StartDate.AddHours(spotMeeting.Duration);

            return (now >= start && now <= end);
        }
        #endregion
    }
}
