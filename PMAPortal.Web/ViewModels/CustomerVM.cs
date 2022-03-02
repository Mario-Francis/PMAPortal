using PMAPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.ViewModels
{
    public class CustomerVM
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
                return DateShared?.ToString("MMM d, yyyy 'at' hh:mmtt");
            }
        }
         
        public static CustomerVM FromCustomer(Customer customer, int? clientTimeOffset = null)
        {
            return new CustomerVM
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
        }

        public static string GetSurveyStatus(Customer customer)
        {
            var status = "Pending";
            if(customer.Surveys.Count() > 0)
            {
                var survey = customer.Surveys.First();
                if(survey.SurveyStaffId!=null && survey.ScheduleDate==null && survey.SurveyRemark == null)
                {
                    status = "Assigned";
                }else if(survey.ScheduleDate != null && survey.SurveyRemark == null)
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
