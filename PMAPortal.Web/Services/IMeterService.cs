using PMAPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.Services
{
    public interface IMeterService
    {
        Task CreateMeter(Meter meter);
        Task DeleteMeter(long meterId);
        Task UpdateMeter(Meter meter);
        IEnumerable<Meter> GetMeters();
        Task<Meter> GetMeter(long id);
    }
}
