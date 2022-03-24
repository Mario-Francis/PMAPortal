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
        private readonly IFeedbackService feedbackService;
        private readonly IInstallationService installationService;
        private readonly IInstallationBatchService installationBatchService;
        private readonly IBatchService batchService;
        private readonly ICustomerService customerService;
        private readonly ISurveyService surveyService;
        private readonly ILoggerService<DashboardController> logger;

        public DashboardController(IUserService userService,
            IFeedbackService feedbackService,
            IInstallationService installationService,
            IInstallationBatchService installationBatchService,
            IBatchService batchService,
            ICustomerService customerService,
            ISurveyService surveyService,
            ILoggerService<DashboardController> logger)
        {
            this.userService = userService;
            this.feedbackService = feedbackService;
            this.installationService = installationService;
            this.installationBatchService = installationBatchService;
            this.batchService = batchService;
            this.customerService = customerService;
            this.surveyService = surveyService;
            this.logger = logger;
        }
        public IActionResult Index()
        {
            var currentUser = HttpContext.GetUserSession();
            var customers = customerService.GetCustomers();
            var installations = installationService.GetInstallations();
            var model = new DashboardVM
            {
                UserCount = userService.GetUsers().Count(),
                CustomerCount=customers.Count(),
                
                PendingSurveyCount = customers.Where(c=> c.Surveys.Count()==0 || c.Surveys.FirstOrDefault()?.SurveyRemark==null).Count(),
                CompletedSurveyCount = customers.Where(c => c.Surveys.Count() > 0 && c.Surveys.FirstOrDefault()?.SurveyRemark != null).Count(),
                MeterReadyCount = customers.Where(c => c.Surveys.Count() > 0 && c.Surveys.FirstOrDefault()?.SurveyRemark == Constants.SURVEY_REMARK_METER_READY).Count(),
                NotMeterReadyCount = customers.Where(c => c.Surveys.Count() > 0 && c.Surveys.FirstOrDefault()?.SurveyRemark == Constants.SURVEY_REMARK_NOT_METER_READY).Count(),

                PendingInstallationCount = installations.Where(i=>i.InstallationStatusId == (long)InstallationStatuses.Pending ||
                i.InstallationStatusId == (long)InstallationStatuses.Scheduled_for_Installation || i.InstallationStatusId == (long)InstallationStatuses.Installation_In_Progress).Count(),
                CompletedInstallationCount= installations.Where(i => i.InstallationStatusId == (long)InstallationStatuses.Installation_Completed).Count(),
                RejectedInstallationCount= installations.Where(i => i.InstallationStatusId == (long)InstallationStatuses.Disco_Confirmation_Failed).Count(),
                ApprovedInstallationCount= installations.Where(i => i.InstallationStatusId == (long)InstallationStatuses.Disco_Confirmation_Successful).Count(),

                FeedbackCount = feedbackService.GetFeedbacks().Count(),
                CustomerBatchCount=batchService.GetBatches().Count(),
                InstallerBatchCount=installationBatchService.GetBatches().Count(),
            };

            if (User.IsInRole(Constants.ROLE_SURVEY_STAFF))
            {
                model.MyPendingSurveyCount = customers.Where(c => c.Surveys.FirstOrDefault()?.SurveyRemark == null && c.Surveys.FirstOrDefault()?.SurveyStaffId == currentUser.Id).Count();
                model.CompletedSurveyCount = customers.Where(c => c.Surveys.FirstOrDefault()?.SurveyRemark != null && c.Surveys.FirstOrDefault()?.SurveyStaffId == currentUser.Id).Count();
            }

            if (User.IsInRole(Constants.ROLE_INSTALLER))
            {
                model.MyPendingInstallationCount = installations.Where(i => (i.InstallationStatusId == (long)InstallationStatuses.Pending ||
                i.InstallationStatusId == (long)InstallationStatuses.Scheduled_for_Installation || i.InstallationStatusId == (long)InstallationStatuses.Installation_In_Progress) && i.InstallerId == currentUser.Id).Count();
                model.MyCompletedInstallationCount = installations.Where(i => i.InstallationStatusId == (long)InstallationStatuses.Installation_Completed && i.InstallerId == currentUser.Id).Count();
                model.MyApprovedInstallationCount = installations.Where(i => i.InstallationStatusId == (long)InstallationStatuses.Disco_Confirmation_Successful && i.InstallerId == currentUser.Id).Count();
                model.MyRejectedInstallationCount = installations.Where(i => i.InstallationStatusId == (long)InstallationStatuses.Disco_Confirmation_Failed && i.InstallerId == currentUser.Id).Count();
            }

            return View(model);
        }
    }
}
