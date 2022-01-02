using System.Collections.Generic;

namespace PMAPortal.Web.DTOs
{
    public class MailObject
    {
        public IEnumerable<Recipient> Recipients { get; set; }
        public string ApplicantName { get; set; }
        public string ApplicantPhoneNo { get; set; }
        public string ApplicantEmail { get; set; }
        public string MeterType { get; set; }
        public string TrackNo { get; set; }
        public string ApplicationStatus { get; set; }
        public string Comment { get; set; }

        public string AssignedByName { get; set; }
        public string AssignedByEmail { get; set; }
    }
}
