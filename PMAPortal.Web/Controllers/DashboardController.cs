using Microsoft.AspNetCore.Mvc;
using PMAPortal.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IUserService userService;
        private readonly IApplicationService applicationService;

        public DashboardController(IUserService userService,
            IApplicationService applicationService)
        {
            this.userService = userService;
            this.applicationService = applicationService;
        }
        public IActionResult Index()
        {
            var userCount = userService.GetUsers().Count();
            ViewData["UsersCount"] = userCount;
            return View();
        }
    }
}
