using PMAPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.ViewModels
{
    public class SurveyVM
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }

        public string ReadyToPay { get; set; }
        public string OccupierPhoneNumber { get; set; }
        public int? BedroomCount { get; set; }
        public string TypeOfApartment { get; set; }
        public string ExistingMeterType { get; set; }
        public string ExistingMeterNumber { get; set; }
        public string CustomerBillMatchUploadedData { get; set; }
        public decimal? EstimatedTotalLoadInAmps { get; set; }
        public string RecommendedMeterType { get; set; }
        public string InstallationMode { get; set; }
        public string LoadWireSeparationRequired { get; set; }
        public string AccountSeparationRequired { get; set; }
        public int? NumberOf1QRequired { get; set; }
        public int? NumberOf3QRequired { get; set; }
        public long? SurveyStaffId { get; set; }
        public long? AssignedById { get; set; }
        public DateTimeOffset? ScheduleDate { get; set; }
        public DateTimeOffset? SurveyDate { get; set; }
        public string SurveyRemark { get; set; } // meter ready || not meter ready
        public string MAP { get; set; }
        public string AdditionalComment { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public long? CreatedById { get; set; }
        public long? UpdatedById { get; set; }

        public string CreatedBy { get; set; }
        public string AssignedBy { get; set; }
        public string UpdatedBy { get; set; }
        public CustomerVM Customer { get; set; }
        public UserVM AssignedByUser { get; set; }
        public  UserVM UpdatedByUser { get; set; }
        public UserVM SurveyStaff { get; set; }

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

        public string FormattedSurveyDate
        {
            get
            {
                return SurveyDate?.ToString("MMM d, yyyy 'at' hh:mmtt");
            }
        }

        public Survey ToSurvey()
        {
            return new Survey
            {
                AccountSeparationRequired = AccountSeparationRequired,
                AdditionalComment = AdditionalComment,
                AssignedBy = AssignedById,
                BedroomCount = BedroomCount,
                CreatedBy = CreatedById,
                CreatedDate = DateTimeOffset.Now,
                CustomerBillMatchUploadedData = CustomerBillMatchUploadedData,
                CustomerId = CustomerId,
                EstimatedTotalLoadInAmps = EstimatedTotalLoadInAmps,
                ExistingMeterNumber = ExistingMeterNumber,
                ExistingMeterType = ExistingMeterType,
                InstallationMode = InstallationMode,
                LoadWireSeparationRequired = LoadWireSeparationRequired,
                MAP = MAP,
                NumberOf1QRequired = NumberOf1QRequired,
                NumberOf3QRequired = NumberOf3QRequired,
                OccupierPhoneNumber = OccupierPhoneNumber,
                ReadyToPay = ReadyToPay,
                RecommendedMeterType = RecommendedMeterType,
                ScheduleDate = ScheduleDate,
                SurveyDate = DateTimeOffset.Now,
                SurveyRemark = SurveyRemark,
                SurveyStaffId = SurveyStaffId,
                TypeOfApartment = TypeOfApartment,
                UpdatedDate = DateTimeOffset.Now
            };
        }

        public static SurveyVM FromSurvey(Survey survey, int? clientTimeOffset = null)
        {
            return new SurveyVM
            {
                Id = survey.Id,
                AccountSeparationRequired = survey.AccountSeparationRequired,
                AdditionalComment = survey.AdditionalComment,
                AssignedById = survey.AssignedBy,
                BedroomCount = survey.BedroomCount,
                CreatedById = survey.CreatedBy,
                CustomerBillMatchUploadedData = survey.CustomerBillMatchUploadedData,
                CustomerId = survey.CustomerId,
                EstimatedTotalLoadInAmps = survey.EstimatedTotalLoadInAmps,
                ExistingMeterNumber = survey.ExistingMeterNumber,
                ExistingMeterType = survey.ExistingMeterType,
                InstallationMode = survey.InstallationMode,
                LoadWireSeparationRequired = survey.LoadWireSeparationRequired,
                MAP = survey.MAP,
                NumberOf1QRequired = survey.NumberOf1QRequired,
                NumberOf3QRequired = survey.NumberOf3QRequired,
                OccupierPhoneNumber = survey.OccupierPhoneNumber,
                ReadyToPay = survey.ReadyToPay,
                RecommendedMeterType = survey.RecommendedMeterType,
                SurveyRemark = survey.SurveyRemark,
                SurveyStaffId = survey.SurveyStaffId,
                TypeOfApartment = survey.TypeOfApartment,
                UpdatedById = survey.UpdatedBy,
                CreatedBy = survey.CreatedByUser == null ? null : $"{survey.CreatedByUser.FirstName} {survey.CreatedByUser.LastName} ({survey.CreatedByUser.Email})",
                UpdatedBy = survey.UpdatedByUser == null ? null : $"{survey.UpdatedByUser.FirstName} {survey.UpdatedByUser.LastName} ({survey.UpdatedByUser.Email})",
                AssignedBy = survey.AssignedByUser == null ? null : $"{survey.AssignedByUser.FirstName} {survey.AssignedByUser.LastName} ({survey.AssignedByUser.Email})",
                CreatedDate = clientTimeOffset == null ? survey.CreatedDate : survey.CreatedDate.ToOffset(TimeSpan.FromMinutes(clientTimeOffset.Value)),
                UpdatedDate = clientTimeOffset == null ? survey.UpdatedDate : survey.UpdatedDate.ToOffset(TimeSpan.FromMinutes(clientTimeOffset.Value)),
                ScheduleDate = clientTimeOffset == null ? survey.ScheduleDate : survey.ScheduleDate?.ToOffset(TimeSpan.FromMinutes(clientTimeOffset.Value)),
                SurveyDate = clientTimeOffset == null ? survey.SurveyDate : survey.SurveyDate?.ToOffset(TimeSpan.FromMinutes(clientTimeOffset.Value)),
                AssignedByUser = UserVM.FromUser(survey.AssignedByUser),
                Customer = CustomerVM.FromCustomer(survey.Customer),
                SurveyStaff = UserVM.FromUser(survey.SurveyStaff),
                UpdatedByUser = UserVM.FromUser(survey.UpdatedByUser)
            };
        }
    }
}
