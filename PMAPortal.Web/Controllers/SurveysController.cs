using DataTablesParser;
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
    public class SurveysController : Controller
    {
        private readonly IOptionsSnapshot<AppSettings> appSettingsDelegate;
        private readonly ICustomerService customerService;
        private readonly ISurveyService surveyService;
        private readonly ILoggerService<SurveysController> logger;

        public SurveysController(
            IOptionsSnapshot<AppSettings> appSettingsDelegate,
            ICustomerService customerService,
            ISurveyService surveyService,
            ILoggerService<SurveysController> logger)
        {
            this.appSettingsDelegate = appSettingsDelegate;
            this.customerService = customerService;
            this.surveyService = surveyService;
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

            var customers = surveyService.GetUnassigned().Select(c => CustomerVM.FromCustomer(c, clientTimeOffset));

            var parser = new Parser<CustomerVM>(Request.Form, customers.AsQueryable())
                   .SetConverter(x => x.CreatedDate, x => x.CreatedDate.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.DateShared, x => x.DateShared == null ? "" : x.DateShared.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.ScheduleDate, x => x.ScheduleDate == null ? "" : x.ScheduleDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.SurveyDate, x => x.SurveyDate == null ? "" : x.SurveyDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"));

            return Ok(parser.Parse());
        }

        [HttpPost]
        public IActionResult AssignedDataTable()
        {
            var clientTimeOffset = string.IsNullOrEmpty(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]) ?
                appSettingsDelegate.Value.DefaultTimeZoneOffset : Convert.ToInt32(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]);

            var customers = surveyService.GetAssigned().Select(c => CustomerVM.FromCustomer(c, clientTimeOffset));

            var parser = new Parser<CustomerVM>(Request.Form, customers.AsQueryable())
                   .SetConverter(x => x.CreatedDate, x => x.CreatedDate.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.DateShared, x => x.DateShared == null ? "" : x.DateShared.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.ScheduleDate, x => x.ScheduleDate == null ? "" : x.ScheduleDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.SurveyDate, x => x.SurveyDate == null ? "" : x.SurveyDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"));

            return Ok(parser.Parse());
        }
        [HttpPost("[controller]/AssignedDataTable/{surveyStaffId}")]
        public IActionResult AssignedDataTable(long surveyStaffId)
        {
            var clientTimeOffset = string.IsNullOrEmpty(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]) ?
                appSettingsDelegate.Value.DefaultTimeZoneOffset : Convert.ToInt32(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]);

            var customers = surveyService.GetAssigned(surveyStaffId).Select(c => CustomerVM.FromCustomer(c.Customer, clientTimeOffset));

            var parser = new Parser<CustomerVM>(Request.Form, customers.AsQueryable())
                   .SetConverter(x => x.CreatedDate, x => x.CreatedDate.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.DateShared, x => x.DateShared == null ? "" : x.DateShared.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.ScheduleDate, x => x.ScheduleDate == null ? "" : x.ScheduleDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.SurveyDate, x => x.SurveyDate == null ? "" : x.SurveyDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"));

            return Ok(parser.Parse());
        }

        [HttpPost]
        public IActionResult CompletedDataTable()
        {
            var clientTimeOffset = string.IsNullOrEmpty(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]) ?
                appSettingsDelegate.Value.DefaultTimeZoneOffset : Convert.ToInt32(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]);

            var customers = surveyService.GetCompleted().Select(c => CustomerVM.FromCustomer(c.Customer, clientTimeOffset));

            var parser = new Parser<CustomerVM>(Request.Form, customers.AsQueryable())
                   .SetConverter(x => x.CreatedDate, x => x.CreatedDate.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.DateShared, x => x.DateShared == null ? "" : x.DateShared.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.ScheduleDate, x => x.ScheduleDate == null ? "" : x.ScheduleDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.SurveyDate, x => x.SurveyDate == null ? "" : x.SurveyDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"));

            return Ok(parser.Parse());
        }

        [HttpPost("[controller]/CompletedDataTable/{surveyStaffId}")]
        public IActionResult CompletedDataTable(long surveyStaffId)
        {
            var clientTimeOffset = string.IsNullOrEmpty(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]) ?
                appSettingsDelegate.Value.DefaultTimeZoneOffset : Convert.ToInt32(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]);

            var customers = surveyService.GetCompleted(surveyStaffId).Select(c => CustomerVM.FromCustomer(c.Customer, clientTimeOffset));

            var parser = new Parser<CustomerVM>(Request.Form, customers.AsQueryable())
                   .SetConverter(x => x.CreatedDate, x => x.CreatedDate.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.DateShared, x => x.DateShared == null ? "" : x.DateShared.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.ScheduleDate, x => x.ScheduleDate == null ? "" : x.ScheduleDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.SurveyDate, x => x.SurveyDate == null ? "" : x.SurveyDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"));

            return Ok(parser.Parse());
        }

        [HttpPost]
        public async Task<IActionResult> AssignSurvey(long? customerId, long? surveyStaffId)
        {
            try
            {
                if (customerId == null || surveyStaffId==null)
                {
                    return StatusCode(400, new { IsSuccess = false, Message = "Invalid customer/survey staff id", ErrorItems = new string[] { } });
                }
                else
                {
                    await surveyService.AssignSurvey(surveyStaffId.Value, customerId.Value);
                    return Ok(new { IsSuccess = true, Message = "Survey assigned successfully", ErrorItems = new string[] { } });
                }
            }
            catch (AppException ex)
            {
                return StatusCode(400, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "An error was encountered while assigning survey");

                return StatusCode(500, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AssignSurveys(IEnumerable<long> customerIds, long? surveyStaffId)
        {
            try
            {
                if (surveyStaffId == null)
                {
                    return StatusCode(400, new { IsSuccess = false, Message = "Invalid survey staff id", ErrorItems = new string[] { } });
                }
                else if(customerIds==null || customerIds.Count() == 0)
                {
                    return StatusCode(400, new { IsSuccess = false, Message = "At least one customer id is required", ErrorItems = new string[] { } });
                }
                else
                {
                    await surveyService.AssignSurveys(surveyStaffId.Value, customerIds);
                    return Ok(new { IsSuccess = true, Message = "Surveys assigned successfully", ErrorItems = new string[] { } });
                }
            }
            catch (AppException ex)
            {
                return StatusCode(400, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "An error was encountered while assigning surveys");

                return StatusCode(500, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ReassignSurvey(long? surveyId, long? surveyStaffId)
        {
            try
            {
                if (surveyId == null || surveyStaffId == null)
                {
                    return StatusCode(400, new { IsSuccess = false, Message = "Invalid customer/survey staff id", ErrorItems = new string[] { } });
                }
                else
                {
                    await surveyService.ReassignSurvey(surveyStaffId.Value, surveyId.Value);
                    return Ok(new { IsSuccess = true, Message = "Survey reassigned successfully", ErrorItems = new string[] { } });
                }
            }
            catch (AppException ex)
            {
                return StatusCode(400, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "An error was encountered while reassigning survey");

                return StatusCode(500, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ReassignSurveys(IEnumerable<long> surveyIds, long? surveyStaffId)
        {
            try
            {
                if (surveyStaffId == null)
                {
                    return StatusCode(400, new { IsSuccess = false, Message = "Invalid survey staff id", ErrorItems = new string[] { } });
                }
                else if (surveyIds == null || surveyIds.Count() == 0)
                {
                    return StatusCode(400, new { IsSuccess = false, Message = "At least one customer id is required", ErrorItems = new string[] { } });
                }
                else
                {
                    await surveyService.ReassignSurveys(surveyStaffId.Value, surveyIds);
                    return Ok(new { IsSuccess = true, Message = "Surveys reassigned successfully", ErrorItems = new string[] { } });
                }
            }
            catch (AppException ex)
            {
                return StatusCode(400, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "An error was encountered while reassigning surveys");

                return StatusCode(500, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ScheduleSurvey(long? surveyId, DateTimeOffset? scheduleDate)
        {
            try
            {
                if (surveyId == null || scheduleDate==null)
                {
                    return StatusCode(400, new { IsSuccess = false, Message = "Invalid survey id/schedule date", ErrorItems = new string[] { } });
                }
                else
                {
                    await surveyService.ScheduleSurvey(surveyId.Value, scheduleDate.Value);
                    return Ok(new { IsSuccess = true, Message = "Survey scheduled successfully", ErrorItems = new string[] { } });
                }
            }
            catch (AppException ex)
            {
                return StatusCode(400, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "An error was encountered while scheduling survey");

                return StatusCode(500, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ScheduleSurveys(IEnumerable<long> surveyIds, DateTimeOffset? scheduleDate)
        {
            try
            {
                if(surveyIds==null || surveyIds.Count() == 0)
                {
                    return StatusCode(400, new { IsSuccess = false, Message = "Survey ids are required", ErrorItems = new string[] { } });
                }
                else if (scheduleDate == null)
                {
                    return StatusCode(400, new { IsSuccess = false, Message = "Invalid schedule date", ErrorItems = new string[] { } });
                }
                else
                {
                    await surveyService.ScheduleSurveys(surveyIds, scheduleDate.Value);
                    return Ok(new { IsSuccess = true, Message = "Surveys scheduled successfully", ErrorItems = new string[] { } });
                }
            }
            catch (AppException ex)
            {
                return StatusCode(400, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "An error was encountered while scheduling surveys");

                return StatusCode(500, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRemark(long? surveyId, string remark)
        {
            try
            {
                if (surveyId == null || remark == null)
                {
                    return StatusCode(400, new { IsSuccess = false, Message = "Invalid survey id/remark", ErrorItems = new string[] { } });
                }
                else
                {
                    await surveyService.UpdateRemark(surveyId.Value, remark);
                    return Ok(new { IsSuccess = true, Message = "Survey remark updated successfully", ErrorItems = new string[] { } });
                }
            }
            catch (AppException ex)
            {
                return StatusCode(400, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "An error was encountered while survey remark");

                return StatusCode(500, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSurvey(SurveyVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errs = ModelState.Values.Where(v => v.Errors.Count > 0).Select(v => v.Errors.First().ErrorMessage);
                    return StatusCode(400, new { IsSuccess = false, Message = "One or more fields failed validation", ErrorItems = errs });
                }
                else
                {
                    await surveyService.UpdateSurvey(model.ToSurvey());
                    return Ok(new { IsSuccess = true, Message = "Survey updated succeessfully", ErrorItems = new string[] { } });
                }
            }
            catch (AppException ex)
            {
                return StatusCode(400, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "An error was encountered while creating a new area");

                return StatusCode(500, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CompleteSurvey(SurveyVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errs = ModelState.Values.Where(v => v.Errors.Count > 0).Select(v => v.Errors.First().ErrorMessage);
                    return StatusCode(400, new { IsSuccess = false, Message = "One or more fields failed validation", ErrorItems = errs });
                }
                else
                {
                    await surveyService.CompleteSurvey(model.ToSurvey());
                    return Ok(new { IsSuccess = true, Message = "Survey completed succeessfully", ErrorItems = new string[] { } });
                }
            }
            catch (AppException ex)
            {
                return StatusCode(400, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "An error was encountered while submitting survey");

                return StatusCode(500, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
        }

        [HttpGet("[controller]/{id}")]
        public async Task<IActionResult> GetSurvey(long? id)
        {
            try
            {
                if (id==null)
                {
                    return StatusCode(400, new { IsSuccess = false, Message = "Invalid survey id", ErrorItems = new string[] { } });
                }
                else
                {
                    var survey = await surveyService.GetSurvey(id.Value);
                    return Ok(new { IsSuccess = true, Message = "Survey retrieved successfully", Data=SurveyVM.FromSurvey(survey), ErrorItems = new string[] { } });
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
    }
}
