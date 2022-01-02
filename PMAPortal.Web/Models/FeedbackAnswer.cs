using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.Models
{
    public class FeedbackAnswer:BaseEntity
    {
        public long ApplicantFeedbackId { get; set; }
        public long FeedbackQuestionId { get; set; }
        public int rating { get; set; }

        //======
        public virtual ApplicantFeedback ApplicantFeedback { get; set; }
        public virtual FeedbackQuestion FeedbackQuestion { get; set; }
    }
}
