using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PMAPortal.Web.Models
{
    public class Installation:BaseEntity, IUpdatable
    {
        public long CustomerId { get; set; }
        public long InstallationStatusId { get; set; }
        public long? InstallerId { get; set; }
        public long? AssignedBy { get; set; }
        public string MeterType { get; set; }
        public string MeterNumber { get; set; }
        public string MeterNamePlateImagePath { get; set; }
        public string InProgessImagePath { get; set; }
        public string MeterSealImagePath { get; set; }
        public string CompleteWithSealImagePath { get; set; }
        public string MeterCardImagePath { get; set; }
        public string SupplyCableVisibleToMeterImagePath { get; set; }
        public string SupplyCableToThePremisesImagePath { get; set; }
        public string RetrievedMeterImagePath { get; set; }
        public string MonitorShowingRemainingUnitImagePath { get; set; }

        public long? UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; } = DateTimeOffset.Now;


        // navigation
        public virtual Customer Customer { get; set; }
        [JsonIgnore]
        [NotMapped]
        public virtual User UpdatedByUser { get; set; }
        public virtual User Installer { get; set; }
        public virtual User AssignedByUser { get; set; }
        public virtual InstallationStatus InstallationStatus { get; set; }
    }
}
