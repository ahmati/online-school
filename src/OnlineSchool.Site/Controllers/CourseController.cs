using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NodaTime;
using OnlineSchool.Contract.Courses;
using OnlineSchool.Contract.Session;
using OnlineSchool.Contract.Subject;
using OnlineSchool.Core.Courses_;
using OnlineSchool.Core.Identity_.Services;
using OnlineSchool.Core.Lessons_.Service;
using OnlineSchool.Core.Materials_.Services;
using OnlineSchool.Core.Session_.Service;
using OnlineSchool.Core.Students_.Service;
using OnlineSchool.Core.Subjects_.Services;
using OnlineSchool.Core.Teachers_.Service;
using OnlineSchool.Core.TeacherSubject_.Service;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using IW = ItalWebConsulting.Infrastructure.Mvc;
using X.PagedList;
using OnlineSchool.Contract.Contacts;
using OnlineSchool.Contract.Resources;

namespace OnlineSchool.Site.Controllers
{
    public class CourseController : IW.ControllerBase
    {
        public ICourseService CourseService { get; set; }
        public ISessionService SessionService { get; set; }
        public ITeacherSubjectService TeacherSubjectService { get; set; }
        public ITeacherService TeacherService { get; set; }
        public ISubjectService SubjectService { get; set; }
        public IStudentService StudentService { get; set; }
        public IIdentityService IdentityService { get; set; }
        public ILessonService LessonService { get; set; }
        public IMaterialService MaterialService { get; set; }

        public async Task<IActionResult> Index(string search, int? page)
        {
            var courses = await CourseService.GetAllPublishedAsync(search);

            int pageSize = 6;
            int pageNumber = (page ?? 1);

            return View(courses.ToPagedList(pageNumber, pageSize));
        }

        [Authorize(Roles = "Admin, Teacher")]
        public IActionResult Manage()
        {
            return View();
        }

        [HttpGet("[controller]/{id}/[action]")]
        public async Task<IActionResult> Details(int id)
        {
            var course = await CourseService.GetByIdAsync(id);
            if (course is null)
                return RedirectToAction("Error", "Home");

            var teacherSubject = await TeacherSubjectService.GetByIdAsync(course.TeacherSubjectId);
            ViewBag.TeacherSubject = teacherSubject;

            var teacherSocialNetworks = await TeacherService.GetTeacherSocialNetworksAsync(teacherSubject.TeacherId);
            ViewBag.TeacherSocialNetworks = teacherSocialNetworks;

            ViewBag.IsBooked = (await StudentService.GetExistingBookedCourseAsync(GetUser().Identity.Name, id)) != null;

            var bookedCourses = await CourseService.GetBookedCoursesByTeacherSubjectIdAsync(teacherSubject.Id);
            ViewBag.BookedCoursesCount = bookedCourses.Count();

            ViewBag.Category = await SubjectService.GetCategoryByIdAsync(ViewBag.TeacherSubject.Subject.CategoryId);

            ViewBag.Lessons = await LessonService.GetLessonsByTeacherSubjectId(teacherSubject.Id);

            ViewBag.Sessions = course.Sessions.OrderBy(s => s.StartTime);
            ViewBag.NextSessionDate = course.Sessions
                .Where(s => s.StartDate > DateTime.Now || (s.StartDate < DateTime.Now  && s.EndDate > DateTime.Now))
                .Select(s => s.StartDate)
                .OrderBy(s => s.TimeOfDay)
                .FirstOrDefault();

            ViewBag.PurchasableItemType = PurchasableItemType.Course;

            ViewBag.CurrentAvailableSpots = (course.AvailableSpots - ViewBag.BookedCoursesCount) < 0 ? 0 : (course.AvailableSpots - ViewBag.BookedCoursesCount);

            return View(course);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> Create(CreateCourseModel model)
        {
            model.ImagePath = model.ImageFile?.FileName ?? null;

            var result = await CourseService.CreateAsync(model);
            if (result.HasErrors)
            {
                foreach (var err in result.Errors)
                    ModelState.AddModelError(string.Empty, err);
                return View(model);
            }

            TempData["Success"] = SharedResources.CourseCreateSuccess;
            return RedirectToAction("Manage");
        }

        [Authorize(Roles = "Admin, Teacher")]
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var course = await CourseService.GetByIdAsync(id);

            var model = Mapper.Value.Map<UpdateCourseModel>(course);
            return View(model);
        }

        [Authorize(Roles = "Admin, Teacher")]
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateCourseModel model)
        {
            if (model.ImageFile != null)
                model.ImagePath = model.ImageFile.FileName;

            var result = await CourseService.UpdateAsync(model);
            if (result.HasErrors)
            {
                foreach (var err in result.Errors)
                    ModelState.AddModelError(string.Empty, err);
                return View(model);
            }

            TempData["Success"] = SharedResources.CourseUpdateSuccess;
            return RedirectToAction("Manage");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost] [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await CourseService.DeleteAsync(id);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCourses([DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<CourseModel> list;
            if (User.IsInRole("Admin"))
            {
                list = await CourseService.GetAllAsync();
            }
            else
            {
                var teacher = await TeacherService.GetByEmailAsync(GetUser().Identity.Name);
                list = await TeacherService.GetTeacherCoursesAsync(teacher.Id);
            }

            var result = list.ToDataSourceResult(request);
            return Json(result);
        }

        [Authorize(Roles = "Admin, Teacher")]
        [HttpPost]
        public async Task<IActionResult> Publish(int id)
        {
            var result = await CourseService.PublishAsync(id);
            return Json(result);
        }

        [Authorize(Roles = "Admin, Teacher")]
        [HttpPost]
        public async Task<IActionResult> Unpublish(int id)
        {
            var result = await CourseService.UnpublishAsync(id);
            return Json(result);
        }

        [Authorize(Roles = "Student")]
        [HttpGet]
        public async Task<IActionResult> JoinCourse(int id)
        {
            var email = GetUser().Identity.Name;

            var isPurchased = await CourseService.GetExistingPurchasedItemAsync(email, id);
            if (isPurchased.Output)
            {
                return RedirectToAction("Details", new { id = id });
            }

            var result = await CourseService.JoinAsync(id, email);
            if (result.HasErrors)
            {
                TempData["Error"] = result.Errors;
            }

            TempData["Success"] = SharedResources.JoinCourseSuccess;
            return RedirectToAction("Details", new { id = id });
        }

        #region Course - Session

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetSessionByCourseId([DataSourceRequest] DataSourceRequest request,int courseId )
        {
            var data = await CourseService.GetSessionsAsync(courseId);

            var result = data.ToDataSourceResult(request);
            return Json(result);
        }

        [Authorize(Roles = "Admin, Teacher")]
        [HttpPost]
        public async Task<IActionResult> CreateSession([FromBody] CreateSessionModel model)
        {
            var result = await CourseService.CreateSessionAsync(model);
            return Json(result);
        }

        [Authorize(Roles = "Admin, Teacher")]
        [HttpDelete("[controller]/session/{id}/delete")]
        public async Task<IActionResult> DeleteSession(int id)
        {
            var result = await CourseService.DeleteSessionAsync(id);
            return Json(result);
        }

        #endregion
    }
}
