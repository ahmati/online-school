using AutoMapper;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineSchool.Contract.Identity;
using OnlineSchool.Contract.Infrastructure;
using OnlineSchool.Contract.SocialNetworks;
using OnlineSchool.Contract.Teachers;
using OnlineSchool.Core.Identity_.Services;
using OnlineSchool.Core.SocialNetworks_.Service;
using OnlineSchool.Core.Teachers_.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IW = ItalWebConsulting.Infrastructure.Mvc;
using X.PagedList;
using Microsoft.AspNetCore.Authorization;
using OnlineSchool.Core.Session_.Service;
using OnlineSchool.Core.Courses_;
using OnlineSchool.Contract.Resources;

namespace OnlineSchool.Site.Controllers
{
    public class TeacherController : IW.ControllerBase
    {
        public IIdentityService IdentityService { get; set; }
        public ITeacherService TeacherService { get; set; }
        public ISocialNetworkService SocialNetworkService { get; set; }

        public async Task<IActionResult> Index(int? page)
        {
            var teachers = await TeacherService.GetAllTeachersAsync();
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(teachers.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public async Task<IActionResult> Manage()
        {
            var socialNetworks = await SocialNetworkService.GetAllAsync();
            Dictionary<int, string> listType = new Dictionary<int, string>();

            foreach (var s in socialNetworks)
                listType.Add(s.Id, s.Description);

            ViewBag.SocialTypes = listType;

            return View();
        }

        [HttpGet("[controller]/{teacherId}/[action]")]
        public async Task<IActionResult> Details(int teacherId)
        {
            var teacher = await TeacherService.GetByIdAsync(teacherId);
            return View(teacher);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateTeacherModel model)
        {
            model.ImagePath = model.ImageFile?.FileName ?? null;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await TeacherService.CreateAsync(model);
            if (result.HasErrors)
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, err);
                    return View(model);
                }
            }

            TempData["Success"] = SharedResources.TeacherCreateSuccessful;
            return RedirectToAction("Manage");
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var teacher = await TeacherService.GetByIdAsync(id);
            var model = Mapper.Value.Map<UpdateTeacherModel>(teacher);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(UpdateTeacherModel model)
        {
            if (model.ImageFile != null)
                model.ImagePath = model.ImageFile.FileName;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await TeacherService.UpdateAsync(model);
            if (result.HasErrors)
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, err);
                    return View(model);
                }
            }

            TempData["Success"] = SharedResources.TeacherEditSuccessful;
            return RedirectToAction("Manage");
        }

        [HttpGet]
        public async Task<ActionResult> GetAllTeachers([DataSourceRequest] DataSourceRequest request)
        {
            var list = await TeacherService.GetAllAsync();

            var result = list.ToDataSourceResult(request);

            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> ResetPassword([FromBody] ResetUserPassword request)
        {
            var output = new ResponseBase<bool>() { Output = false };

            if (string.IsNullOrWhiteSpace(request.Email))
                output.AddError(ValidationSharedResources.InvalidEmail);

            if (string.IsNullOrWhiteSpace(request.Password))
                output.AddError(ValidationSharedResources.InvalidPassword);

            if (output.HasErrors)
                return Json(output);

            var user = await IdentityService.FindByEmailAsync(request.Email);
            if (user is null)
            {
                var error = ValidationSharedResources.UserWithEmailNotFound;
                error = error.Replace("Email", request.Email);
                output.AddError(error);
                return Json(output);
            }

            var pwToken = await IdentityService.GeneratePasswordResetTokenAsync(user);
            var result = await IdentityService.ResetPasswordAsync(user, pwToken, (request.Password));
            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                {
                    output.AddError(err.Description);
                }
                return Json(output);
            }

            output.Output = true;
            return Json(output);
        }

        [HttpGet]
        public async Task<IActionResult> GetTeacherById(int id)
        {
            var data = await TeacherService.GetByIdAsync(id);
            return Json(data);
        }

        [HttpGet]
        public async Task<JsonResult> GetAllTeachers_Dropdown()
        {
            var data = await TeacherService.GetAllAsync();
            return Json(data);
        }

        #region --------------------------------------- Teacher subjects ---------------------------------------

        [HttpGet]
        public IActionResult MySubjects()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetMySubjects([DataSourceRequest] DataSourceRequest request)
        {
            var personalInfo = await TeacherService.GetByEmailAsync(GetUser().Identity.Name);
            var mySubjects = await TeacherService.GetTeacherSubjectsAsync(personalInfo.Id);
            var result = mySubjects.ToDataSourceResult(request);
            return Json(result);
        }

        #endregion

        #region --------------------------------------- Teacher courses ---------------------------------------

        [HttpGet]
        public IActionResult MyCourses()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetMyCourses([DataSourceRequest] DataSourceRequest request)
        {
            var personalInfo = await TeacherService.GetByEmailAsync(GetUser().Identity.Name);
            var mySubjects = await TeacherService.GetTeacherCoursesAsync(personalInfo.Id);
            var result = mySubjects.ToDataSourceResult(request);
            return Json(result);
        }

        #endregion

        #region --------------------------------------- Upcoming Sessions ---------------------------------------

        [Authorize(Roles = "Teacher")]
        [HttpGet]
        public IActionResult UpcomingSessions()
        {
            return View();
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet]
        public async Task<IActionResult> GetUpcomingSessions([DataSourceRequest] DataSourceRequest request)
        {
            var personalInfo = await TeacherService.GetByEmailAsync(GetUser().Identity.Name);
            var mySubjects = await TeacherService.GetUpcomingSessionsAsync(personalInfo.Id);
            var result = mySubjects.ToDataSourceResult(request);
            return Json(result);
        }

        #endregion

        #region --------------------------------------- Teacher social network ---------------------------------------

        [HttpPost]
        public async Task<IActionResult> CreateSocial([FromBody] TeacherSocialNetworkModel teacherSocialNetworkModel)
        {
            var output = new ResponseBase<bool>() { Output = false };

            if (!ModelState.IsValid)
            {
                ModelState.Keys.Each(k =>
                {
                    ModelState[k].Errors.Each(e => output.AddError(e.ErrorMessage));
                });
                return Json(output);
            }

            var result = await TeacherService.CreateTeacherSocialNetworkAsync(teacherSocialNetworkModel);
            return Json(output);
        }

        [HttpPost]
        public async Task<IActionResult> EditSocial([FromBody] TeacherSocialNetworkModel model)
        {
            var output = new ResponseBase<bool>() { Output = false };

            if (!ModelState.IsValid)
            {
                ModelState.Keys.Each(k =>
                {
                    ModelState[k].Errors.Each(e => output.AddError(e.ErrorMessage));
                });
                return Json(output);
            }

            var result = await TeacherService.UpdateTeacherSocialNetworkAsync(model);
            return Json(output);
        }

        [HttpPost]
        [HttpDelete]
        public async Task<IActionResult> DeleteSocial([FromBody] TeacherSocialNetworkModel model)
        {
            var result = await TeacherService.DeleteTeacherSocialNetworkAsync(model.TeacherId, model.SocialNetworkId);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetTeacherSocialNetworks([DataSourceRequest] DataSourceRequest request, int teacherId)
        {

            var data = await TeacherService.GetTeacherSocialNetworksAsync(teacherId);
            var result = data.ToDataSourceResult(request);
            return Json(result);
        }

        #endregion

        // ------------------------------------ OLD --------------------------------------------------------------------

        [HttpGet]
        public async Task<IActionResult> GetSocial([DataSourceRequest] DataSourceRequest request)
        {
            var data = await SocialNetworkService.GetAllAsync();
            var result = data.ToDataSourceResult(request);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetTeacherSubject(int teacherId)
        {
            var data = await TeacherService.GetTeacherAsync(teacherId);

            //var result = data.ToDataSourceResult(request);

            //return Json(result);
            return PartialView("_TeacherProfile", data);
        }
    }
}