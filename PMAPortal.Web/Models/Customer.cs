using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.Models
{
    public class Customer:BaseEntity
    {
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

        // navigation
        public virtual Batch Batch { get; set; }
        public virtual ICollection<Survey> Surveys { get; set; }
        public virtual ICollection<Installation> Installations { get; set; }
        public virtual ICollection<CustomerStatusLog> CustomerStatusLogs { get; set; }
    }
}
