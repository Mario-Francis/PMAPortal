using AutoMapper;
using ClosedXML.Excel;
using DataTablesParser;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MimeKit;
using PMAPortal.Web.DTOs;
using PMAPortal.Web.Extensions;
using PMAPortal.Web.Services;
using PMAPortal.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ILoggerService<ReportsController> logger;
        private readonly IApplicationService applicationService;
        private readonly IMeterService meterService;
        private readonly IPaymentService paymentService;
        private readonly IOptionsSnapshot<AppSettings> appSettingsDelegate;
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public ReportsController(ILoggerService<ReportsController> logger,
            IApplicationService applicationService,
            IMeterService meterService,
            IPaymentService paymentService,
            IOptionsSnapshot<AppSettings> appSettingsDelegate,
            IUserService userService,
            IMapper mapper)
        {
            this.logger = logger;
            this.applicationService = applicationService;
            this.meterService = meterService;
            this.paymentService = paymentService;
            this.appSettingsDelegate = appSettingsDelegate;
            this.userService = userService;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ReportsDataTable(ReportFilterVM filter)
        {
            var clientTimeOffset = string.IsNullOrEmpty(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]) ?
                appSettingsDelegate.Value.DefaultTimeZoneOffset : Convert.ToInt32(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]);

            var _applications = applicationService.GetApplications().Where(a => a.ApplicationStatusId != 1);

            if (filter.MeterId != null)
            {
                _applications = _applications.Where(a => a.MeterId == filter.MeterId.Value);
            }

            if (!string.IsNullOrEmpty(filter.Area))
            {
                _applications = _applications.Where(a => a.Applicant.ApplicantAddresses.Any(ad => ad.Area == filter.Area));
            }

            if (filter.StatusId != null)
            {
                _applications = _applications.Where(a => a.ApplicationStatusId == filter.StatusId.Value);
            }

            if (filter.InstallerId != null)
            {
                _applications = _applications.Where(a => a.InstallerId == filter.InstallerId.Value);
            }

            if (filter.FromDate != null  && filter.ToDate==null)
            {
                _applications = _applications.Where(a => a.CreatedDate >= filter.FromDate);
            }

            if(filter.FromDate != null && filter.ToDate!=null && filter.ToDate >= filter.FromDate)
            {
                _applications = _applications.Where(a => a.CreatedDate >= filter.FromDate && a.CreatedDate <= filter.ToDate);
            }else if(filter.FromDate != null && filter.ToDate != null && filter.ToDate <= filter.FromDate)
            {
                _applications = _applications.Where(a => a.CreatedDate <= filter.FromDate && a.CreatedDate >= filter.ToDate);
            }

            IEnumerable<ApplicationItemVM> applications = _applications
                .OrderByDescending(a => a.Id).Select(a => ApplicationItemVM.FromApplication(a, clientTimeOffset));

           

            var parser = new Parser<ApplicationItemVM>(Request.Form, applications.AsQueryable())
                  .SetConverter(x => x.UpdatedDate, x => x.UpdatedDate.ToString("MMM d, yyyy 'at' hh:mmtt"))
                   .SetConverter(x => x.CreatedDate, x => x.CreatedDate.ToString("MMM d, yyyy 'at' hh:mmtt"));

            var dtData = parser.Parse();
            var result = mapper.Map<DataTableResultVM<ApplicationItemVM>>(dtData);

            result.MeterCount = _applications.Count();
            result.MeterCountByTypes = _applications.GroupBy(a => a.Meter).Select(g => new ReportItemVM { Count = g.Count(), Name = g.Key.Name });
            result.TotalAmount = _applications.Select(a => a.Payments.First().Amount).Sum();
            result.FormattedTotalAmount = result.TotalAmount.Format();
            result.TotalAmountByTypes = _applications.GroupBy(a => a.Meter)
                .Select(g => new ReportItemVM { 
                    Name = g.Key.Name, 
                    Amount = g.Select(i => i.Payments.First().Amount).Sum(),  
                    FormattedAmount= g.Select(i => i.Payments.First().Amount).Sum().Format() });
            result.AreaCount = _applications.Select(a => a.Applicant.ApplicantAddresses.First().Area).Distinct().Count();
            result.Areas = _applications.Select(a => a.Applicant.ApplicantAddresses.First().Area).Distinct();

            return Ok(result);
            //return Ok(parser.Parse());
        }

        public IActionResult ExportReport(ReportFilterVM filter)
        {
            var file = ExportReportToExcel(filter);
            var fileName = $"Applications Report_{DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss")}.xlsx";
            return File(file, MimeTypes.GetMimeType(fileName), fileName);
        }
        private byte[] ExportReportToExcel(ReportFilterVM filter)
        {
            var clientTimeOffset = string.IsNullOrEmpty(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]) ?
               appSettingsDelegate.Value.DefaultTimeZoneOffset : Convert.ToInt32(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]);

            var _applications = applicationService.GetApplications().Where(a => a.ApplicationStatusId != 1);

            if (filter.MeterId != null)
            {
                _applications = _applications.Where(a => a.MeterId == filter.MeterId.Value);
            }

            if (!string.IsNullOrEmpty(filter.Area))
            {
                _applications = _applications.Where(a => a.Applicant.ApplicantAddresses.Any(ad => ad.Area == filter.Area));
            }

            if (filter.StatusId != null)
            {
                _applications = _applications.Where(a => a.ApplicationStatusId == filter.StatusId.Value);
            }

            if (filter.InstallerId != null)
            {
                _applications = _applications.Where(a => a.InstallerId == filter.InstallerId.Value);
            }

            if (filter.FromDate != null && filter.ToDate == null)
            {
                _applications = _applications.Where(a => a.CreatedDate >= filter.FromDate);
            }

            if (filter.FromDate != null && filter.ToDate != null && filter.ToDate >= filter.FromDate)
            {
                _applications = _applications.Where(a => a.CreatedDate >= filter.FromDate && a.CreatedDate <= filter.ToDate);
            }
            else if (filter.FromDate != null && filter.ToDate != null && filter.ToDate <= filter.FromDate)
            {
                _applications = _applications.Where(a => a.CreatedDate <= filter.FromDate && a.CreatedDate >= filter.ToDate);
            }
            var applications = _applications.OrderByDescending(a => a.Id).Select(a => ApplicationItemVM.FromApplication(a, clientTimeOffset));
            var meterCount = _applications.Count();
            var meterCountByTypes = _applications.GroupBy(a => a.Meter).Select(g => new ReportItemVM { Count = g.Count(), Name = g.Key.Name });
            var totalAmount = _applications.Select(a => a.Payments.First().Amount).Sum();
            var formattedTotalAmount = totalAmount.Format();
            var totalAmountByTypes = _applications.GroupBy(a => a.Meter)
                .Select(g => new ReportItemVM
                {
                    Name = g.Key.Name,
                    Amount = g.Select(i => i.Payments.First().Amount).Sum(),
                    FormattedAmount = g.Select(i => i.Payments.First().Amount).Sum().Format()
                });
            var areaCount = _applications.Select(a => a.Applicant.ApplicantAddresses.First().Area).Distinct().Count();
            var areas = _applications.Select(a => a.Applicant.ApplicantAddresses.First().Area).Distinct();

            // create excel
            var workbook = new XLWorkbook(ClosedXML.Excel.XLEventTracking.Disabled);

            // using data table
            var table = new DataTable("Applications Report");
            table.Columns.Add("SN", typeof(string));
            table.Columns.Add("Applicant", typeof(string));
            table.Columns.Add("Area", typeof(string));
            table.Columns.Add("Phone Number", typeof(string));
            table.Columns.Add("Meter", typeof(string));
            table.Columns.Add("Tracking Number", typeof(string));
            table.Columns.Add("Amount Paid", typeof(string));
            table.Columns.Add("Status", typeof(string));
            table.Columns.Add("Assigned To", typeof(string));
            table.Columns.Add("Created Date", typeof(string));
            table.Columns.Add("Last Updated By", typeof(string));
            table.Columns.Add("Last Updated Date", typeof(string));
           

            var cnt = 1;
            foreach (var a in applications)
            {
                var row = table.NewRow();

                row[0] = cnt;
                row[1] = a.Applicant;
                row[2] = a.Area;
                row[3] = a.PhoneNumber;
                row[4] = a.Meter;
                row[5] = a.TrackNumber;
                row[6] = a.AmountPaid;
                row[7] = a.Status;
                row[8] = a.Installer;
                row[9] = a.FormattedCreatedDate;
                row[10] = a.UpdatedBy;
                row[11] = a.FormattedUpdatedDate;

                table.Rows.Add(row);
                cnt++;
            }
            var _row = table.NewRow();
            _row[1] = "Area Count";
            _row[2] = areaCount.ToString();
            _row[3] = "Meter Count";
            _row[4] = meterCount;
            _row[5] = "Total Amount";
            _row[6] = formattedTotalAmount;

            table.Rows.Add(_row);

            workbook.AddWorksheet(table);

            byte[] byteFile = null;
            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                byteFile = stream.ToArray();
            }

            return byteFile;
        }

    }
}
