using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.DTOs
{
    public class AppSettings
    {
        public int MaxUploadSize { get; set; }
        public int DefaultTimeZoneOffset { get; set; }
        public int ReminderServiceExecutionInterval { get; set; }
        public EmailSMTPConfig EmailSMTPConfig { get; set; }
        public int MailRetentionPeriod { get; set; }
        public Paystack Paystack { get; set; }
    }
}
