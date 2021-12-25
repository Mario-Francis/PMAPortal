using PMAPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.ViewModels
{
    public class AddressVM
    {
        public long Id { get; set; }
        public long ApplicantId { get; set; }
        public string Area { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string ApartmentNumber { get; set; }
        public string Landmark { get; set; }
        public string Description { get; set; }

        public string FormattedAddress { get
            {
                return $"{HouseNumber}, {Street}, {Landmark}, {Area}.";
            } }

        public static AddressVM FromApplicantAddress(ApplicantAddress address)
        {
            return new AddressVM
            {
                Id = address.Id,
                ApplicantId = address.ApplicantId,
                ApartmentNumber = address.ApartmentNumber,
                Area = address.Area,
                Description = address.Description,
                HouseNumber = address.HouseNumber,
                Landmark = address.Landmark,
                Street = address.Street
            };
        }
    }
}
