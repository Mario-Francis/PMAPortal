using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.ViewModels
{
    public class ReportFilterVM
    {
        public long? MeterId { get; set; }
        public string Area { get; set; }
        public long? StatusId { get; set; }
        public long? InstallerId { get; set; }
        public DateTimeOffset? FromDate { get; set; }
        public DateTimeOffset? ToDate { get; set; }

        // =====================================
        public string SurveyStatus { get; set; }
        public string ScheduleStatus { get; set; }
        public long? SurveyStaff { get; set; }
        public DateTimeOffset? SharedDateFrom { get; set; }
        public DateTimeOffset? SharedDateTo { get; set; }
        public DateTimeOffset? ScheduleDateFrom { get; set; }
        public DateTimeOffset? ScheduleDateTo { get; set; }

        public long? InstallationStatusId { get; set; }
        public string MeterType { get; set; }

    }
}
