using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.Models
{
    public class CustomerStatusLog:BaseEntity
    {
        public long CustomerId { get; set; }
        public long? ActionBy { get; set; }
        public string Description { get; set; }
        public long? InstallationStatusId { get; set; }
        public string Comment { get; set; }

        // navigation properties
        [NotMapped]
        public virtual User ActionByUser { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual InstallationStatus InstallationStatus { get; set; }
    }
}
