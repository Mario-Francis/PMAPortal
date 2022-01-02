using PMAPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.ViewModels
{
    public class ApplicationVM
    {
        public long Id { get; set; }
        // Applicant
        public long ApplicantId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        // Address
        public long AddressId { get; set; }
        [Required]
        public string Area { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string HouseNumber { get; set; }
        public string ApartmentNumber { get; set; }
        [Required]
        public string Landmark { get; set; }
        public string Description { get; set; }

       

        [Required]
        public long HouseTypeId { get; set; }
        [Required]
        [Range(1, 50)]
        public int RoomsCount { get; set; }

        // pets
        public bool HasPets { get; set; }
        //[Required]
        public IEnumerable<ItemCountVM> Pets { get; set; }

        // appliances
        [Required]
        public long MeterId { get; set; }
        [Required]
        public IEnumerable<ItemCountVM> Appliances { get; set; }
        public long? AssignedBy { get; set; }
        public long? InstallerId { get; set; }

        // display
        public string ApplicantFullName { get; set; }
        public string HouseType { get; set; }
        public string MeterName { get; set; }
        public PaymentVM Payment { get; set; }
        public ApplicantVM Applicant { get; set; }
        public string TrackNumber { get; set; }
        public long StatusId { get; set; }
        public string Status { get; set; }
        public string AssignedByUser { get; set; }
        public string Installer { get; set; }

        public IEnumerable<StatusLogVM> StatusLogs { get; set; }

        public string UpdatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
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


        public ApplicantAddress ToApplicantAddress()
        {
            return new ApplicantAddress
            {
                Id=AddressId,
                ApplicantId=ApplicantId,
                ApartmentNumber = string.IsNullOrEmpty(ApartmentNumber) ? null : ApartmentNumber,
                Area = Area,
                Description = string.IsNullOrEmpty(Description) ? null : Description,
                HouseNumber = HouseNumber,
                Landmark = Landmark,
                Street = Street,
                CreatedDate = DateTimeOffset.Now,
                UpdatedDate = DateTimeOffset.Now
            };
        }

        public Applicant ToApplicant()
        {
            return new Applicant
            {
                Id = ApplicantId,
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                PhoneNumber = PhoneNumber,
                CreatedDate = DateTimeOffset.Now,
                UpdatedDate = DateTimeOffset.Now
            };
        }

        public Application ToApplication()
        {
            return new Application
            {
                Id = Id,
                HasPets = HasPets,
                HouseTypeId = HouseTypeId,
                MeterId = MeterId,
                RoomsCount = RoomsCount,
                ApplicationStatusId = (int)ApplicationsStatus.Pending,
                CreatedDate = DateTimeOffset.Now,
                UpdatedDate = DateTimeOffset.Now
            };
        }

        public IEnumerable<ApplicationAppliance> ToApplicationAppliances()
        {
            if (Appliances == null)
            {
                return new List<ApplicationAppliance>();
            }
            else
            {
                return Appliances.Select(a => new ApplicationAppliance
                {
                    Appliance = a.Name,
                    Count = a.Count,
                    CreatedDate = DateTimeOffset.Now
                });
            }
        }

        public IEnumerable<ApplicationPet> ToApplicationPets()
        {
            if (Pets == null)
            {
                return new List<ApplicationPet>();
            }
            else
            {
                return Pets.Select(a => new ApplicationPet
                {
                    Pet = a.Name,
                    Count = a.Count,
                    CreatedDate = DateTimeOffset.Now
                });
            }
        }

        public static ApplicationVM FromApplication(Application application, int? clientTimeOffset = null)
        {
            var applicant = application.Applicant;
            var address = applicant.ApplicantAddresses.First();
            return new ApplicationVM
            {
                Id = application.Id,
                AddressId = address.Id,
                ApartmentNumber = address.ApartmentNumber,
                Appliances = application.ApplicationAppliances.Select(a => new ItemCountVM { Id = a.Id, Name = a.Appliance, Count = a.Count }),
                Applicant = ApplicantVM.FromApplicant(applicant),
                ApplicantFullName = $"{applicant.FirstName} {applicant.LastName}",
                ApplicantId = applicant.Id,
                Area = address.Area,
                Description = address.Description,
                Email = applicant.Email,
                FirstName = applicant.FirstName,
                HasPets = application.HasPets,
                HouseNumber = address.HouseNumber,
                HouseType = application.HouseType.Name,
                HouseTypeId = application.HouseTypeId,
                Landmark = address.Landmark,
                LastName = applicant.LastName,
                MeterId = application.MeterId,
                MeterName = application.Meter.Name,
                Payment = PaymentVM.FromPayment(application.Payments.First()),
                Pets = application.ApplicationPets.Select(p => new ItemCountVM { Id = p.Id, Name = p.Pet, Count = p.Count }),
                PhoneNumber = applicant.PhoneNumber,
                RoomsCount = application.RoomsCount,
                Street = address.Street,
                TrackNumber = application.TrackNumber,
                StatusId = application.ApplicationStatusId,
                Status = application.ApplicationStatus.Name,
                CreatedDate = clientTimeOffset == null ? application.CreatedDate : application.CreatedDate.ToOffset(TimeSpan.FromMinutes(clientTimeOffset.Value)),
                UpdatedBy = application.UpdatedByUser == null ? null : $"{application.UpdatedByUser.FirstName} {application.UpdatedByUser.LastName} ({application.UpdatedByUser.Email})",
                UpdatedDate = clientTimeOffset == null ? application.UpdatedDate : application.UpdatedDate.ToOffset(TimeSpan.FromMinutes(clientTimeOffset.Value)),
                StatusLogs = application.ApplicationStatusLogs.Select(l => StatusLogVM.FromApplicationStatusLog(l, clientTimeOffset)),
                AssignedByUser = application.AssignedByUser == null ? null : $"{application.AssignedByUser.FirstName} {application.AssignedByUser.LastName} ({application.AssignedByUser.Email})",
                AssignedBy=application.AssignedBy,
                Installer = application.Installer == null ? null : $"{application.Installer.FirstName} {application.Installer.LastName} ({application.Installer.Email})",
                InstallerId = application.InstallerId
            };
        }
    }
}
