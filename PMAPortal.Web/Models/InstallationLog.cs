using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.Models
{
    public class InstallationLog:BaseEntity
    {
        public long InstallationId { get; set; }
        public long? ActionBy { get; set; }
        public long InstallationStatusId { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }

        // navigation properties
       // [NotMapped]
        public virtual User ActionByUser { get; set; }
        public virtual Installation Installation { get; set; }
        public virtual InstallationStatus InstallationStatus { get; set; }
    }
}
