using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PMAPortal.Web.DTOs;
using PMAPortal.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.Controllers
{
    public class CustomersController : Controller
    {
        private readonly IOptionsSnapshot<AppSettings> appSettingsDelegate;
        private readonly ICustomerService customerService;
        private readonly ILoggerService<CustomersController> logger;

        public CustomersController(
             IOptionsSnapshot<AppSettings> appSettingsDelegate,
            ICustomerService customerService,
            ILoggerService<CustomersController> logger)
        {
            this.appSettingsDelegate = appSettingsDelegate;
            this.customerService = customerService;
            this.logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AreasDataTable()
        {
            var clientTimeOffset = string.IsNullOrEmpty(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]) ?
                appSettingsDelegate.Value.DefaultTimeZoneOffset : Convert.ToInt32(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]);

            var Areas = areaService.GetAreas().Select(s => ItemVM.FromArea(s, clientTimeOffset));

            var parser = new Parser<ItemVM>(Request.Form, Areas.AsQueryable())
                   .SetConverter(x => x.CreatedDate, x => x.CreatedDate.ToString("MMM d, yyyy 'at' hh:mmtt"));

            return Ok(parser.Parse());
        }

        [HttpPost]
        public async Task<IActionResult> AddArea(ItemVM model)
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
                    await areaService.CreateArea(new Area { Name = model.Name });
                    return Ok(new { IsSuccess = true, Message = "Area added succeessfully", ErrorItems = new string[] { } });
                }
            }
            catch (AppException ex)
            {
                return StatusCode(400, new { IsSuccess = false, Message = ex.Message, ErrorDetail = JsonSerializer.Serialize(ex.InnerException) });
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "An error was encountered while creating a new area");

                return StatusCode(500, new { IsSuccess = false, Message = ex.Message, ErrorDetail = JsonSerializer.Serialize(ex.InnerException) });
            }
        }


        public async Task<IActionResult> DeleteArea(long? id)
        {
            try
            {
                if (id == null)
                {
                    return StatusCode(400, new { IsSuccess = false, Message = "Area is not found", ErrorItems = new string[] { } });
                }
                else
                {
                    await areaService.DeleteArea(id.Value);
                    return Ok(new { IsSuccess = true, Message = "Area deleted succeessfully", ErrorItems = new string[] { } });
                }
            }
            catch (AppException ex)
            {
                return StatusCode(400, new { IsSuccess = false, Message = ex.Message, ErrorDetail = JsonSerializer.Serialize(ex.InnerException) });
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "An error was encountered while deleting a area");

                return StatusCode(500, new { IsSuccess = false, Message = ex.Message, ErrorDetail = JsonSerializer.Serialize(ex.InnerException) });
            }
        }

        public async Task<IActionResult> GetArea(long? id)
        {
            try
            {
                if (id == null)
                {
                    return StatusCode(400, new { IsSuccess = false, Message = "Area is not found", ErrorItems = new string[] { } });
                }
                else
                {
                    var Area = await areaService.GetArea(id.Value);
                    return Ok(new { IsSuccess = true, Message = "Area retrieved succeessfully", Data = ItemVM.FromArea(Area) });
                }
            }
            catch (AppException ex)
            {
                return StatusCode(400, new { IsSuccess = false, Message = ex.Message, ErrorDetail = JsonSerializer.Serialize(ex.InnerException) });
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "An error was encountered while fetching a area");

                return StatusCode(500, new { IsSuccess = false, Message = ex.Message, ErrorDetail = JsonSerializer.Serialize(ex.InnerException) });
            }
        }
    }
}
