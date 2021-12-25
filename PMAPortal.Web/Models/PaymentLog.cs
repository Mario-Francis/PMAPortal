using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.Models
{
    public class PaymentLog:BaseEntity
    {
        public long PaymentId { get; set; }
        public string PaymentRef { get; set; }
        public bool IsSuccess { get; set; }

        // navigation properties
        public virtual Payment Payment { get; set; }
    }
}
