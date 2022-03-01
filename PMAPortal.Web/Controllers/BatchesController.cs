using DataTablesParser;
using Microsoft.AspNetCore.Http;
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
    public class BatchesController : Controller
    {
        private readonly IBatchService batchService;
        private readonly IOptionsSnapshot<AppSettings> appSettingsDelegate;
        private readonly ILoggerService<BatchesController> logger;

        public BatchesController(
            IBatchService batchService,
            IOptionsSnapshot<AppSettings> appSettingsDelegate,
            ILoggerService<BatchesController> logger)
        {
            this.batchService = batchService;
            this.appSettingsDelegate = appSettingsDelegate;
            this.logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult BatchesDataTable()
        {
            var clientTimeOffset = string.IsNullOrEmpty(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]) ?
                appSettingsDelegate.Value.DefaultTimeZoneOffset : Convert.ToInt32(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]);

            var batches = batchService.GetBatches().Select(b => BatchVM.FromBatch(b, clientTimeOffset));

            var parser = new Parser<BatchVM>(Request.Form, batches.AsQueryable())
                   .SetConverter(x => x.CreatedDate, x => x.CreatedDate.ToString("MMM d, yyyy 'at' hh:mmtt"))
                   .SetConverter(x => x.DateShared, x => x.DateShared.ToString("MMM d, yyyy 'at' hh:mmtt"));

            return Ok(parser.Parse());
        }

        [HttpPost]
        public async Task<IActionResult> UploadBatch(IFormFile file, DateTimeOffset? dateShared)
        {
            try
            {
                await batchService.AddCustomerBatch(file, dateShared);
                return Ok(new { IsSuccess = true, Message = "Batch file uploaded and read succeessfully", ErrorItems = new string[] { } });
            }
            catch (AppException ex)
            {
                return StatusCode(400, new { IsSuccess = false, Message = ex.Message, ErrorItems=ex.ErrorItems, ErrorDetail = JsonSerializer.Serialize(ex.InnerException) });
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "An error was encountered while uploading a new batch");

                return StatusCode(500, new { IsSuccess = false, Message = ex.Message, ErrorDetail = JsonSerializer.Serialize(ex.InnerException) });
            }
        }


        public async Task<IActionResult> DeleteBatch(long? id)
        {
            try
            {
                if (id == null)
                {
                    return StatusCode(400, new { IsSuccess = false, Message = "Batch is not found", ErrorItems = new string[] { } });
                }
                else
                {
                    await batchService.DeleteBatch(id.Value);
                    return Ok(new { IsSuccess = true, Message = "Batch deleted succeessfully", ErrorItems = new string[] { } });
                }
            }
            catch (AppException ex)
            {
                return StatusCode(400, new { IsSuccess = false, Message = ex.Message, ErrorDetail = JsonSerializer.Serialize(ex.InnerException) });
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "An error was encountered while deleting a batch");

                return StatusCode(500, new { IsSuccess = false, Message = ex.Message, ErrorDetail = JsonSerializer.Serialize(ex.InnerException) });
            }
        }

        public async Task<IActionResult> GetBatch(long? id)
        {
            try
            {
                if (id == null)
                {
                    return StatusCode(400, new { IsSuccess = false, Message = "Batch is not found", ErrorItems = new string[] { } });
                }
                else
                {
                    var batch = await batchService.GetBatch(id.Value);
                    return Ok(new { IsSuccess = true, Message = "Batch retrieved succeessfully", Data = BatchVM.FromBatch(batch) });
                }
            }
            catch (AppException ex)
            {
                return StatusCode(400, new { IsSuccess = false, Message = ex.Message, ErrorDetail = JsonSerializer.Serialize(ex.InnerException) });
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "An error was encountered while fetching a batch");

                return StatusCode(500, new { IsSuccess = false, Message = ex.Message, ErrorDetail = JsonSerializer.Serialize(ex.InnerException) });
            }
        }
    }
}
