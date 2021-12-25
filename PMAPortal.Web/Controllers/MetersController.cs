using DataTablesParser;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PMAPortal.Web.DTOs;
using PMAPortal.Web.Services;
using PMAPortal.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PMAPortal.Web.Controllers
{
    public class MetersController : Controller
    {
        private readonly IOptionsSnapshot<AppSettings> appSettingsDelegate;
        private readonly IMeterService meterService;
        private readonly ILoggerService<MetersController> logger;

        public MetersController(
            IOptionsSnapshot<AppSettings> appSettingsDelegate,
            IMeterService meterService,
            ILoggerService<MetersController> logger)
        {
            this.appSettingsDelegate = appSettingsDelegate;
            this.meterService = meterService;
            this.logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult MetersDataTable()
        {
            var clientTimeOffset = string.IsNullOrEmpty(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]) ?
                appSettingsDelegate.Value.DefaultTimeZoneOffset : Convert.ToInt32(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]);

            var Meters = meterService.GetMeters().Select(s => MeterVM.FromMeter(s, clientTimeOffset));

            var parser = new Parser<MeterVM>(Request.Form, Meters.AsQueryable())
                  .SetConverter(x => x.UpdatedDate, x => x.UpdatedDate.ToString("MMM d, yyyy 'at' hh:mmtt"))
                   .SetConverter(x => x.CreatedDate, x => x.CreatedDate.ToString("MMM d, yyyy 'at' hh:mmtt"));

            return Ok(parser.Parse());
        }

        [HttpPost]
        public async Task<IActionResult> AddMeter(MeterVM MeterVM)
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
                    await meterService.CreateMeter(MeterVM.ToMeter());
                    return Ok(new { IsSuccess = true, Message = "Meter added succeessfully", ErrorItems = new string[] { } });
                }
            }
            catch (AppException ex)
            {
                return StatusCode(400, new { IsSuccess = false, Message = ex.Message, ErrorDetail = JsonSerializer.Serialize(ex.InnerException) });
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "An error was encountered while creating a new meter");

                return StatusCode(500, new { IsSuccess = false, Message = ex.Message, ErrorDetail = JsonSerializer.Serialize(ex.InnerException) });
            }
        }


        [HttpPost]
        public async Task<IActionResult> UpdateMeter(MeterVM MeterVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errs = ModelState.Values.Where(v => v.Errors.Count > 0).Select(v => v.Errors.First().ErrorMessage);
                    return StatusCode(400, new { IsSuccess = false, Message = "One or more fields failed validation", ErrorItems = errs });
                }
                else if (MeterVM.Id == 0)
                {
                    return StatusCode(400, new { IsSuccess = false, Message = $"Invalid meter id {MeterVM.Id}", ErrorItems = new string[] { } });
                }
                else
                {

                    await meterService.UpdateMeter(MeterVM.ToMeter());
                    return Ok(new { IsSuccess = true, Message = "Meter updated succeessfully", ErrorItems = new string[] { } });
                }
            }
            catch (AppException ex)
            {
                return StatusCode(400, new { IsSuccess = false, Message = ex.Message, ErrorDetail = JsonSerializer.Serialize(ex.InnerException) });
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "An error was encountered while updating a meter");

                return StatusCode(500, new { IsSuccess = false, Message = ex.Message, ErrorDetail = JsonSerializer.Serialize(ex.InnerException) });
            }
        }
       
        public async Task<IActionResult> DeleteMeter(long? id)
        {
            try
            {
                if (id == null)
                {
                    return StatusCode(400, new { IsSuccess = false, Message = "Meter is not found", ErrorItems = new string[] { } });
                }
                else
                {
                    await meterService.DeleteMeter(id.Value);
                    return Ok(new { IsSuccess = true, Message = "Meter deleted succeessfully", ErrorItems = new string[] { } });
                }
            }
            catch (AppException ex)
            {
                return StatusCode(400, new { IsSuccess = false, Message = ex.Message, ErrorDetail = JsonSerializer.Serialize(ex.InnerException) });
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "An error was encountered while deleting a meter");

                return StatusCode(500, new { IsSuccess = false, Message = ex.Message, ErrorDetail = JsonSerializer.Serialize(ex.InnerException) });
            }
        }

        public async Task<IActionResult> GetMeter(long? id)
        {
            try
            {
                if (id == null)
                {
                    return StatusCode(400, new { IsSuccess = false, Message = "Meter is not found", ErrorItems = new string[] { } });
                }
                else
                {
                    var Meter = await meterService.GetMeter(id.Value);
                    return Ok(new { IsSuccess = true, Message = "Meter retrieved succeessfully", Data = MeterVM.FromMeter(Meter) });
                }
            }
            catch (AppException ex)
            {
                return StatusCode(400, new { IsSuccess = false, Message = ex.Message, ErrorDetail = JsonSerializer.Serialize(ex.InnerException) });
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "An error was encountered while fetching a meter");

                return StatusCode(500, new { IsSuccess = false, Message = ex.Message, ErrorDetail = JsonSerializer.Serialize(ex.InnerException) });
            }
        }
       
    }
}
