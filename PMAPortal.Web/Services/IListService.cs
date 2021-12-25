using PMAPortal.Web.Models;
using System.Collections.Generic;

namespace PMAPortal.Web.Services
{
    public interface IListService
    {
        IEnumerable<Appliance> GetAppliances();
        IEnumerable<Area> GetAreas();
        IEnumerable<HouseType> GetHouseTypes();
        IEnumerable<Role> GetRoles();
        IEnumerable<Meter> GetMeters();
        IEnumerable<Pet> GetPets();
        IEnumerable<ApplicationStatus> GetApplicationStatuses();
    }
}
