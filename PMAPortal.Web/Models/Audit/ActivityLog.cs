using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.Models.Audit
{
    public class ActivityLog:BaseEntity
    {
        public string ActionType { get; set; }
        public string ActionBy { get; set; }
        public string ActionByType { get; set; }
        public string Description { get; set; }
        public string IPAddress { get; set; }

        // Navigation property
        public virtual AuditLog AuditLog { get; set; }
    }
}
