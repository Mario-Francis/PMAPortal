using DataTablesParser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PMAPortal.Web.DTOs;
using PMAPortal.Web.Extensions;
using PMAPortal.Web.Services;
using PMAPortal.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.Controllers
{
    public class InstallationsController : Controller
    {
        private readonly IOptionsSnapshot<AppSettings> appSettingsDelegate;
        private readonly ICustomerService customerService;
        private readonly ISurveyService surveyService;
        private readonly IInstallationService installationService;
        private readonly ILoggerService<InstallationsController> logger;

        public InstallationsController(
             IOptionsSnapshot<AppSettings> appSettingsDelegate,
            ICustomerService customerService,
            ISurveyService surveyService,
            IInstallationService installationService,
            ILoggerService<InstallationsController> logger)
        {
            this.appSettingsDelegate = appSettingsDelegate;
            this.customerService = customerService;
            this.surveyService = surveyService;
            this.installationService = installationService;
            this.logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult UnassignedDataTable()
        {
            var clientTimeOffset = string.IsNullOrEmpty(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]) ?
                appSettingsDelegate.Value.DefaultTimeZoneOffset : Convert.ToInt32(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]);

            var customers = installationService.GetUnassigned().Select(c => CustomerVM.FromCustomer(c, clientTimeOffset));

            var parser = new Parser<CustomerVM>(Request.Form, customers.AsQueryable())
                   .SetConverter(x => x.CreatedDate, x => x.CreatedDate.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.DateShared, x => x.DateShared == null ? "" : x.DateShared.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.ScheduleDate, x => x.ScheduleDate == null ? "" : x.ScheduleDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.IScheduleDate, x => x.IScheduleDate == null ? "" : x.IScheduleDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.SurveyDate, x => x.SurveyDate == null ? "" : x.SurveyDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"));

            return Ok(parser.Parse());
        }

        [HttpPost]
        public IActionResult AssignedDataTable()
        {
            var clientTimeOffset = string.IsNullOrEmpty(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]) ?
                appSettingsDelegate.Value.DefaultTimeZoneOffset : Convert.ToInt32(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]);

            var customers = installationService.GetAssigned().Select(c => CustomerVM.FromCustomer(c, clientTimeOffset));

            var parser = new Parser<CustomerVM>(Request.Form, customers.AsQueryable())
                   .SetConverter(x => x.CreatedDate, x => x.CreatedDate.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.DateShared, x => x.DateShared == null ? "" : x.DateShared.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.ScheduleDate, x => x.ScheduleDate == null ? "" : x.ScheduleDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.IScheduleDate, x => x.IScheduleDate == null ? "" : x.IScheduleDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.SurveyDate, x => x.SurveyDate == null ? "" : x.SurveyDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"));

            return Ok(parser.Parse());
        }

        [HttpPost]
        public IActionResult RejectedDataTable()
        {
            var clientTimeOffset = string.IsNullOrEmpty(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]) ?
                appSettingsDelegate.Value.DefaultTimeZoneOffset : Convert.ToInt32(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]);

            var customers = installationService.GetRejected().Select(i => CustomerVM.FromCustomer(i.Customer, clientTimeOffset));

            var parser = new Parser<CustomerVM>(Request.Form, customers.AsQueryable())
                   .SetConverter(x => x.CreatedDate, x => x.CreatedDate.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.DateShared, x => x.DateShared == null ? "" : x.DateShared.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.ScheduleDate, x => x.ScheduleDate == null ? "" : x.ScheduleDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.IScheduleDate, x => x.IScheduleDate == null ? "" : x.IScheduleDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.SurveyDate, x => x.SurveyDate == null ? "" : x.SurveyDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"));

            return Ok(parser.Parse());
        }

        [HttpPost]
        public IActionResult CompletedDataTable()
        {
            var clientTimeOffset = string.IsNullOrEmpty(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]) ?
                appSettingsDelegate.Value.DefaultTimeZoneOffset : Convert.ToInt32(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]);

            var customers = installationService.GetCompleted().Select(i => CustomerVM.FromCustomer(i.Customer, clientTimeOffset));

            var parser = new Parser<CustomerVM>(Request.Form, customers.AsQueryable())
                   .SetConverter(x => x.CreatedDate, x => x.CreatedDate.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.DateShared, x => x.DateShared == null ? "" : x.DateShared.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.ScheduleDate, x => x.ScheduleDate == null ? "" : x.ScheduleDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.IScheduleDate, x => x.IScheduleDate == null ? "" : x.IScheduleDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.SurveyDate, x => x.SurveyDate == null ? "" : x.SurveyDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"));

            return Ok(parser.Parse());
        }

        [HttpPost]
        public IActionResult DiscoRejectedDataTable()
        {
            var clientTimeOffset = string.IsNullOrEmpty(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]) ?
                appSettingsDelegate.Value.DefaultTimeZoneOffset : Convert.ToInt32(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]);

            var customers = installationService.GetDiscoRejected().Select(i => CustomerVM.FromCustomer(i.Customer, clientTimeOffset));

            var parser = new Parser<CustomerVM>(Request.Form, customers.AsQueryable())
                   .SetConverter(x => x.CreatedDate, x => x.CreatedDate.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.DateShared, x => x.DateShared == null ? "" : x.DateShared.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.ScheduleDate, x => x.ScheduleDate == null ? "" : x.ScheduleDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.IScheduleDate, x => x.IScheduleDate == null ? "" : x.IScheduleDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.SurveyDate, x => x.SurveyDate == null ? "" : x.SurveyDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"));

            return Ok(parser.Parse());
        }

        [HttpPost]
        public IActionResult DiscoApprovedDataTable()
        {
            var clientTimeOffset = string.IsNullOrEmpty(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]) ?
                appSettingsDelegate.Value.DefaultTimeZoneOffset : Convert.ToInt32(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]);

            var customers = installationService.GetDiscoApproved().Select(i => CustomerVM.FromCustomer(i.Customer, clientTimeOffset));

            var parser = new Parser<CustomerVM>(Request.Form, customers.AsQueryable())
                   .SetConverter(x => x.CreatedDate, x => x.CreatedDate.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.DateShared, x => x.DateShared == null ? "" : x.DateShared.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.ScheduleDate, x => x.ScheduleDate == null ? "" : x.ScheduleDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.IScheduleDate, x => x.IScheduleDate == null ? "" : x.IScheduleDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.SurveyDate, x => x.SurveyDate == null ? "" : x.SurveyDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"));

            return Ok(parser.Parse());
        }


        [HttpPost]
        public async Task<IActionResult> AssignInstallation(long? installationId, long? installerId)
        {
            try
            {
                if (installationId == null || installerId == null)
                {
                    return StatusCode(400, new { IsSuccess = false, Message = "Invalid installation/installer id", ErrorItems = new string[] { } });
                }
                else
                {
                    await installationService.AssignInstallation(installerId.Value, installationId.Value);
                    return Ok(new { IsSuccess = true, Message = "Installation assigned successfully", ErrorItems = new string[] { } });
                }
            }
            catch (AppException ex)
            {
                return StatusCode(400, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "An error was encountered while assigning installation");

                return StatusCode(500, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AssignInstallations(IEnumerable<long> installationIds, long? installerId)
        {
            try
            {
                if (installerId == null)
                {
                    return StatusCode(400, new { IsSuccess = false, Message = "Invalid installer id", ErrorItems = new string[] { } });
                }
                else if (installationIds == null || installationIds.Count() == 0)
                {
                    return StatusCode(400, new { IsSuccess = false, Message = "At least one installation id is required", ErrorItems = new string[] { } });
                }
                else
                {
                    await installationService.AssignInstallations(installerId.Value, installationIds);
                    return Ok(new { IsSuccess = true, Message = "Installations assigned successfully", ErrorItems = new string[] { } });
                }
            }
            catch (AppException ex)
            {
                return StatusCode(400, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "An error was encountered while assigning installations");

                return StatusCode(500, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ReassignInstallation(long? installationId, long? installerId)
        {
            try
            {
                if (installationId == null || installerId == null)
                {
                    return StatusCode(400, new { IsSuccess = false, Message = "Invalid installation/installer id", ErrorItems = new string[] { } });
                }
                else
                {
                    await installationService.ReassignInstallation(installerId.Value, installationId.Value);
                    return Ok(new { IsSuccess = true, Message = "Installation reassigned successfully", ErrorItems = new string[] { } });
                }
            }
            catch (AppException ex)
            {
                return StatusCode(400, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "An error was encountered while reassigning installation");

                return StatusCode(500, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ReassignInstallations(IEnumerable<long> installationIds, long? installerId)
        {
            try
            {
                if (installerId == null)
                {
                    return StatusCode(400, new { IsSuccess = false, Message = "Invalid installer id", ErrorItems = new string[] { } });
                }
                else if (installationIds == null || installationIds.Count() == 0)
                {
                    return StatusCode(400, new { IsSuccess = false, Message = "At least one installation id is required", ErrorItems = new string[] { } });
                }
                else
                {
                    await installationService.ReassignInstallations(installerId.Value, installationIds);
                    return Ok(new { IsSuccess = true, Message = "Installations reassigned successfully", ErrorItems = new string[] { } });
                }
            }
            catch (AppException ex)
            {
                return StatusCode(400, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "An error was encountered while reassigning installations");

                return StatusCode(500, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
        }



        public IActionResult MyInstallations()
        {
            return View();
        }

        [HttpPost("[controller]/PendingDataTable/{installerId}")]
        public IActionResult PendingDataTable(long installerId)
        {
            var clientTimeOffset = string.IsNullOrEmpty(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]) ?
                appSettingsDelegate.Value.DefaultTimeZoneOffset : Convert.ToInt32(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]);

            var customers = installationService.GetPendingInstaller(installerId).Select(c => CustomerVM.FromCustomer(c.Customer, clientTimeOffset));

            var parser = new Parser<CustomerVM>(Request.Form, customers.AsQueryable())
                   .SetConverter(x => x.CreatedDate, x => x.CreatedDate.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.DateShared, x => x.DateShared == null ? "" : x.DateShared.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.ScheduleDate, x => x.ScheduleDate == null ? "" : x.ScheduleDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                     .SetConverter(x => x.IScheduleDate, x => x.IScheduleDate == null ? "" : x.IScheduleDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.SurveyDate, x => x.SurveyDate == null ? "" : x.SurveyDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"));

            return Ok(parser.Parse());
        }

        [HttpPost("[controller]/RejectedDataTable/{installerId}")]
        public IActionResult InstallerRejectedDataTable(long installerId)
        {
            var clientTimeOffset = string.IsNullOrEmpty(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]) ?
                appSettingsDelegate.Value.DefaultTimeZoneOffset : Convert.ToInt32(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]);

            var customers = installationService.GetRejetcedByInstaller(installerId).Select(c => CustomerVM.FromCustomer(c.Customer, clientTimeOffset));

            var parser = new Parser<CustomerVM>(Request.Form, customers.AsQueryable())
                   .SetConverter(x => x.CreatedDate, x => x.CreatedDate.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.DateShared, x => x.DateShared == null ? "" : x.DateShared.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.ScheduleDate, x => x.ScheduleDate == null ? "" : x.ScheduleDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                     .SetConverter(x => x.IScheduleDate, x => x.IScheduleDate == null ? "" : x.IScheduleDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.SurveyDate, x => x.SurveyDate == null ? "" : x.SurveyDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"));

            return Ok(parser.Parse());
        }

        [HttpPost("[controller]/CompletedDataTable/{installerId}")]
        public IActionResult InstallerCompletedDataTable(long installerId)
        {
            var clientTimeOffset = string.IsNullOrEmpty(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]) ?
                appSettingsDelegate.Value.DefaultTimeZoneOffset : Convert.ToInt32(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]);

            var customers = installationService.GetCompletedByInstaller(installerId).Select(c => CustomerVM.FromCustomer(c.Customer, clientTimeOffset));

            var parser = new Parser<CustomerVM>(Request.Form, customers.AsQueryable())
                   .SetConverter(x => x.CreatedDate, x => x.CreatedDate.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.DateShared, x => x.DateShared == null ? "" : x.DateShared.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.ScheduleDate, x => x.ScheduleDate == null ? "" : x.ScheduleDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                     .SetConverter(x => x.IScheduleDate, x => x.IScheduleDate == null ? "" : x.IScheduleDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.SurveyDate, x => x.SurveyDate == null ? "" : x.SurveyDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"));

            return Ok(parser.Parse());
        }

        [HttpPost("[controller]/DiscoRejectedDataTable/{installerId}")]
        public IActionResult DiscoRejectedDataTable(long installerId)
        {
            var clientTimeOffset = string.IsNullOrEmpty(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]) ?
                appSettingsDelegate.Value.DefaultTimeZoneOffset : Convert.ToInt32(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]);

            var customers = installationService.GetDiscoRejected(installerId).Select(c => CustomerVM.FromCustomer(c.Customer, clientTimeOffset));

            var parser = new Parser<CustomerVM>(Request.Form, customers.AsQueryable())
                   .SetConverter(x => x.CreatedDate, x => x.CreatedDate.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.DateShared, x => x.DateShared == null ? "" : x.DateShared.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.ScheduleDate, x => x.ScheduleDate == null ? "" : x.ScheduleDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                     .SetConverter(x => x.IScheduleDate, x => x.IScheduleDate == null ? "" : x.IScheduleDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.SurveyDate, x => x.SurveyDate == null ? "" : x.SurveyDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"));

            return Ok(parser.Parse());
        }

        [HttpPost("[controller]/DiscoApprovedDataTable/{installerId}")]
        public IActionResult DiscoApprovedDataTable(long installerId)
        {
            var clientTimeOffset = string.IsNullOrEmpty(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]) ?
                appSettingsDelegate.Value.DefaultTimeZoneOffset : Convert.ToInt32(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]);

            var customers = installationService.GetDiscoApproved(installerId).Select(c => CustomerVM.FromCustomer(c.Customer, clientTimeOffset));

            var parser = new Parser<CustomerVM>(Request.Form, customers.AsQueryable())
                   .SetConverter(x => x.CreatedDate, x => x.CreatedDate.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.DateShared, x => x.DateShared == null ? "" : x.DateShared.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.ScheduleDate, x => x.ScheduleDate == null ? "" : x.ScheduleDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                     .SetConverter(x => x.IScheduleDate, x => x.IScheduleDate == null ? "" : x.IScheduleDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.SurveyDate, x => x.SurveyDate == null ? "" : x.SurveyDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"));

            return Ok(parser.Parse());
        }

        [HttpPost]
        public async Task<IActionResult> ScheduleInstallation(long? installationId, DateTimeOffset? scheduleDate)
        {
            try
            {
                if (installationId == null || scheduleDate == null)
                {
                    return StatusCode(400, new { IsSuccess = false, Message = "Invalid installation id/schedule date", ErrorItems = new string[] { } });
                }
                else
                {
                    await installationService.ScheduleInstallation(installationId.Value, scheduleDate.Value);
                    return Ok(new { IsSuccess = true, Message = "Installation scheduled successfully", ErrorItems = new string[] { } });
                }
            }
            catch (AppException ex)
            {
                return StatusCode(400, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "An error was encountered while scheduling installation");

                return StatusCode(500, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ScheduleInstallations(IEnumerable<long> installationIds, DateTimeOffset? scheduleDate)
        {
            try
            {
                if (installationIds == null || installationIds.Count() == 0)
                {
                    return StatusCode(400, new { IsSuccess = false, Message = "Installation ids are required", ErrorItems = new string[] { } });
                }
                else if (scheduleDate == null)
                {
                    return StatusCode(400, new { IsSuccess = false, Message = "Invalid schedule date", ErrorItems = new string[] { } });
                }
                else
                {
                    await installationService.ScheduleInstallations(installationIds, scheduleDate.Value);
                    return Ok(new { IsSuccess = true, Message = "Installations scheduled successfully", ErrorItems = new string[] { } });
                }
            }
            catch (AppException ex)
            {
                return StatusCode(400, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "An error was encountered while scheduling installations");

                return StatusCode(500, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
        }

        [HttpPost]
        public async Task<IActionResult> StartInstallation(long? installationId, string meterType, string meterNumber)
        {
            try
            {
                if (installationId == null)
                {
                    return StatusCode(400, new { IsSuccess = false, Message = "Invalid installation id", ErrorItems = new string[] { } });
                }
                else if (meterType == null || meterNumber == null)
                {
                    return StatusCode(400, new { IsSuccess = false, Message = "Meter type/meter number is required", ErrorItems = new string[] { } });
                }
                else
                {
                    await installationService.UpdateMeterInfo(installationId.Value, meterType, meterNumber);
                    return Ok(new { IsSuccess = true, Message = "Meter info saved successfully", ErrorItems = new string[] { } });
                }
            }
            catch (AppException ex)
            {
                return StatusCode(400, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "An error was encountered while saving meter info");

                return StatusCode(500, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
        }


        [HttpGet("[controller]/{id}/install")]
        public async Task<IActionResult> Install(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var installation = await installationService.GetInstallation(id.Value);
            if (installation == null)
            {
                return NotFound();
            }
            if(installation.InstallationStatusId!=(long)InstallationStatuses.Scheduled_for_Installation && 
                installation.InstallationStatusId != (long)InstallationStatuses.Installation_In_Progress &&
                installation.InstallationStatusId != (long)InstallationStatuses.Disco_Confirmation_Failed)
            {
                return RedirectToAction("MyInstallations");
            }

            return View(InstallationVM.FromInstallation(installation));
        }

        [HttpGet("[controller]/{id}/images")]
        public async Task<IActionResult> GetImages(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var installation = await installationService.GetInstallation(id.Value);
            if (installation == null)
            {
                return NotFound();
            }
            var images = GetInstallationImages(InstallationVM.FromInstallation(installation));
            return Ok(new { IsSuccess = true, Message = "Images retrieved successfully", Data=images, ErrorItems = new string[] { } });
        }

        [HttpPost("[controller]/UploadImage")]
        public async Task<IActionResult> UploadImage(long? installationId, IFormFile file, string fieldName)
        {
            try
            {
                if (installationId == null)
                {
                    return StatusCode(400, new { IsSuccess = false, Message = "Installation id is required", ErrorItems = new string[] { } });
                }
                else if (file == null)
                {
                    return StatusCode(400, new { IsSuccess = false, Message = "Image is required", ErrorItems = new string[] { } });
                }
                else if (fieldName == null)
                {
                    return StatusCode(400, new { IsSuccess = false, Message = "Field name is required", ErrorItems = new string[] { } });
                }
                else
                {
                    await installationService.UploadImage(installationId.Value, fieldName, file);
                    return Ok(new { IsSuccess = true, Message = "Image uploaded successfully", ErrorItems = new string[] { } });
                }
            }
            catch (AppException ex)
            {
                return StatusCode(400, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "An error was encountered while uploading image");

                return StatusCode(500, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
        }

        [HttpGet("[controller]/{installationId}/DeleteImage")]
        public async Task<IActionResult> DeleteImage(long? installationId, string fieldName)
        {
            try
            {
                if (installationId == null)
                {
                    return StatusCode(400, new { IsSuccess = false, Message = "Installation id is required", ErrorItems = new string[] { } });
                }else if (fieldName == null)
                {
                    return StatusCode(400, new { IsSuccess = false, Message = "Field name is required", ErrorItems = new string[] { } });
                }
                else
                {
                    await installationService.DeleteImage(installationId.Value, fieldName);
                    return Ok(new { IsSuccess = true, Message = "Image deleted successfully", ErrorItems = new string[] { } });
                }
            }
            catch (AppException ex)
            {
                return StatusCode(400, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "An error was encountered while deleting image");

                return StatusCode(500, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
        }

        [HttpPost("[controller]/CompleteInstallation")]
        public async Task<IActionResult> CompleteInstallation(long? installationId, string comment=null)
        {
            try
            {
                await installationService.CompleteInstallation(installationId.Value, comment);
                return Ok(new { IsSuccess = true, Message = "Installatiton completed successfully", ErrorItems = new string[] { } });
            }
            catch (AppException ex)
            {
                return StatusCode(400, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "An error was encountered while completing installation");

                return StatusCode(500, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
        }

        [HttpPost("[controller]/RejectInstallation")]
        public async Task<IActionResult> RejectInstallation(long? installationId, string comment = null)
        {
            try
            {
                await installationService.RejectInstallation(installationId.Value, comment);
                return Ok(new { IsSuccess = true, Message = "Installatiton rejected successfully", ErrorItems = new string[] { } });
            }
            catch (AppException ex)
            {
                return StatusCode(400, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "An error was encountered while rejecting installation");

                return StatusCode(500, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
        }

        [NonAction]
        private IEnumerable<ImageVM> GetInstallationImages(InstallationVM installation)
        {
            return new List<ImageVM>()
            {
                new ImageVM
                {
                    Id=1,
                    InstallationId=installation.Id,
                    Path=installation.LocationFrontViewImagePath,
                    Name="LocationFrontViewImagePath",
                    Caption="Front-view image of installation location"
                },
                new ImageVM
                {
                    Id=2,
                    InstallationId=installation.Id,
                    Path=installation.MeterPointBeforeInstallationImagePath,
                    Name="MeterPointBeforeInstallationImagePath",
                    Caption="Image of proposed meter point before installation"
                },
                 new ImageVM
                {
                     Id=3,
                    InstallationId=installation.Id,
                    Path=installation.CustomerBillImagePath,
                    Name="CustomerBillImagePath",
                    Caption="Image of customer bill"
                },
                 new ImageVM
                {
                     Id=4,
                    InstallationId=installation.Id,
                    Path=installation.MeterNamePlateImagePath,
                    Name="MeterNamePlateImagePath",
                    Caption="Image of meter name plate"
                },
                  new ImageVM
                {
                      Id=5,
                    InstallationId=installation.Id,
                    Path=installation.InProgessImagePath,
                    Name="InProgessImagePath",
                    Caption="Image showing installation in progress"
                },
                 new ImageVM
                {
                     Id=6,
                    InstallationId=installation.Id,
                    Path=installation.MeterSealImagePath,
                    Name="MeterSealImagePath",
                    Caption="Image showing meter seal"
                },
                 new ImageVM
                {
                     Id=7,
                    InstallationId=installation.Id,
                    Path=installation.CompleteWithSealImagePath,
                    Name="CompleteWithSealImagePath",
                    Caption="Image showing complete installation with meter seal"
                },
                 new ImageVM
                {
                     Id=8,
                    InstallationId=installation.Id,
                    Path=installation.MeterCardImagePath,
                    Name="MeterCardImagePath",
                    Caption="Image of meter card"
                },
                 new ImageVM
                {
                     Id=9,
                    InstallationId=installation.Id,
                    Path=installation.SupplyCableVisibleToMeterImagePath,
                    Name="SupplyCableVisibleToMeterImagePath",
                    Caption="Image showing supply cable visible to the installed meter"
                },
                new ImageVM
                {
                    Id=10,
                    InstallationId=installation.Id,
                    Path=installation.SupplyCableToThePremisesImagePath,
                    Name="SupplyCableToThePremisesImagePath",
                    Caption="Image showing supply cable to the premisses from the installed meter point"
                },
                new ImageVM
                {
                    Id=11,
                    InstallationId=installation.Id,
                    Path=installation.RetrievedMeterImagePath,
                    Name="RetrievedMeterImagePath",
                    Caption="Image of retrieved meter (Where applicable)",
                    Required=false
                },
                new ImageVM
                {
                    Id=12,
                    InstallationId=installation.Id,
                    Path=installation.MonitorShowingRemainingUnitImagePath,
                    Name="MonitorShowingRemainingUnitImagePath",
                    Caption="Image of monitor/UIU showing remaining unit of the retrieved meter (Where applicable)",
                    Required=false
                }
            };
        }




        public IActionResult DiscoInstallations()
        {
            return View();
        }

        [HttpGet("[controller]/{id}/DiscoReview")]
        public async Task<IActionResult> DiscoReview(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var installation = await installationService.GetInstallation(id.Value);
            if (installation == null)
            {
                return NotFound();
            }
            if (installation.InstallationStatusId != (long)InstallationStatuses.Installation_Completed)
            {
                return RedirectToAction("DiscoInstallations");
            }

            return View(InstallationVM.FromInstallation(installation));
        }

        [HttpPost("[controller]/DiscoApproveInstallation")]
        public async Task<IActionResult> DiscoApproveInstallation(long? installationId, string comment = null)
        {
            try
            {
                await installationService.DiscoApproveInstallation(installationId.Value, comment);
                return Ok(new { IsSuccess = true, Message = "Installatiton approved successfully", ErrorItems = new string[] { } });
            }
            catch (AppException ex)
            {
                return StatusCode(400, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "An error was encountered while disco approving installation");

                return StatusCode(500, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
        }

        [HttpPost("[controller]/DiscoRejectInstallation")]
        public async Task<IActionResult> DiscoRejectInstallation(long? installationId, string comment = null)
        {
            try
            {
                await installationService.DiscoRejectInstallation(installationId.Value, comment);
                return Ok(new { IsSuccess = true, Message = "Installatiton rejected successfully", ErrorItems = new string[] { } });
            }
            catch (AppException ex)
            {
                return StatusCode(400, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "An error was encountered while disco rejecting installation");

                return StatusCode(500, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
        }



        [HttpGet("[controller]/{id}")]
        public async Task<IActionResult> GetInstallation(long? id)
        {
            try
            {
                if (id == null)
                {
                    return StatusCode(400, new { IsSuccess = false, Message = "Invalid installation id", ErrorItems = new string[] { } });
                }
                else
                {
                    var installation = await installationService.GetInstallation(id.Value);
                    return Ok(new { IsSuccess = true, Message = "Installation retrieved successfully", Data = InstallationVM.FromInstallation(installation), ErrorItems = new string[] { } });
                }
            }
            catch (AppException ex)
            {
                return StatusCode(400, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "An error was encountered while fetching survey");

                return StatusCode(500, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
        }

        [HttpGet("[controller]/{id}/Details")]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var installation = await installationService.GetInstallation(id.Value);
            if (installation == null)
            {
                return NotFound();
            }
            //if (installation.InstallationStatusId == (long)InstallationStatuses.Pending)
            //{
            //    return NotFound();
            //}

            return View(InstallationVM.FromInstallation(installation));
        }
    }
}
