using ItalWebConsulting.Infrastructure.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using OnlineSchool.Contract.Resources;
using OnlineSchool.Contract.Subject;
using System;
using System.Threading.Tasks;

namespace OnlineSchool.Site.Controllers
{
    public class SubjectController : ControllerBase<SubjectController>
    {
        public ISubjectService SubjectService { get; set; }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSubjects([DataSourceRequest] DataSourceRequest request)
        {
            var result = await SubjectService.GetAllAsync();
            var data = result.ToDataSourceResult(request);

            return Json(data);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllSubjects_DropDown([DataSourceRequest] DataSourceRequest request)
        {
            var result = await SubjectService.GetAllAsync();
            
            return Json(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateSubjectModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await SubjectService.CreateAsync(model);
            if (result.HasErrors)
            {
                foreach (var err in result.Errors)
                    ModelState.AddModelError(string.Empty, err);
                return View(model);
            }

            TempData["Success"] = SharedResources.SubjectCreateSuccess;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var subject = await SubjectService.GetByIdAsync(id);
            var model = Mapper.Value.Map<UpdateSubjectModel>(subject);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([DataSourceRequest] DataSourceRequest request, UpdateSubjectModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await SubjectService.UpdateAsync(model);
            if(result.HasErrors)
            {
                foreach (var err in result.Errors)
                    ModelState.AddModelError(string.Empty, err);
                return View(model);
            }

            TempData["Success"] = SharedResources.SubjectUpdateSuccess;
            return RedirectToAction("Index");
        }

        [HttpPost]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await SubjectService.DeleteAsync(id);
            return Json(result);
        }

    }
}