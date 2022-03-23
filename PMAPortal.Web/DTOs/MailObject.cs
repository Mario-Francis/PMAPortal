using System.Collections.Generic;

namespace PMAPortal.Web.DTOs
{
    public class MailObject
    {
        public IEnumerable<Recipient> Recipients { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhoneNo { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAccountNumber { get; set; }
        public string MeterType { get; set; }
        public string MeterNo { get; set; }
        public string InstallationStatus { get; set; }
        public string Comment { get; set; }
        public string ScheduleDate { get; set; }

        public string AssignedByName { get; set; }
        public string AssignedByEmail { get; set; }

        public string SurveyStaffName { get; set; }
        public string SurveyStaffEmail { get; set; }
        public string SurveyStaffPhoneNo { get; set; }

        public string InstallerName { get; set; }
        public string InstallerEmail { get; set; }
        public string InstallerPhoneNo { get; set; }
    }
}
