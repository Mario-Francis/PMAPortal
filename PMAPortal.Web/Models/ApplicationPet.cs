using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.Models
{
    public class ApplicationPet:BaseEntity
    {
        public long ApplicationId { get; set; }
        public string Pet { get; set; }
        public int Count { get; set; }

        // navigation properties
        public virtual Application Application { get; set; }
    }
}
