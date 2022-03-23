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

        private readonly ISurveyService surveyService;
        private readonly ICustomerService customerService;
        private readonly IInstallationService installationService;

        public ReportsController(ILoggerService<ReportsController> logger,
            IApplicationService applicationService,
            IMeterService meterService,
            IPaymentService paymentService,
            IOptionsSnapshot<AppSettings> appSettingsDelegate,
            IUserService userService,
            IMapper mapper,
            ISurveyService surveyService,
            ICustomerService customerService,
            IInstallationService installationService
            )
        {
            this.logger = logger;
            this.applicationService = applicationService;
            this.meterService = meterService;
            this.paymentService = paymentService;
            this.appSettingsDelegate = appSettingsDelegate;
            this.userService = userService;
            this.mapper = mapper;
            this.surveyService = surveyService;
            this.customerService = customerService;
            this.installationService = installationService;
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

        public IActionResult Surveys()
        {
            var surveyCompanies = surveyService.GetSurveys().Where(s => s.SurveryCompany != null).Select(s => s.SurveryCompany);
            ViewData["SurveyCompanies"] = surveyCompanies;

            return View();
        }

        [HttpPost]
        public IActionResult AllSurveysDataTable(ReportFilterVM filter)
        {
            var clientTimeOffset = string.IsNullOrEmpty(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]) ?
                appSettingsDelegate.Value.DefaultTimeZoneOffset : Convert.ToInt32(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]);

            var customers = customerService.GetCustomers().Select(c => ReportVM.FromCustomer(c, clientTimeOffset));

            if (filter.SurveyStatus == "Pending")
            {
                customers = customers.Where(c => c.SurveyRemark == null);
            }else if(filter.SurveyStatus == "Completed")
            {
                customers = customers.Where(c => c.SurveyRemark != null);
            }

            if (filter.ScheduleStatus == "Scheduled")
            {
                customers = customers.Where(c => c.ScheduleDate != null);
            }
            else if (filter.ScheduleStatus == "Not Scheduled")
            {
                customers = customers.Where(c => c.ScheduleDate == null);
            }

            if (filter.SurveyStaff != null)
            {
                customers = customers.Where(c => c.SurveyStaffId == filter.SurveyStaff);
            }
            // shared date
            if (filter.SharedDateFrom != null && filter.SharedDateTo == null)
            {
                customers = customers.Where(c => c.DateShared!=null  && c.DateShared >= filter.SharedDateFrom);
            }

            if (filter.SharedDateFrom != null && filter.SharedDateTo != null && filter.SharedDateTo >= filter.SharedDateFrom)
            {
                customers = customers.Where(c => c.DateShared >= filter.SharedDateFrom && c.DateShared <= filter.SharedDateTo);
            }
            else if (filter.SharedDateFrom != null && filter.SharedDateTo != null && filter.SharedDateTo <= filter.SharedDateFrom)
            {
                customers = customers.Where(c => c.DateShared <= filter.SharedDateFrom && c.DateShared >= filter.SharedDateTo);
            }
            // schedule date
            if (filter.ScheduleDateFrom != null && filter.ScheduleDateTo == null)
            {
                customers = customers.Where(c => c.ScheduleDate != null && c.ScheduleDate >= filter.ScheduleDateFrom);
            }

            if (filter.ScheduleDateFrom != null && filter.ScheduleDateTo != null && filter.ScheduleDateTo >= filter.ScheduleDateFrom)
            {
                customers = customers.Where(c => c.ScheduleDate >= filter.ScheduleDateFrom && c.ScheduleDate <= filter.ScheduleDateTo);
            }
            else if (filter.ScheduleDateFrom != null && filter.ScheduleDateTo != null && filter.ScheduleDateTo <= filter.ScheduleDateFrom)
            {
                customers = customers.Where(c => c.ScheduleDate <= filter.ScheduleDateFrom && c.ScheduleDate >= filter.ScheduleDateTo);
            }

            var parser = new Parser<ReportVM>(Request.Form, customers.AsQueryable())
                   .SetConverter(x => x.CreatedDate, x => x.CreatedDate.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.DateShared, x => x.DateShared == null ? "" : x.DateShared.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.ScheduleDate, x => x.ScheduleDate == null ? "" : x.ScheduleDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.SurveyDate, x => x.SurveyDate == null ? "" : x.SurveyDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"));

            return Ok(parser.Parse());
        }

        public async Task<IActionResult> ExportSurveyReport(ReportFilterVM filter)
        {
            var file = await ExportSurveyReportToExcel(filter);
            var fileName = $"Survey Report_{DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss")}.xlsx";
            return File(file, MimeTypes.GetMimeType(fileName), fileName);
        }
        private async Task<byte[]> ExportSurveyReportToExcel(ReportFilterVM filter)
        {
            var clientTimeOffset = string.IsNullOrEmpty(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]) ?
                 appSettingsDelegate.Value.DefaultTimeZoneOffset : Convert.ToInt32(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]);

            var customers = customerService.GetCustomers().Select(c => ReportVM.FromCustomer(c, clientTimeOffset));

            if (filter.SurveyStatus == "Pending")
            {
                customers = customers.Where(c => c.SurveyRemark == null);
            }
            else if (filter.SurveyStatus == "Completed")
            {
                customers = customers.Where(c => c.SurveyRemark != null);
            }

            if (filter.ScheduleStatus == "Scheduled")
            {
                customers = customers.Where(c => c.ScheduleDate != null);
            }
            else if (filter.ScheduleStatus == "Not Scheduled")
            {
                customers = customers.Where(c => c.ScheduleDate == null);
            }

            if (filter.SurveyStaff != null)
            {
                customers = customers.Where(c => c.SurveyStaffId == filter.SurveyStaff);
            }
            // shared date
            if (filter.SharedDateFrom != null && filter.SharedDateTo == null)
            {
                customers = customers.Where(c => c.DateShared != null && c.DateShared >= filter.SharedDateFrom);
            }

            if (filter.SharedDateFrom != null && filter.SharedDateTo != null && filter.SharedDateTo >= filter.SharedDateFrom)
            {
                customers = customers.Where(c => c.DateShared >= filter.SharedDateFrom && c.DateShared <= filter.SharedDateTo);
            }
            else if (filter.SharedDateFrom != null && filter.SharedDateTo != null && filter.SharedDateTo <= filter.SharedDateFrom)
            {
                customers = customers.Where(c => c.DateShared <= filter.SharedDateFrom && c.DateShared >= filter.SharedDateTo);
            }
            // schedule date
            if (filter.ScheduleDateFrom != null && filter.ScheduleDateTo == null)
            {
                customers = customers.Where(c => c.ScheduleDate != null && c.ScheduleDate >= filter.ScheduleDateFrom);
            }

            if (filter.ScheduleDateFrom != null && filter.ScheduleDateTo != null && filter.ScheduleDateTo >= filter.ScheduleDateFrom)
            {
                customers = customers.Where(c => c.ScheduleDate >= filter.ScheduleDateFrom && c.ScheduleDate <= filter.ScheduleDateTo);
            }
            else if (filter.ScheduleDateFrom != null && filter.ScheduleDateTo != null && filter.ScheduleDateTo <= filter.ScheduleDateFrom)
            {
                customers = customers.Where(c => c.ScheduleDate <= filter.ScheduleDateFrom && c.ScheduleDate >= filter.ScheduleDateTo);
            }

            // create excel
            var workbook = new XLWorkbook(ClosedXML.Excel.XLEventTracking.Disabled);

            // using data table
            var table = new DataTable("Survey Report");
            table.Columns.Add("SN", typeof(string));
            table.Columns.Add("Date Shared", typeof(string));
            table.Columns.Add("Batch", typeof(string));
            table.Columns.Add("Account Number", typeof(string));
            table.Columns.Add("ARN", typeof(string));
            table.Columns.Add("Customer Name", typeof(string));
            table.Columns.Add("CIS Name", typeof(string));
            table.Columns.Add("Email", typeof(string));
            table.Columns.Add("Phone Number", typeof(string));
            table.Columns.Add("Address", typeof(string));
            table.Columns.Add("CIS Address", typeof(string));
            table.Columns.Add("Landmark", typeof(string));
            table.Columns.Add("BU", typeof(string));
            table.Columns.Add("UT", typeof(string));
            table.Columns.Add("Feeder", typeof(string));
            table.Columns.Add("DT", typeof(string));
            table.Columns.Add("Tariff", typeof(string));
            table.Columns.Add("Metered Status", typeof(string));
            table.Columns.Add("Ready To Pay?", typeof(string));
            table.Columns.Add("Occupier Phone Number", typeof(string));
            table.Columns.Add("Type of Apartment", typeof(string));
            table.Columns.Add("Existing Meter Type", typeof(string));
            table.Columns.Add("Existing Meter Number", typeof(string));
            table.Columns.Add("Does customer bill match data in column 3 - 10?", typeof(string));
            table.Columns.Add("Estimated Customer Total Load", typeof(string));
            table.Columns.Add("Recommended Meter Type", typeof(string));
            table.Columns.Add("Installation Mode", typeof(string));
            table.Columns.Add("Load wire separation required?", typeof(string));
            table.Columns.Add("Account separation required?", typeof(string));
            table.Columns.Add("Number of 1Q required for account separation", typeof(string));
            table.Columns.Add("Number of 3Q required for account separation", typeof(string));
            table.Columns.Add("survey company", typeof(string));
            table.Columns.Add("Survey Staff", typeof(string));
            table.Columns.Add("Survey Date", typeof(string));
            table.Columns.Add("Remark", typeof(string));
            table.Columns.Add("MAP", typeof(string));
            table.Columns.Add("Reporting Date", typeof(string));
            table.Columns.Add("Additional Comment", typeof(string));


            var cnt = 1;
            foreach (var c in customers)
            {
                
                var row = table.NewRow();

                row[0] = cnt;
                row[1] = c.FormattedDateShared;
                row[2] = c.BatchNumber;
                row[3] = c.AccountNumber;
                row[4] = c.ARN;
                row[5] = c.CustomerName;
                row[6] = c.CISName;
                row[7] = c.Email;
                row[8] = c.PhoneNumber;
                row[9] = c.Address;
                row[10] = c.CISAddress;
                row[11] = c.Landmark;
                row[12] = c.BU;
                row[13] = c.UT;
                row[14] = c.Feeder;
                row[15] = c.DT;
                row[16] = c.Tariff;
                row[17] = c.MeteredStatus;
                if (c.SurveyId != null)
                {
                    var s = await surveyService.GetSurvey(c.SurveyId.Value);

                    row[18] = s.ReadyToPay;
                    row[19] = s.OccupierPhoneNumber;
                    row[20] = $"{s.BedroomCount} {s.TypeOfApartment}";
                    row[21] = s.ExistingMeterType;
                    row[22] = s.ExistingMeterNumber;
                    row[23] = s.CustomerBillMatchUploadedData;
                    row[24] = s.EstimatedTotalLoadInAmps;
                    row[25] = s.RecommendedMeterType;
                    row[26] = s.InstallationMode;
                    row[27] = s.LoadWireSeparationRequired;
                    row[28] = s.AccountSeparationRequired;
                    row[29] = s.NumberOf1QRequired;
                    row[30] = s.NumberOf3QRequired;
                    row[31] = s.SurveryCompany;
                    row[32] = c.SurveyStaff;
                    row[33] = c.FormattedSurveyDate;
                    row[34] = s.SurveyRemark;
                    row[35] = s.MAP;
                    
                    row[37] = s.AdditionalComment;
                }
                row[36] = DateTime.Now.ToString("MMM d, yyyy 'at' hh:mmtt");

                table.Rows.Add(row);
                cnt++;
            }

            workbook.AddWorksheet(table);

            byte[] byteFile = null;
            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                byteFile = stream.ToArray();
            }

            return byteFile;
        }

        public IActionResult Installations()
        {
            var installationCompanies = installationService.GetInstallations().Where(s => s.Installer != null).Select(s => s.Installer.CompanyName);
            ViewData["InstallationCompanies"] = installationCompanies;

            return View();
        }

        [HttpPost]
        public IActionResult AllInstallationsDataTable(ReportFilterVM filter)
        {
            var clientTimeOffset = string.IsNullOrEmpty(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]) ?
                appSettingsDelegate.Value.DefaultTimeZoneOffset : Convert.ToInt32(Request.Cookies[Constants.CLIENT_TIMEOFFSET_COOKIE_ID]);

            var customers = installationService.GetInstallations().Select(i => ReportVM.FromCustomer(i.Customer, clientTimeOffset));

            if (filter.InstallationStatusId != null)
            {
                customers = customers.Where(c => c.InstallationStatusId==filter.InstallationStatusId);
            }

            if (filter.ScheduleStatus == "Scheduled")
            {
                customers = customers.Where(c => c.IScheduleDate != null);
            }
            else if (filter.ScheduleStatus == "Not Scheduled")
            {
                customers = customers.Where(c => c.IScheduleDate == null);
            }

            if (filter.InstallerId != null)
            {
                customers = customers.Where(c => c.InstallerId == filter.InstallerId);
            }

            if (filter.MeterType != null)
            {
                customers = customers.Where(c => c.MeterType == filter.MeterType);
            }
           
            // schedule date
            if (filter.ScheduleDateFrom != null && filter.ScheduleDateTo == null)
            {
                customers = customers.Where(c => c.ScheduleDate != null && c.ScheduleDate >= filter.ScheduleDateFrom);
            }

            if (filter.ScheduleDateFrom != null && filter.ScheduleDateTo != null && filter.ScheduleDateTo >= filter.ScheduleDateFrom)
            {
                customers = customers.Where(c => c.ScheduleDate >= filter.ScheduleDateFrom && c.ScheduleDate <= filter.ScheduleDateTo);
            }
            else if (filter.ScheduleDateFrom != null && filter.ScheduleDateTo != null && filter.ScheduleDateTo <= filter.ScheduleDateFrom)
            {
                customers = customers.Where(c => c.ScheduleDate <= filter.ScheduleDateFrom && c.ScheduleDate >= filter.ScheduleDateTo);
            }

            var parser = new Parser<ReportVM>(Request.Form, customers.AsQueryable())
                   .SetConverter(x => x.CreatedDate, x => x.CreatedDate.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.DateShared, x => x.DateShared == null ? "" : x.DateShared.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.ScheduleDate, x => x.ScheduleDate == null ? "" : x.ScheduleDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"))
                    .SetConverter(x => x.SurveyDate, x => x.SurveyDate == null ? "" : x.SurveyDate.Value.ToString("MMM d, yyyy 'at' hh:mmtt"));

            return Ok(parser.Parse());
        }
    }
}
