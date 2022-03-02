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
        public IActionResult CustomersDataTable()
        {
            var clientTimeOffset = string.IsNullOrEmpty(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]) ?
                appSettingsDelegate.Value.DefaultTimeZoneOffset : Convert.ToInt32(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]);

            var customers = customerService.GetCustomers().Select(c => CustomerVM.FromCustomer(c, clientTimeOffset));

            var parser = new Parser<CustomerVM>(Request.Form, customers.AsQueryable())
                   .SetConverter(x => x.CreatedDate, x => x.CreatedDate.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.DateShared, x => x.DateShared==null?"":x.DateShared.Value.ToString("MMM d, yyyy 'at' hh:mmtt"));

            return Ok(parser.Parse());
        }

        public async Task<IActionResult> DeleteCustomer(long? id)
        {
            try
            {
                if (id == null)
                {
                    return StatusCode(400, new { IsSuccess = false, Message = "Customer is not found", ErrorItems = new string[] { } });
                }
                else
                {
                    await customerService.DeleteCustomer(id.Value);
                    return Ok(new { IsSuccess = true, Message = "Customer deleted succeessfully", ErrorItems = new string[] { } });
                }
            }
            catch (AppException ex)
            {
                return StatusCode(400, new { IsSuccess = false, Message = ex.Message, ErrorDetail = JsonSerializer.Serialize(ex.InnerException) });
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "An error was encountered while deleting a customer");

                return StatusCode(500, new { IsSuccess = false, Message = ex.Message, ErrorDetail = JsonSerializer.Serialize(ex.InnerException) });
            }
        }

        public async Task<IActionResult> GetCustomer(long? id)
        {
            try
            {
                if (id == null)
                {
                    return StatusCode(400, new { IsSuccess = false, Message = "Customer is not found", ErrorItems = new string[] { } });
                }
                else
                {
                    var customer = await customerService.GetCustomer(id.Value);
                    return Ok(new { IsSuccess = true, Message = "Customer retrieved succeessfully", Data = CustomerVM.FromCustomer(customer) });
                }
            }
            catch (AppException ex)
            {
                return StatusCode(400, new { IsSuccess = false, Message = ex.Message, ErrorDetail = JsonSerializer.Serialize(ex.InnerException) });
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "An error was encountered while fetching a customer");

                return StatusCode(500, new { IsSuccess = false, Message = ex.Message, ErrorDetail = JsonSerializer.Serialize(ex.InnerException) });
            }
        }
    }
}
