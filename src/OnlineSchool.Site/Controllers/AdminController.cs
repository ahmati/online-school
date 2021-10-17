using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineSchool.Domain.OnlineSchoolDB.EF;

namespace OnlineSchool.Site.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly UserManager<AspNetUserRoles> userManager;
        public AdminController(UserManager<AspNetUserRoles> usrMgr)
        {
            userManager = usrMgr;
        }
        public ViewResult Index() => View(userManager.Users);
    }
}