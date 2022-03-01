using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PMAPortal.Web.Models
{
    public class Survey:BaseEntity, IUpdatable
    {
        public long CustomerId { get; set; }
       
        public string ReadyToPay { get; set; }
        public string OccupierPhoneNumber { get; set; }
        public string TypeOfApartment { get; set; }
        public string ExistingMeterType { get; set; }
        public string ExistingMeterNumber { get; set; }
        public string CustomerBillMatchUploadedData { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal? EstimatedTotalLoadInAmps { get; set; }
        public string RecommendedMeterType { get; set; }
        public string InstallationMode { get; set; }
        public string LoadWireSeparationRequired{ get; set; }
        public string AccountSeparationRequired { get; set; }
        public int? NumberOf1QRequired { get; set; }
        public int? NumberOf3QRequired { get; set; }
        public string SurveryCompany { get; set; }
        public long? SurveyStaffId { get; set; }
        public long? AssignedBy { get; set; }
        public DateTimeOffset? ScheduleDate { get; set; }
        public DateTimeOffset SurveyDate { get; set; }
        public string SurveyRemark { get; set; } // meter ready || not meter ready
        public string MAP { get; set; }
        public string AdditionalComment { get; set; }
        public string LocationFrontViewImagePath { get; set; }
        public string MeterPointBeforeInstallationImagePath { get; set; }
        public string CustomerBillImagePath { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; } = DateTimeOffset.Now;


        // naviagtion
        [JsonIgnore]
        [NotMapped]
        public virtual User UpdatedByUser { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual User SurveyStaff { get; set; }
        public virtual User AssignedByUser { get; set; }
    }
}
