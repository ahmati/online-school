using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineSchool.Contract.Identity.Models;
using OnlineSchool.Contract.Roles;
using OnlineSchool.Core.Roles_.Services;
using System.Threading.Tasks;

namespace OnlineSchool.Site.Controllers
{
    [Authorize]
    public class RoleController : ItalWebConsulting.Infrastructure.Mvc.ControllerBase
    {
        public IRoleService RoleService { get; set; }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetAllUsers([DataSourceRequest] DataSourceRequest request)
        {
            var getUserRoles = await RoleService.GetAllUserRoles();
            var result = getUserRoles.ToDataSourceResult(request);
            return Json(result);
        }

        [HttpGet]
        public async Task<ActionResult> Read_AllRoleData([DataSourceRequest] DataSourceRequest request)
        {
            var data = await RoleService.GetAllRolesAsync();
            return Json(data);
        }

        [AcceptVerbs("Post")]
        public async Task<ActionResult> Create_Role([DataSourceRequest] DataSourceRequest request, Ruoli role)
        {
            bool result = await RoleService.RoleExists(role.Name);
            if (role != null && ModelState.IsValid && !result)
                result = await RoleService.CreateRole(new Microsoft.AspNetCore.Identity.IdentityRole(role.Name));

            if (!result)
                ModelState.AddModelError("error", "Role gia esiste");

            return Json(new[] { role }.ToDataSourceResult(request, ModelState));

        }

        [AcceptVerbs("Post")]
        public async Task<ActionResult> Update_Role([DataSourceRequest] DataSourceRequest request, Ruoli role)
        {
            if (role != null && ModelState.IsValid)
                await RoleService.UpdateRoleAsync(role);

            return Json(new[] { role }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs("Post")]
        public async Task<ActionResult> DeleteUser([DataSourceRequest] DataSourceRequest request, string id)
        {
            if (id != null)
                await RoleService.DeleteUser(id);

            return Json(request, ModelState);
        }

        public async Task<ActionResult> EditUserRole(string userId)
        {
            var model = await RoleService.GetUserById(userId);
            return View(model);
        }

        [AcceptVerbs("Post")]
        public async Task<ActionResult> UpdateUser(ApplicationUser user)
        {
            var model = await RoleService.UpdateUser(user);
            if (model == 1)
            {
                return View("Index");
            }
            else
            {
                return null;
            }
        }
    }
}