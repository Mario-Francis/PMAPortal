using Microsoft.AspNetCore.Mvc;
using PMAPortal.Web.Extensions;
using PMAPortal.Web.Services;
using PMAPortal.Web.ViewModels;
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
        private readonly IFeedbackService feedbackService;

        public DashboardController(IUserService userService,
            IApplicationService applicationService,
            IFeedbackService feedbackService)
        {
            this.userService = userService;
            this.applicationService = applicationService;
            this.feedbackService = feedbackService;
        }
        public IActionResult Index()
        {
            var currentUser = HttpContext.GetUserSession();
            var model = new DashboardVM
            {
                UsersCount = userService.GetUsers().Count(),
                TotalApplicationCount = applicationService.GetApplications(new long[] { 2, 3, 4, 5, 6, 7, 8 }).Count(),
                UnassignedApplicationCount = applicationService.GetApplications(new long[] { 2, 3, 4, 5, 6, 7, 8 }).Where(a => a.InstallerId == null).Count(),
                TotalAssignedApplicationCount = applicationService.GetApplications(new long[] { 2, 3, 4, 5, 6, 7, 8 }).Where(a => a.InstallerId == currentUser.Id).Count(),
                PendingApplicationCount = applicationService.GetApplications(new long[] { 2, 3, 4, 6 }).Count(),
                FailedApplicationCount = applicationService.GetApplications(new long[] { 5, 7 }).Count(),
                CompletedApplicationCount = applicationService.GetApplications(new long[] { 8 }).Count(),
                TotalIncome = applicationService.GetApplications(new long[] { 2, 3, 4, 5, 6, 7, 8 }).Select(a => a.Payments.First().Amount).Sum().Format(),
                CompletedIncome = applicationService.GetApplications(new long[] { 8 }).Select(a => a.Payments.First().Amount).Sum().Format(),
                FailedIncome = applicationService.GetApplications(new long[] { 5, 7 }).Select(a => a.Payments.First().Amount).Sum().Format(),
                PendingIncome = applicationService.GetApplications(new long[] { 2, 3, 4, 6 }).Select(a => a.Payments.First().Amount).Sum().Format(),
                FeedbacksCount = feedbackService.GetFeedbacks().Count(),
                TotalAmountByType = applicationService.GetApplications(new long[] { 2, 3, 4, 5, 6, 7, 8 }).GroupBy(a => a.Meter).Select(g => new ReportItemVM
                {
                    Name = g.Key.Name,
                    Amount = g.Select(i => i.Payments.First().Amount).Sum(),
                    FormattedAmount = g.Select(i => i.Payments.First().Amount).Sum().Format()
                })
            };

            if (User.IsInRole(Constants.ROLE_INSTALLER))
            {
                model.PendingApplicationCount = applicationService.GetApplications(new long[] { 2, 3, 4 }).Where(a => a.InstallerId == currentUser.Id).Count();
                model.FailedApplicationCount = applicationService.GetApplications(new long[] { 5, 7 }).Where(a => a.InstallerId == currentUser.Id).Count();
                model.CompletedApplicationCount = applicationService.GetApplications(new long[] { 6, 8 }).Where(a => a.InstallerId == currentUser.Id).Count();
            }

            if (User.IsInRole(Constants.ROLE_DISCO))
            {
                model.PendingApplicationCount = applicationService.GetApplications(new long[] { 6 }).Where(a => a.InstallerId == currentUser.Id).Count();
                model.FailedApplicationCount = applicationService.GetApplications(new long[] { 7 }).Where(a => a.InstallerId == currentUser.Id).Count();
                model.CompletedApplicationCount = applicationService.GetApplications(new long[] { 8 }).Where(a => a.InstallerId == currentUser.Id).Count();
            }

            return View(model);
        }
    }
}
