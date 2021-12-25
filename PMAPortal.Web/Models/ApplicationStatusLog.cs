using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.Models
{
    public class ApplicationStatusLog:BaseEntity
    {
        public long ApplicationId { get; set; }
        public long? ActionBy { get; set; }
        public long ApplicationStatusId { get; set; }
        public string Comment { get; set; }

        // navigation properties
        [NotMapped]
        public virtual User ActionByUser { get; set; }
        public virtual Application Application { get; set; }
        public virtual ApplicationStatus ApplicationStatus { get; set; }
    }
}
