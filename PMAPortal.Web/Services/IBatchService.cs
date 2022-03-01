using Microsoft.AspNetCore.Http;
using PMAPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.Services
{
    public interface IBatchService
    {
        Task AddCustomerBatch(IFormFile file, DateTimeOffset? dateShared = null);
        Task<Batch> GetBatch(long id);
        IEnumerable<Batch> GetBatches();
        Task DeleteBatch(long id);
    }
}
