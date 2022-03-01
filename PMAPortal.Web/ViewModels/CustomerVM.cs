using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.ViewModels
{
    public class CustomerVM
    {
        public long Id { get; set; }
        public long BatchId { get; set; }
        public string AccountNumber { get; set; }
        public string ARN { get; set; }
        public string CustomerName { get; set; }
        public string CISName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string CISAddress { get; set; }
        public string Landmark { get; set; }

        public string SurveyStatus { get; set; } // pending | Meter ready | Not Meter Ready
        public string InstallationStatus { get; set; }

        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string FormattedCreatedDate
        {
            get
            {
                return CreatedDate.ToString("MMM d, yyyy 'at' hh:mmtt");
            }
        }
    }
}
