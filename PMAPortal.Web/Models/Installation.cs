using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PMAPortal.Web.Models
{
    public class Installation:BaseEntity, IUpdatable
    {
        public long CustomerId { get; set; }
        public long? SurveyId { get; set; }
        public long InstallationStatusId { get; set; }
        public DateTimeOffset? ScheduleDate { get; set; }
        public long? InstallerId { get; set; }
        public long? AssignedBy { get; set; }
        public string MeterType { get; set; }
        public string MeterNumber { get; set; }
        public string LocationFrontViewImagePath { get; set; }
        public string MeterPointBeforeInstallationImagePath { get; set; }
        public string CustomerBillImagePath { get; set; }
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
        public virtual Survey Survey { get; set; }
        public virtual ICollection<InstallationLog> InstallationLogs { get; set; }
        public virtual ICollection<CustomerFeedback> CustomerFeedbacks { get; set; }
    }
}
