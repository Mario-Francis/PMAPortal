using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.Models
{
    public interface IUpdatable
    {
        public long? UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }

        // Navigation
        public User UpdatedByUser { get; set; }
    }
}
