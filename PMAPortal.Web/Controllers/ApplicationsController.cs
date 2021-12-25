using DataTablesParser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PMAPortal.Web.DTOs;
using PMAPortal.Web.Extensions;
using PMAPortal.Web.Models;
using PMAPortal.Web.Services;
using PMAPortal.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PMAPortal.Web.Controllers
{
    [AllowAnonymous]
    public class ApplicationsController : Controller
    {
        private readonly ILoggerService<ApplicationsController> logger;
        private readonly IApplicationService applicationService;
        private readonly IMeterService meterService;
        private readonly IPaymentService paymentService;
        private readonly IOptionsSnapshot<AppSettings> appSettingsDelegate;
        private readonly IUserService userService;

        public ApplicationsController(ILoggerService<ApplicationsController> logger,
            IApplicationService applicationService,
            IMeterService meterService,
            IPaymentService paymentService,
            IOptionsSnapshot<AppSettings> appSettingsDelegate,
            IUserService userService)
        {
            this.logger = logger;
            this.applicationService = applicationService;
            this.meterService = meterService;
            this.paymentService = paymentService;
            this.appSettingsDelegate = appSettingsDelegate;
            this.userService = userService;
        }
       
        public IActionResult Index()
        {
            if (User.IsInRole(Constants.ROLE_ADMIN))
            {
                return View();
            }
            else if (User.IsInRole(Constants.ROLE_INSTALLER))
            {
                return View("InstallerApplications");
            }
            else if (User.IsInRole(Constants.ROLE_DISCO))
            {
                return View("DiscoApplications");
            }
            else
            {
                return NotFound();
            }
        }


        [HttpPost]
        public IActionResult AdminApplicationsDataTable(string tableType)
        {
            var clientTimeOffset = string.IsNullOrEmpty(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]) ?
                appSettingsDelegate.Value.DefaultTimeZoneOffset : Convert.ToInt32(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]);

            long[] statuses=null;
            if (tableType == "all")
            {
                statuses = new long[] {
                    (long)ApplicationsStatus.Submitted,
                    (long)ApplicationsStatus.Scheduled_for_Installation,
                    (long)ApplicationsStatus.Installation_In_Progress,
                    (long)ApplicationsStatus.Installation_Failed,
                    (long)ApplicationsStatus.Installation_Completed,
                    (long)ApplicationsStatus.Disco_Confirmation_Failed,
                    (long)ApplicationsStatus.Disco_Confirmation_Successful,
                };
            }else if (tableType == "installer")
            {
                statuses = new long[] {
                    (long)ApplicationsStatus.Submitted,
                    (long)ApplicationsStatus.Scheduled_for_Installation,
                    (long)ApplicationsStatus.Installation_In_Progress,
                    (long)ApplicationsStatus.Installation_Failed,
                    (long)ApplicationsStatus.Disco_Confirmation_Failed
                };
            }else if (tableType == "disco")
            {
                statuses = new long[] {
                    (long)ApplicationsStatus.Installation_Completed,
                    (long)ApplicationsStatus.Disco_Confirmation_Failed
                };
            }else if (tableType == "completed")
            {
                statuses = new long[] {
                    (long)ApplicationsStatus.Disco_Confirmation_Successful
                };
            }

            var applications = applicationService.GetApplications(statuses).Select(a => ApplicationItemVM.FromApplication(a, clientTimeOffset));
           
            var parser = new Parser<ApplicationItemVM>(Request.Form, applications.AsQueryable())
                  .SetConverter(x => x.UpdatedDate, x => x.UpdatedDate.ToString("MMM d, yyyy 'at' hh:mmtt"))
                   .SetConverter(x => x.CreatedDate, x => x.CreatedDate.ToString("MMM d, yyyy 'at' hh:mmtt"));

            return Ok(parser.Parse());
        }

        [HttpPost]
        public IActionResult InstallerApplicationsDataTable(string tableType)
        {
            var clientTimeOffset = string.IsNullOrEmpty(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]) ?
                appSettingsDelegate.Value.DefaultTimeZoneOffset : Convert.ToInt32(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]);

            long[] statuses = null;
            if (tableType == "pending")
            {
                statuses = new long[] {
                    (long)ApplicationsStatus.Submitted,
                    (long)ApplicationsStatus.Scheduled_for_Installation,
                    (long)ApplicationsStatus.Installation_In_Progress
                };
            }
            else if (tableType == "failed")
            {
                statuses = new long[] {
                    (long)ApplicationsStatus.Installation_Failed
                };
            }
            else if (tableType == "disco")
            {
                statuses = new long[] {
                    (long)ApplicationsStatus.Disco_Confirmation_Failed
                };
            }
            else if (tableType == "completed")
            {
                statuses = new long[] {
                    (long)ApplicationsStatus.Disco_Confirmation_Successful
                };
            }

            var applications = applicationService.GetApplications(statuses).Select(a => ApplicationItemVM.FromApplication(a, clientTimeOffset));

            var parser = new Parser<ApplicationItemVM>(Request.Form, applications.AsQueryable())
                  .SetConverter(x => x.UpdatedDate, x => x.UpdatedDate.ToString("MMM d, yyyy 'at' hh:mmtt"))
                   .SetConverter(x => x.CreatedDate, x => x.CreatedDate.ToString("MMM d, yyyy 'at' hh:mmtt"));

            return Ok(parser.Parse());
        }

        [HttpPost]
        public IActionResult DiscoApplicationsDataTable(string tableType)
        {
            var clientTimeOffset = string.IsNullOrEmpty(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]) ?
                appSettingsDelegate.Value.DefaultTimeZoneOffset : Convert.ToInt32(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]);

            long[] statuses = null;
            if (tableType == "pending")
            {
                statuses = new long[] {
                    (long)ApplicationsStatus.Installation_Completed
                };
            }
            else if (tableType == "failed")
            {
                statuses = new long[] {
                    (long)ApplicationsStatus.Disco_Confirmation_Failed
                };
            }
            else if (tableType == "completed")
            {
                statuses = new long[] {
                    (long)ApplicationsStatus.Disco_Confirmation_Successful
                };
            }

            var applications = applicationService.GetApplications(statuses).Select(a => ApplicationItemVM.FromApplication(a, clientTimeOffset));

            var parser = new Parser<ApplicationItemVM>(Request.Form, applications.AsQueryable())
                  .SetConverter(x => x.UpdatedDate, x => x.UpdatedDate.ToString("MMM d, yyyy 'at' hh:mmtt"))
                   .SetConverter(x => x.CreatedDate, x => x.CreatedDate.ToString("MMM d, yyyy 'at' hh:mmtt"));

            return Ok(parser.Parse());
        }


        [HttpGet("[controller]/{id}")]
        public async Task<IActionResult> ApplicationDetails(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var application = await applicationService.GetApplication(id.Value);
                application.UpdatedByUser = await userService.GetUser(application.UpdatedBy.GetValueOrDefault());

                var statusLogs = application.ApplicationStatusLogs.AsEnumerable();
                application.ApplicationStatusLogs = new List<ApplicationStatusLog>();
                foreach(var l in statusLogs)
                {
                    l.ActionByUser = await userService.GetUser(l.ActionBy.GetValueOrDefault());
                    application.ApplicationStatusLogs.Add(l);
                }

                var clientTimeOffset = string.IsNullOrEmpty(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]) ?
                appSettingsDelegate.Value.DefaultTimeZoneOffset : Convert.ToInt32(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]);
                if (application == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(ApplicationVM.FromApplication(application, clientTimeOffset));
                }
            }
        }

        [HttpGet("[controller]/Track/{trackNumber}")]
        public async Task<IActionResult> TrackApplication(string trackNumber)
        {
            if (string.IsNullOrEmpty(trackNumber))
            {
                return NotFound();
            }
            else
            {
                var application = await applicationService.GetApplication(trackNumber);
                application.UpdatedByUser = await userService.GetUser(application.UpdatedBy.GetValueOrDefault());

                var statusLogs = application.ApplicationStatusLogs.AsEnumerable();
                application.ApplicationStatusLogs = new List<ApplicationStatusLog>();
                foreach (var l in statusLogs)
                {
                    l.ActionByUser = await userService.GetUser(l.ActionBy.GetValueOrDefault());
                    application.ApplicationStatusLogs.Add(l);
                }

                var clientTimeOffset = string.IsNullOrEmpty(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]) ?
                appSettingsDelegate.Value.DefaultTimeZoneOffset : Convert.ToInt32(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]);
                if (application == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(ApplicationVM.FromApplication(application, clientTimeOffset));
                }
            }
        }

        [HttpGet("[controller]/ValidateTrackNumber/{trackNumber}")]
        public async Task<IActionResult> ValidateTrackNo(string trackNumber)
        {
            try
            {
                if (string.IsNullOrEmpty(trackNumber))
                {
                    throw new AppException("Track number id is required");
                }
                else
                {
                    var application = await applicationService.GetApplication(trackNumber);
                    if (application==null)
                    {
                        return Ok(new
                        {
                            IsSuccess = false,
                            Message = "Invalid",
                            ErrorItems = new string[] { }
                        });
                    }
                    else
                    {
                        return Ok(new
                        {
                            IsSuccess = true,
                            Message = "Valid",
                            ErrorItems = new string[] { }
                        });
                    }

                }
            }
            catch (AppException ex)
            {
                return StatusCode(400, new { IsSuccess = false, Message = ex.Message, ErrorDetail = JsonSerializer.Serialize(ex.InnerException) });
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "An error was encountered while validating track number");
                return StatusCode(500, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSatus(UpdateStatusVM model)
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
                    if ((model.StatusId == 5 || model.StatusId==7) && string.IsNullOrEmpty(model.Comment))
                    {
                        throw new AppException("Comment is required");
                    }

                    await applicationService.UpdateApplicationStatus(model.ApplicationId, model.StatusId, model.Comment);
                    return Ok(new
                    {
                        IsSuccess = true,
                        Message = "Status updated succeessfully",
                        ErrorItems = new string[] { }
                    });
                }
            }
            catch (AppException ex)
            {
                return StatusCode(400, new { IsSuccess = false, Message = ex.Message, ErrorDetail = JsonSerializer.Serialize(ex.InnerException) });
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "An error was encountered while application status");
                return StatusCode(500, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
        }



        //===============
        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(ApplicationVM model)
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
                    var result = await applicationService.AddApplication(
                        model.ToApplication(),
                        model.ToApplicant(), 
                        model.ToApplicantAddress(), 
                        model.ToApplicationAppliances(), 
                        model.ToApplicationPets());
                    var meter = await meterService.GetMeter(model.MeterId);
                    var paymentId = await paymentService.AddPayment(result.applicationId, meter.Amount);
                    return Ok(new { 
                        IsSuccess = true, 
                        Message = "Application added succeessfully",
                        Data=new { TrackNumber=result.trackNo, 
                            ApplicationId=result.applicationId, 
                            PaymentId=paymentId,
                            FormattedAmount=meter.Amount.Format(), 
                            Amount=meter.Amount.ToString("N")}, 
                        ErrorItems = new string[] { } });
                }
            }
            catch (AppException ex)
            {
                return StatusCode(400, new { IsSuccess = false, Message = ex.Message, ErrorDetail = JsonSerializer.Serialize(ex.InnerException) });
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "An error was encountered while creating a new application");
                return StatusCode(500, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
        }


        public async Task<IActionResult> VerifyPayment(long? paymentId, string paymentRef)
        {
            try
            {
                if (paymentId==null)
                {
                    throw new AppException("Payment id is required");
                }else if (string.IsNullOrEmpty(paymentRef))
                {
                    throw new AppException("Payment ref is required");
                }
                else
                {
                    var vResult = await paymentService.VerifyPayment(paymentId.Value, paymentRef);
                    if (vResult.isSuccess)
                    {
                        var payment = await paymentService.GetPayment(paymentId.Value);
                        await paymentService.UpdatePaidPayment(paymentId.Value, vResult.datePaid.Value);
                        await paymentService.LogPayment(paymentId.Value, paymentRef, true);
                        await applicationService.UpdateApplicationStatus(payment.ApplicationId, (int)ApplicationsStatus.Submitted);

                        // schedule mail for applicant and installers
                        await applicationService.ScheduleNewApplicationMail(payment.ApplicationId);

                        return Ok(new
                        {
                            IsSuccess = true,
                            Message = "Payment successful",
                            ErrorItems = new string[] { }
                        });
                    }
                    else
                    {
                        await paymentService.LogPayment(paymentId.Value, paymentRef, false);
                        return Ok(new
                        {
                            IsSuccess = false,
                            Message = "Payment not successful",
                            ErrorItems = new string[] { }
                        });
                    }
                    
                }
            }
            catch (AppException ex)
            {
                return StatusCode(400, new { IsSuccess = false, Message = ex.Message, ErrorDetail = JsonSerializer.Serialize(ex.InnerException) });
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "An error was encountered while verifying payment");
                return StatusCode(500, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
        }

        [HttpGet("[controller]/ApplicantExist/{email}")]
        public async Task<IActionResult> ApplicantExist(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    throw new AppException("Email is required");
                }
                else
                {
                    var result = await applicationService.ApplicantExists(email);
                    return Ok(new
                    {
                        IsSuccess = true,
                        Exists = result.exist,
                        Data=ApplicantVM.FromApplicant(result.applicant),
                        Message=result.exist?"Applicant data already exists":"Applicat data does not exist",
                        ErrorItems = new string[] { }
                    });
                }
            }
            catch (AppException ex)
            {
                return StatusCode(400, new { IsSuccess = false, Message = ex.Message, ErrorDetail = JsonSerializer.Serialize(ex.InnerException) });
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "An error was encountered while checking if an applicant exists");
                return StatusCode(500, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
        }


    }
}
