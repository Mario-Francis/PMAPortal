using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PMAPortal.Web.Models
{
    public class Payment:BaseEntity,IUpdatable
    {
        public long ApplicationId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; }
        public DateTimeOffset DatePaid { get; set; }

        public long? UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; } = DateTimeOffset.Now;

        //Navigation Properties
        [JsonIgnore]
        [NotMapped]
        public virtual User UpdatedByUser { get; set; }
        public virtual Application Application { get; set; }
        public virtual ICollection<PaymentLog> PaymentLogs { get; set; }
    }
}
