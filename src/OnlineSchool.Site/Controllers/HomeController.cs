using ItalWebConsulting.Infrastructure.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineSchool.Contract.Globalization;
using OnlineSchool.Contract.Students;
using OnlineSchool.Contract.Subject;
using OnlineSchool.Contract.Teachers;
using OnlineSchool.Core.Courses_;
using OnlineSchool.Core.SpotMeeting_.Service;
using OnlineSchool.Core.Students_.Service;
using OnlineSchool.Core.Subjects_.Services;
using OnlineSchool.Core.Teachers_.Service;
using OnlineSchool.Core.Test_.Services;
using OnlineSchool.Site.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineSchool.Site.Controllers
{
    //[Authorize]
    public class HomeController : ControllerBase<HomeController>
    {
        private readonly ILogger<HomeController> _logger;
        public ITestService TestService { get; set; }
        public ITeacherService TeacherService { get; set; }
        public IStudentService StudentService { get; set; }
        public ICourseService CourseService { get; set; }
        public ISpotMeetingService SpotMeetingService { get; set; }

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetLanguage(string culture, string returnUrl)
        {
            HttpContext.Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture, culture)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(2),
                    IsEssential = true,
                    Path = "/",
                    HttpOnly = false,
                    Secure = true,
                });

            return LocalRedirect(returnUrl);
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var student = await StudentService.GetByEmailAsync(GetUser().Identity.Name);
                if(student != null)
                    ViewData["StudentId"] = student.Id;

                var teachers = await TeacherService.GetAllTeachersAsync();

                ViewBag.StudentsNumber = await StudentService.GetCountAsync();

                var courses = await CourseService.GetAllPublishedAsync();
                ViewBag.Courses = courses;

                ViewBag.CoursesNumber = courses.Count();

                var meetings = await SpotMeetingService.GetAllPublishedAsync();
                ViewBag.Meetings = meetings;

                ViewBag.MeetingsNumber = meetings.Count();

                return View(teachers);
            }
            catch (Exception ex)
            {
                Logger.LogInformation(ex.Message);
                return Error();
            } 
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
