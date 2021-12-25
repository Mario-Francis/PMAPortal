using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PMAPortal.Web.Models
{
    public class BaseEntity
    {
        [Key]
        public long Id { get; set; }
        public long? CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.Now;

        //Navigation Properties
        [JsonIgnore]
        [ForeignKey("CreatedBy")]
        [NotMapped]
        public virtual User CreatedByUser { get; set; }

        public T Clone<T>() where T : BaseEntity
        {
            return (T)this.MemberwiseClone();
        }
    }
}
