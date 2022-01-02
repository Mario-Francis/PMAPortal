using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PMAPortal.Web.Models
{
    public class Application:BaseEntity,IUpdatable
    {
        public long ApplicantId { get; set; }
        public long MeterId { get; set; }
        public long HouseTypeId { get; set; }
        public int RoomsCount { get; set; }
        public bool HasPets { get; set; }
        public string TrackNumber { get; set; }
        public long ApplicationStatusId { get; set; }
        public long? InstallerId { get; set; }
        public long? AssignedBy { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; } = DateTimeOffset.Now;

        //Navigation Properties
        [JsonIgnore]
        [NotMapped]
        public virtual User UpdatedByUser { get; set; }
        public virtual Applicant Applicant { get; set; }
        public virtual User Installer { get; set; }
        public virtual User AssignedByUser { get; set; }
        public virtual Meter Meter { get; set; }
        public virtual HouseType HouseType { get; set; }
        public virtual ApplicationStatus ApplicationStatus { get; set; }
        public virtual ICollection<ApplicationAppliance> ApplicationAppliances { get; set; }
        public virtual ICollection<ApplicationPet> ApplicationPets { get; set; }
        public virtual ICollection<ApplicationStatusLog> ApplicationStatusLogs { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<ApplicantFeedback> ApplicantFeedbacks { get; set; }

    }
}
