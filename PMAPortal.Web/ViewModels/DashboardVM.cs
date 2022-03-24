using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.ViewModels
{
    public class DashboardVM
    {
        public int CustomerCount { get; set; }

        public int PendingSurveyCount { get; set; }
        public int CompletedSurveyCount { get; set; }
        public int MeterReadyCount { get; set; }
        public int NotMeterReadyCount { get; set; }

        public int PendingInstallationCount { get; set; }
        public int CompletedInstallationCount { get; set; }
        public int RejectedInstallationCount { get; set; }
        public int ApprovedInstallationCount { get; set; }

        public int MyPendingSurveyCount { get; set; }
        public int MyCompletedSurveyCount { get; set; }

        public int MyPendingInstallationCount { get; set; }
        public int MyCompletedInstallationCount { get; set; }
        public int MyApprovedInstallationCount { get; set; }
        public int MyRejectedInstallationCount { get; set; }

        public int UserCount { get; set; }
        public int FeedbackCount { get; set; }
        public int CustomerBatchCount { get; set; }
        public int InstallerBatchCount { get; set; }

    }
}
