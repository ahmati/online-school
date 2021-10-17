using ItalWebConsulting.Infrastructure.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineSchool.Contract.Infrastructure;
using OnlineSchool.Contract.Lessons;
using OnlineSchool.Core.Lessons_.Service;
using OnlineSchool.Core.Schedules_.Service;
using OnlineSchool.Core.Students_.Service;
using OnlineSchool.Core.TeacherSubject_.Service;
using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineSchool.Site.Controllers
{
    //[Authorize]
    public class LessonController : ControllerBase<LessonController>
    {
        public IScheduleService ScheduleService { get; set; }
        public ILessonService LessonService { get; set; }
        public ITeacherSubjectService TeacherSubjectService { get; set; }
        public IStudentService StudentService { get; set; }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LessonModel model)
        {
            if (!ModelState.IsValid)
                return Json(model);

            var result = await LessonService.CreateAsync(model);
            if (result.HasErrors)
            {
                foreach (var err in result.Errors)
                    ModelState.AddModelError(string.Empty, err);
                return Json(result);
            }

            return Json(result);

        }

        [HttpPost]
        public async Task<ActionResult> Edit([FromBody] LessonModel model)
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

            var result = await LessonService.UpdateAsync(model);
            if (result.HasErrors)
            {
                foreach (var err in result.Errors)
                    ModelState.AddModelError(string.Empty, err);
                return Json(result);
            }
            return Json(output);
        }

        [HttpPost]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var lesson = await LessonService.GetByIdAsync(id);
            if(lesson is null)
                return BadRequest("An error occured while deleting lesson.");

            var result = await LessonService.DeleteAsync(id);
            if(!result.HasErrors)
            {
                var dir = $"{WebHostEnvironment.Value.ContentRootPath}\\private_uploads\\materials\\teachersubjects\\{lesson.TeacherSubjectId}\\lessons\\{id}";
                if(System.IO.Directory.Exists(dir))
                {
                    try
                    {
                        var lessonMaterials = System.IO.Directory.EnumerateFiles(dir);
                        foreach (var lessonMaterial in lessonMaterials)
                        {
                            System.IO.File.Delete(lessonMaterial);
                        }
                    } catch {}
                }
            }
            return Json(result);
        }

        //[Authorize]
        public async Task<IActionResult> LessonMaterials([FromBody] LessonModel lesson)
        {
            var teacherSubject = await TeacherSubjectService.GetByIdAsync(lesson.TeacherSubjectId);
            var bookedCourse = await StudentService.GetExistingBookedCourseAsync(GetUser().Identity.Name, teacherSubject.Id);
            
            if (bookedCourse == null)
            {
                return Redirect($"/Payment?teacherSubjectId={teacherSubject.Id}");
            }

            var model = await LessonService.GetByIdAsync(lesson.Id);

            return View(model);
        }
    }
}