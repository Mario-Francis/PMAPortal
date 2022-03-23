using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.Models
{
    public class CustomerFeedback:BaseEntity
    {
        public long CustomerId { get; set; }
        public long InstallationId { get; set; }
        public string Rating { get; set; }
        public string Comment { get; set; }

        // navigation properties
        public virtual Customer Customer { get; set; }
        public virtual Installation Installation { get; set; }
        public virtual ICollection<FeedbackAnswer> FeedbackAnswers { get; set; }
    }
}
