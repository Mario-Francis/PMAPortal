using PMAPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.ViewModels
{
    public class ItemVM
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTimeOffset CreatedDate { get; set; }

        public string FormattedCreatedDate
        {
            get
            {
                return CreatedDate.ToString("MMM d, yyyy 'at' hh:mmtt");
            }
        }

        public static ItemVM FromHouseType(HouseType houseType, int? clientTimeOffset = null)
        {
            return new ItemVM
            {
                Id = houseType.Id,
                Name = houseType.Name,
                CreatedDate = clientTimeOffset == null ? houseType.CreatedDate : houseType.CreatedDate.ToOffset(TimeSpan.FromMinutes(clientTimeOffset.Value))
            };
        }

        public static ItemVM FromArea(Area area, int? clientTimeOffset = null)
        {
            return new ItemVM
            {
                Id = area.Id,
                Name = area.Name,
                CreatedDate = clientTimeOffset == null ? area.CreatedDate : area.CreatedDate.ToOffset(TimeSpan.FromMinutes(clientTimeOffset.Value))
            };
        }

        public static ItemVM FromAppliance(Appliance appliance, int? clientTimeOffset = null)
        {
            return new ItemVM
            {
                Id = appliance.Id,
                Name = appliance.Name,
                CreatedDate = clientTimeOffset == null ? appliance.CreatedDate : appliance.CreatedDate.ToOffset(TimeSpan.FromMinutes(clientTimeOffset.Value))
            };
        }

        public static ItemVM FromPet(Pet pet, int? clientTimeOffset = null)
        {
            return new ItemVM
            {
                Id = pet.Id,
                Name = pet.Name,
                CreatedDate = clientTimeOffset == null ? pet.CreatedDate : pet.CreatedDate.ToOffset(TimeSpan.FromMinutes(clientTimeOffset.Value))
            };
        }
    }
}
