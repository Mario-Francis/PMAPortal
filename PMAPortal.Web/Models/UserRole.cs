using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.Models
{
    public class UserRole:BaseEntity
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }

        // navigation
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }

    }
}
