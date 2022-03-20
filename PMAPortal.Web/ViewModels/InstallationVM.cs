using PMAPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.ViewModels
{
    public class InstallationVM
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public long? SurveyId { get; set; }
        public long InstallationStatusId { get; set; }
        public DateTimeOffset? ScheduleDate { get; set; }
        public long? InstallerId { get; set; }
        public long? AssignedById { get; set; }
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

        public DateTimeOffset UpdatedDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public long? CreatedById { get; set; }
        public long? UpdatedById { get; set; }

        public string CreatedBy { get; set; }
        public string AssignedBy { get; set; }
        public string UpdatedBy { get; set; }

        public virtual CustomerVM Customer { get; set; }
        public virtual UserVM UpdatedByUser { get; set; }
        public virtual UserVM Installer { get; set; }
        public virtual SurveyVM Survey { get; set; }
        public virtual UserVM AssignedByUser { get; set; }
        public virtual string InstallationStatus { get; set; }
        public virtual IEnumerable<InstallationLogVM> InstallationLogs { get; set; }

        public string FormattedCreatedDate
        {
            get
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
        public string FormattedScheduleDate
        {
            get
            {
                return ScheduleDate?.ToString("MMM d, yyyy 'at' hh:mmtt");
            }
        }

        public static InstallationVM FromInstallation(Installation installation, int? clientTimeOffset = null)
        {
            return new InstallationVM
            {
                Id = installation.Id,
                AssignedById=installation.AssignedBy,
                CompleteWithSealImagePath=installation.CompleteWithSealImagePath,
                CustomerBillImagePath=installation.CustomerBillImagePath,
                InProgessImagePath=installation.InProgessImagePath,
                InstallationLogs=installation.InstallationLogs.Select(l=> InstallationLogVM.FromInstallationLog(l)),
                InstallationStatus=installation.InstallationStatus.Name,
                InstallationStatusId=installation.InstallationStatusId,
                InstallerId=installation.InstallerId,
                LocationFrontViewImagePath=installation.LocationFrontViewImagePath,
                MeterCardImagePath=installation.MeterCardImagePath,
                MeterNamePlateImagePath=installation.MeterNamePlateImagePath,
                MeterNumber=installation.MeterNumber,
                MeterPointBeforeInstallationImagePath=installation.MeterPointBeforeInstallationImagePath,
                MeterSealImagePath=installation.MeterSealImagePath,
                MeterType=installation.MeterType,
                MonitorShowingRemainingUnitImagePath=installation.MonitorShowingRemainingUnitImagePath,
                RetrievedMeterImagePath=installation.RetrievedMeterImagePath,
                SupplyCableToThePremisesImagePath=installation.SupplyCableToThePremisesImagePath,
                SupplyCableVisibleToMeterImagePath=installation.SupplyCableVisibleToMeterImagePath,
                SurveyId=installation.SurveyId,
                Survey=SurveyVM.FromSurvey(installation.Survey),
                CreatedById = installation.CreatedBy,
                CustomerId = installation.CustomerId,
                UpdatedById = installation.UpdatedBy,
                CreatedBy = installation.CreatedByUser == null ? null : $"{installation.CreatedByUser.FirstName} {installation.CreatedByUser.LastName} ({installation.CreatedByUser.Email})",
                UpdatedBy = installation.UpdatedByUser == null ? null : $"{installation.UpdatedByUser.FirstName} {installation.UpdatedByUser.LastName} ({installation.UpdatedByUser.Email})",
                AssignedBy = installation.AssignedByUser == null ? null : $"{installation.AssignedByUser.FirstName} {installation.AssignedByUser.LastName} ({installation.AssignedByUser.Email})",
                CreatedDate = clientTimeOffset == null ? installation.CreatedDate : installation.CreatedDate.ToOffset(TimeSpan.FromMinutes(clientTimeOffset.Value)),
                UpdatedDate = clientTimeOffset == null ? installation.UpdatedDate : installation.UpdatedDate.ToOffset(TimeSpan.FromMinutes(clientTimeOffset.Value)),
                ScheduleDate = clientTimeOffset == null ? installation.ScheduleDate : installation.ScheduleDate?.ToOffset(TimeSpan.FromMinutes(clientTimeOffset.Value)),
                AssignedByUser = UserVM.FromUser(installation.AssignedByUser),
                Customer = CustomerVM.FromCustomer(installation.Customer),
                Installer = UserVM.FromUser(installation.Installer),
                UpdatedByUser = UserVM.FromUser(installation.UpdatedByUser)
            };
        }


    }
}
