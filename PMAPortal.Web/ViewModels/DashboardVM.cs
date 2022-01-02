using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.ViewModels
{
    public class DashboardVM
    {
        public long TotalApplicationCount { get; set; }
        public long UnassignedApplicationCount { get; set; }
        public int TotalAssignedApplicationCount { get; set; }
        public int PendingApplicationCount { get; set; }
        public int FailedApplicationCount { get; set; }
        public int CompletedApplicationCount { get; set; }
        public string TotalIncome { get; set; }
        public string CompletedIncome { get; set; }
        public string FailedIncome { get; set; }
        public string PendingIncome { get; set; }
        public int UsersCount { get; set; }
        public int FeedbacksCount { get; set; }
        public IEnumerable<ReportItemVM> TotalAmountByType { get; set; }
    }
}
