using PMAPortal.Web.Extensions;
using PMAPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.ViewModels
{
    public class ApplicationItemVM
    {
        public long Id { get; set; }
        public string Applicant { get; set; }
        public string Area { get; set; }
        public string PhoneNumber { get; set; }
        public string Meter { get; set; }
        public string TrackNumber { get; set; }
        public string AmountPaid { get; set; }
        public string Status { get; set; }
        public long StatusId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public long? InstallerId { get; set; }
        public string Installer { get; set; }
        public string AssignedBy { get; set; }

        public string FormattedCreatedDate { get
            {
                return CreatedDate.ToString("MMM d, yyyy 'at' hh:mmtt");
            }
        }
        public string FormattedUpdatedDate
        {
            get
            {
                return UpdatedDate.ToString("MMM d, yyyy 'at' hh:mmtt");
            }
        }

        public static ApplicationItemVM FromApplication(Application application, int? clientTimeOffset = null)
        {
            if (application == null)
            {
                return null;
            }
            else
            {
                var applicant = application.Applicant;
                return new ApplicationItemVM
                {
                    Id = application.Id,
                    Applicant = $"{applicant.FirstName} {applicant.LastName} ({applicant.Email})",
                    Area = applicant.ApplicantAddresses.First().Area,
                    PhoneNumber = applicant.PhoneNumber,
                    Meter = application.Meter.Name,
                    TrackNumber = application.TrackNumber,
                    AmountPaid = application.Payments.First(p => p.IsPaid).Amount.Format(),
                    StatusId = application.ApplicationStatusId,
                    Status = application.ApplicationStatus.Name,
                    CreatedDate = clientTimeOffset == null ? application.CreatedDate : application.CreatedDate.ToOffset(TimeSpan.FromMinutes(clientTimeOffset.Value)),
                    UpdatedBy = application.UpdatedByUser == null ? null : $"{application.UpdatedByUser.FirstName} {application.UpdatedByUser.LastName} ({application.UpdatedByUser.Email})",
                    UpdatedDate = clientTimeOffset == null ? application.UpdatedDate : application.UpdatedDate.ToOffset(TimeSpan.FromMinutes(clientTimeOffset.Value)),
                    AssignedBy = application.AssignedByUser == null ? null : $"{application.AssignedByUser.FirstName} {application.AssignedByUser.LastName} ({application.AssignedByUser.Email})",
                    Installer = application.Installer == null ? null : $"{application.Installer.FirstName} {application.Installer.LastName} ({application.Installer.Email})",
                    InstallerId = application.InstallerId
                };
            }
        }
    }
}
