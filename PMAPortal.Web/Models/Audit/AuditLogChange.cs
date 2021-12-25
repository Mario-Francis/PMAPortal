using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.Models.Audit
{
    public class AuditLogChange:BaseEntity
    {
        public long AuditLogId { get; set; }
        public string ColumnName { get; set; }
        public string From { get; set; }
        public string To { get; set; }

        // Navigation property
        public virtual AuditLog AuditLog { get; set; }
    }
}
