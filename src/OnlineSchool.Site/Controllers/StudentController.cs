using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineSchool.Contract.Identity;
using OnlineSchool.Contract.Infrastructure;
using OnlineSchool.Contract.Resources;
using OnlineSchool.Contract.Students;
using OnlineSchool.Core.Courses_;
using OnlineSchool.Core.Identity_.Services;
using OnlineSchool.Core.Students_.Service;
using System.Threading.Tasks;
using IW = ItalWebConsulting.Infrastructure.Mvc;

namespace OnlineSchool.Site.Controllers
{
    public class StudentController : IW.ControllerBase<StudentController>
    {
        public IIdentityService IdentityService { get; set; }
        public IStudentService StudentService { get; set; }
        public ICourseService CourseService { get; set; }

        [HttpGet]
        public IActionResult Manage()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateStudentModel model)
        {
            model.ImagePath = model.ImageFile?.FileName ?? null;

            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await StudentService.CreateAsync(model);
            if(result.HasErrors)
            {
                foreach(var err in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, err);
                    return View(model);
                }
            }

            TempData["Success"] = SharedResources.StudentCreateSuccessful;
            return RedirectToAction("Manage");
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var student = await StudentService.GetByIdAsync(id);
            var model = Mapper.Value.Map<UpdateStudentModel>(student);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(UpdateStudentModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await StudentService.UpdateAsync(model);
            if (result.HasErrors)
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, err);
                    return View(model);
                }
            }

            TempData["Success"] = SharedResources.StudentEditSuccessful;
            return RedirectToAction("Manage");
        }

        [HttpGet]
        public async Task<ActionResult> GetAllStudents([DataSourceRequest] DataSourceRequest request)
        {
            var list = await StudentService.GetAllAsync();

            var result = list.ToDataSourceResult(request);

            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> ResetPassword([FromBody] ResetUserPassword request)
        {
            var output = new ResponseBase<bool>() { Output = false };

            if (request is null)
                output.AddError("Invalid request");

            if (string.IsNullOrWhiteSpace(request.Email))
                output.AddError("Invalid email");

            if (string.IsNullOrWhiteSpace(request.Password))
                output.AddError("Invalid password");

            if (output.HasErrors)
                return Json(output);

            var user = await IdentityService.FindByEmailAsync(request.Email);
            if(user is null)
            {
                output.AddError($"User with email '{request.Email}' not found.");
                return Json(output);
            }

            var pwToken = await IdentityService.GeneratePasswordResetTokenAsync(user);
            var result = await IdentityService.ResetPasswordAsync(user, pwToken, request.Password);
            if(!result.Succeeded)
            {
                foreach(var err in result.Errors)
                {
                    output.AddError(err.Description);
                }
                return Json(output);
            }

            output.Output = true;
            return Json(output);
        }

        // API for dropdown usage
        [HttpGet]
        public async Task<JsonResult> GetAllStudents_Dropdown()
        {
            var data = await StudentService.GetAllAsync();
            return Json(data);
        }

        #region --------------------------------------- Student courses ---------------------------------------

        [Authorize(Roles = "Student")]
        [HttpGet]
        public IActionResult MyCourses()
        {
            return View();
        }

        [Authorize(Roles = "Student")]
        [HttpGet]
        public async Task<ActionResult> GetMyCourses([DataSourceRequest] DataSourceRequest request)
        {
            var personalInfo = await StudentService.GetByEmailAsync(GetUser().Identity.Name);
            var myCourses = await StudentService.GetStudentCoursesAsync(personalInfo.Id);
            var result = myCourses.ToDataSourceResult(request);
            return Json(result);
        }

        [Authorize(Roles = "Student")]
        [HttpGet("[controller]/MyCourses/{courseId}/Details")]
        public async Task<IActionResult> MyCourseDetails(int courseId)
        {
            var personalInfo = await StudentService.GetByEmailAsync(GetUser().Identity.Name);

            var isCourseBought = await StudentService.GetExistingBookedCourseAsync(personalInfo.Email, courseId);
            if (isCourseBought == null)
                return RedirectToAction("MyCourses");
            var course = await CourseService.GetByIdAsync(courseId);

            return View("MyCourseDetails", course);

        }

        #endregion

        #region --------------------------------------- Upcoming Sessions ---------------------------------------

        [Authorize(Roles = "Student")]
        [HttpGet]
        public IActionResult UpcomingSessions()
        {
            return View();
        }

        [Authorize(Roles = "Student")]
        [HttpGet]
        public async Task<IActionResult> GetUpcomingSessions([DataSourceRequest] DataSourceRequest request)
        {
            var personalInfo = await StudentService.GetByEmailAsync(GetUser().Identity.Name);
            var sessions = await StudentService.GetUpcmonigSessions(personalInfo.Id);
            var result = sessions.ToDataSourceResult(request);

            return Json(result);
        }
        #endregion


    }
}