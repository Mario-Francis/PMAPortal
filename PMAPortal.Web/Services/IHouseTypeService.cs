using PMAPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.Services
{
    public interface IHouseTypeService
    {
        Task CreateHouseType(HouseType houseType);
        Task DeleteHouseType(long houseTypeId);
        Task UpdateHouseType(HouseType houseType);
        IEnumerable<HouseType> GetHouseTypes();
        Task<HouseType> GetHouseType(long id);
    }
}
