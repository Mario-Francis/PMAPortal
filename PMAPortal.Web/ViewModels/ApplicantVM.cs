using PMAPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.ViewModels
{
    public class ApplicantVM
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public AddressVM Address { get; set; }

        public static ApplicantVM FromApplicant(Applicant applicant)
        {
            
            return applicant==null?null: new ApplicantVM
            {
                Id = applicant.Id,
                FirstName = applicant.FirstName,
                LastName = applicant.LastName,
                Email = applicant.Email,
                PhoneNumber = applicant.PhoneNumber,
                Address = AddressVM.FromApplicantAddress(applicant.ApplicantAddresses.First())
            };
        }
    }
}
