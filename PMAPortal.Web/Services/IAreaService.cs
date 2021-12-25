using PMAPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.Services
{
    public interface IAreaService
    {
        Task CreateArea(Area area);
        Task DeleteArea(long areaId);
        IEnumerable<Area> GetAreas();
        Task<Area> GetArea(long id);
    }
}
