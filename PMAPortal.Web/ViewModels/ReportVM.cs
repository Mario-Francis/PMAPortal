using PMAPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.ViewModels
{
    public class ReportVM
    {
        public long Id { get; set; }
        public long BatchId { get; set; }
        public string SN { get; set; }
        public DateTimeOffset? DateShared { get; set; }
        public string BatchNumber { get; set; }
        public string AccountNumber { get; set; }
        public string ARN { get; set; }
        public string CustomerName { get; set; }
        public string CISName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string CISAddress { get; set; }
        public string Landmark { get; set; }
        public string BU { get; set; }
        public string UT { get; set; }
        public string Feeder { get; set; }
        public string DT { get; set; }
        public string Tariff { get; set; }
        public string MeteredStatus { get; set; }

        public string SurveyStatus { get; set; } // pending | Meter ready | Not Meter Ready
        public string InstallationStatus { get; set; } // Not Eligible | Pending | Assigned | Scheduled | In-Progress | Rejected | Completed | Disco Rejected | Disco Approved

        // if  assigned
        public long? SurveyId { get; set; }
        public long? SurveyStaffId { get; set; }
        public long? AssignedById { get; set; }
        public string AssignedBy { get; set; } = "";
        public string SurveyStaff { get; set; } = "";
        public string SurveyCompany { get; set; } = "";

        public string SurveyRemark { get; set; } = "";
        public DateTimeOffset? ScheduleDate { get; set; }
        public DateTimeOffset? SurveyDate { get; set; }
        //============
        // for installation
        public long? InstallationId { get; set; }
        public long? InstallerId { get; set; }
        public long? InstallationStatusId { get; set; }
        public long? IAssignedById { get; set; }
        public string IAssignedBy { get; set; } = "";
        public string Installer { get; set; } = "";
        public string InstallerCompany { get; set; } = "";
        public DateTimeOffset? IScheduleDate { get; set; }
        public string MeterType { get; set; } = "";
        public string MeterNumber { get; set; } = "";
        public string Comment { get; set; }
        //===========

        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string FormattedCreatedDate
        {
            get
            {
                return CreatedDate.ToString("MMM d, yyyy 'at' hh:mmtt");
            }
        }
        public string FormattedDateShared
        {
            get
            {
                return DateShared?.ToString("MMM d, yyyy 'at' hh:mmtt") ?? "";
            }
        }
        public string FormattedScheduleDate
        {
            get
            {
                return ScheduleDate?.ToString("MMM d, yyyy 'at' hh:mmtt") ?? "";
            }
        }
        public string FormattedSurveyDate
        {
            get
            {
                return SurveyDate?.ToString("MMM d, yyyy 'at' hh:mmtt") ?? "";
            }
        }
        public string FormattedIScheduleDate
        {
            get
            {
                return IScheduleDate?.ToString("MMM d, yyyy 'at' hh:mmtt") ?? "";
            }
        }

        public static ReportVM FromCustomer(Customer customer, int? clientTimeOffset = null)
        {
            var _customer = new ReportVM
            {
                Id = customer.Id,
                AccountNumber = customer.AccountNumber,
                Address = customer.Address,
                ARN = customer.ARN,
                BatchId = customer.BatchId,
                BatchNumber = customer.BatchNumber,
                BU = customer.BU,
                CISAddress = customer.CISAddress,
                CISName = customer.CISName,
                CustomerName = customer.CustomerName,
                DT = customer.DT,
                Email = customer.Email,
                Feeder = customer.Feeder,
                Landmark = customer.Landmark,
                MeteredStatus = customer.MeteredStatus,
                PhoneNumber = customer.PhoneNumber,
                SN = customer.SN,
                Tariff = customer.Tariff,
                UT = customer.UT,
                CreatedBy = customer.CreatedByUser == null ? null : $"{customer.CreatedByUser.FirstName} {customer.CreatedByUser.LastName} ({customer.CreatedByUser.Email})",
                CreatedDate = clientTimeOffset == null ? customer.CreatedDate : customer.CreatedDate.ToOffset(TimeSpan.FromMinutes(clientTimeOffset.Value)),
                DateShared = clientTimeOffset == null ? customer.DateShared : customer.DateShared?.ToOffset(TimeSpan.FromMinutes(clientTimeOffset.Value)),
                SurveyStatus = GetSurveyStatus(customer),
                InstallationStatus = GetInstallationStatus(customer)
            };

            if (customer.Surveys.Count() > 0)
            {
                var survey = customer.Surveys.First();
                _customer.SurveyId = survey.Id;
                _customer.SurveyStaffId = survey.SurveyStaffId;
                _customer.AssignedById = survey.AssignedBy;
                _customer.SurveyRemark = survey.SurveyRemark ?? "";
                _customer.SurveyCompany = survey.SurveryCompany ?? "";
                _customer.AssignedBy = survey.AssignedByUser == null ? "" : $"{survey.AssignedByUser.FirstName} {survey.AssignedByUser.LastName} ({survey.AssignedByUser.Email})";
                _customer.SurveyStaff = survey.SurveyStaff == null ? "" : $"{survey.SurveyStaff.FirstName} {survey.SurveyStaff.LastName} ({survey.SurveyStaff.Email})";
                _customer.ScheduleDate = clientTimeOffset == null ? survey.ScheduleDate : survey.ScheduleDate?.ToOffset(TimeSpan.FromMinutes(clientTimeOffset.Value));
                _customer.SurveyDate = clientTimeOffset == null ? survey.SurveyDate : survey.SurveyDate?.ToOffset(TimeSpan.FromMinutes(clientTimeOffset.Value));
            }

            if (customer.Installations.Count() > 0)
            {
                var installation = customer.Installations.First();
                _customer.InstallationStatusId = installation.InstallationStatusId;
                _customer.InstallationId = installation.Id;
                _customer.InstallerId = installation.InstallerId;
                _customer.IAssignedById = installation.AssignedBy;
                _customer.MeterType = installation.MeterType ?? "";
                _customer.MeterNumber = installation.MeterNumber ?? "";
                _customer.IAssignedBy = installation.AssignedByUser == null ? "" : $"{installation.AssignedByUser.FirstName} {installation.AssignedByUser.LastName} ({installation.AssignedByUser.Email})";
                _customer.Installer = installation.Installer == null ? "" : $"{installation.Installer.FirstName} {installation.Installer.LastName} ({installation.Installer.Email})";
                _customer.InstallerCompany = installation.Installer == null ? "" : $"{installation.Installer.CompanyName}";
                _customer.IScheduleDate = clientTimeOffset == null ? installation.ScheduleDate : installation.ScheduleDate?.ToOffset(TimeSpan.FromMinutes(clientTimeOffset.Value));


                if (installation.InstallationLogs.Count() > 0)
                {
                    var log = installation.InstallationLogs.OrderByDescending(l => l.Id).First();
                    _customer.Comment = log.Comment;
                }
            }


            return _customer;
        }

        public static string GetSurveyStatus(Customer customer)
        {
            var status = "Pending";
            if (customer.Surveys.Count() > 0)
            {
                var survey = customer.Surveys.First();
                if (survey.SurveyStaffId != null && survey.ScheduleDate == null && survey.SurveyRemark == null)
                {
                    status = "Assigned";
                }
                else if (survey.ScheduleDate != null && survey.SurveyRemark == null)
                {
                    status = "Scheduled";
                }
                else
                {
                    status = survey.SurveyRemark;
                }
            }
            return status;
        }

        public static string GetInstallationStatus(Customer customer)
        {
            var status = "Not Eligible";
            if (customer.Installations.Count() > 0)
            {
                var installation = customer.Installations.First();
                status = installation.InstallationStatus.Name;
            }
            return status;
        }
    }
}

