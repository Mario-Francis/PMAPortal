using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.Models
{
    public class ApplicantFeedback:BaseEntity
    {
        public long ApplicantId { get; set; }
        public long ApplicationId { get; set; }
        public string Rating { get; set; }
        public string Comment { get; set; }

        // navigation properties
        public virtual Applicant Applicant { get; set; }
    }
}
