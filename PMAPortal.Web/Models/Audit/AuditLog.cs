using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.Models.Audit
{
    public class AuditLog:BaseEntity
    {
        public long ActivityLogId { get; set; }
        public long? ItemId { get; set; } // record id
        public string TableName { get; set; }


        public virtual ActivityLog ActivityLog { get; set; }
        public virtual ICollection<AuditLogChange> AuditLogChanges { get; set; }
    }
}
