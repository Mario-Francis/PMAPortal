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
    public class FeedbacksController : Controller
    {
        private readonly IFeedbackService feedbackService;
        private readonly IOptionsSnapshot<AppSettings> appSettingsDelegate;
        private readonly ITokenService tokenService;
        private readonly IInstallationService installationService;
        private readonly ICustomerService customerService;
        private readonly ILoggerService<FeedbacksController> logger;

        public FeedbacksController(IFeedbackService feedbackService,
             IOptionsSnapshot<AppSettings> appSettingsDelegate,
             ITokenService tokenService,
             IInstallationService installationService,
             ICustomerService customerService,
             ILoggerService<FeedbacksController> logger)
        {
            this.feedbackService = feedbackService;
            this.appSettingsDelegate = appSettingsDelegate;
            this.tokenService = tokenService;
            this.installationService = installationService;
            this.customerService = customerService;
            this.logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("[controller]/{id}")]
        public async Task<IActionResult> FeedbackDetails(long id)
        {
            var feedback = await feedbackService.GetFeedback(id);
            if (feedback == null)
            {
                return NotFound();
            }

            return View(feedback);
        }
        [HttpPost]
        public IActionResult FeedbacksDataTable()
        {
            var clientTimeOffset = string.IsNullOrEmpty(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]) ?
                appSettingsDelegate.Value.DefaultTimeZoneOffset : Convert.ToInt32(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]);


            var feedbacks = feedbackService.GetFeedbacks().Select(f => FeedbackVM.FromCustomerFeedback(f, clientTimeOffset));

            var parser = new Parser<FeedbackVM>(Request.Form, feedbacks.AsQueryable())
                   .SetConverter(x => x.CreatedDate, x => x.CreatedDate.ToString("MMM d, yyyy 'at' hh:mmtt"));

            return Ok(parser.Parse());
        }

       


        [HttpGet("[controller]/Feedback/{token}")]
        public async Task<IActionResult> Feedback(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    return NotFound();
                }

                var installationId = Convert.ToInt64(tokenService.ExtractDataFromToken(token));
                var installation = await installationService.GetInstallation(installationId);
                var questions = feedbackService.GetQuestions();

                if (installation.CustomerFeedbacks.Count() > 0)
                {
                    return RedirectToAction("FeedbackReceived");
                }

                ViewData["MeterNumber"] = installation.MeterNumber;
                ViewData["CustomerId"] = installation.CustomerId;
                ViewData["InstallationId"] = installationId;

                return View(questions);
            }
            catch (Exception ex)
            {
                logger.LogException(ex);
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Feedback(FeedbackVM model)
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
                    await feedbackService.AddFeedback(model.ToCustomerFeedback());
                    return Ok(new
                    {
                        IsSuccess = true,
                        Message = "Feedback added succeessfully",
                        ErrorItems = new string[] { }
                    });
                }
            }
            catch (AppException ex)
            {
                return StatusCode(400, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "An error was encountered while adding a new feedback");
                return StatusCode(500, new { IsSuccess = false, Message = ex.Message, ErrorDetail = ex.GetErrorDetails() });
            }
        }

        public IActionResult FeedbackReceived()
        {
            return View();
        }
    }
}
