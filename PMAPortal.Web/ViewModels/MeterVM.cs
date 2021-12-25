using PMAPortal.Web.Extensions;
using PMAPortal.Web.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace PMAPortal.Web.ViewModels
{
    public class MeterVM
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }

        public string FormattedAmount
        {
            get
            {
                return Amount.Format();
            }
        }
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

        public Meter ToMeter()
        {
            return new Meter
            {
                Id = Id,
                Name = Name,
                Amount=Amount,
                Description = Description,
                CreatedDate = DateTimeOffset.Now,
                UpdatedDate = DateTimeOffset.Now
            };
        }

        public static MeterVM FromMeter(Meter meter, int? clientTimeOffset = null)
        {
            return new MeterVM
            {
                Id = meter.Id,
                Amount= meter.Amount,
                Name = meter.Name,
                Description = meter.Description ?? "",
                UpdatedBy = meter.UpdatedByUser == null ? null : $"{meter.UpdatedByUser.FirstName} {meter.UpdatedByUser.LastName} ({meter.UpdatedByUser.Email})",
                CreatedDate = clientTimeOffset == null ? meter.CreatedDate : meter.CreatedDate.ToOffset(TimeSpan.FromMinutes(clientTimeOffset.Value)),
                UpdatedDate = clientTimeOffset == null ? meter.UpdatedDate : meter.UpdatedDate.ToOffset(TimeSpan.FromMinutes(clientTimeOffset.Value)),
            };
        }
    }
}
