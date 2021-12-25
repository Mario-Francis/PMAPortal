using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.Models
{
    public class Role:BaseEntity
    {
        public string Name
        {
            get; set;
        }

        // navigation properties
        public virtual ICollection<User> Users { get; set; }
    }
}
