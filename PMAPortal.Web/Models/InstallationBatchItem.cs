using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.Models
{
    public class InstallationBatchItem:BaseEntity
    {
        public long InstallationBatchId { get; set; }
        public string SN { get; set; }
        public DateTimeOffset? DateShared { get; set; }
        public string BatchNumber { get; set; }
        public string AccountNumber { get; set; }
        public string CustomerName { get; set; }

        //public long? CustomerId { get; set; }
        

        // navigation
        public virtual InstallationBatch InstallationBatch { get; set; }
        public virtual Customer Customer { get; set; }

    }
}
