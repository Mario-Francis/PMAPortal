using Microsoft.AspNetCore.Http;
using PMAPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMAPortal.Web.Services
{
    public interface IInstallationBatchService
    {
        Task AddInstallationBatch(IFormFile file, DateTimeOffset? dateShared = null);
        Task<InstallationBatch> GetBatch(long id);
        IEnumerable<InstallationBatch> GetBatches();
        Task DeleteBatch(long id);
    }
}
