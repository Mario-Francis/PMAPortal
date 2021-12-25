using PMAPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.Services
{
    public interface IApplianceService
    {
        Task CreateAppliance(Appliance appliance);
        Task DeleteAppliance(long applianceId);
        IEnumerable<Appliance> GetAppliances();
        Task<Appliance> GetAppliance(long id);
    }
}
