using Microsoft.AspNetCore.Http;
using PMAPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.Services
{
    public interface IInstallationService
    {
        public IEnumerable<Customer> GetUnassigned();
        IEnumerable<Customer> GetAssigned();
        IEnumerable<Installation> GetCompleted();
        IEnumerable<Installation> GetDiscoApproved();
        IEnumerable<Installation> GetRejected();
        IEnumerable<Installation> GetDiscoRejected();
        Task AssignInstallation(long installerId, long installationId);
        Task ReassignInstallation(long installerId, long installationId);
        Task AssignInstallations(long installerId, IEnumerable<long> installationIds);
        Task ReassignInstallations(long installerId, IEnumerable<long> installationIds);
        Task UnassignInstallation(long installationId);
        Task UnassignInstallations(IEnumerable<long> installationIds);
        Task ScheduleInstallation(long installationId, DateTimeOffset scheduleDate);
        Task ScheduleInstallations(IEnumerable<long> installationIds, DateTimeOffset scheduleDate);
        IEnumerable<Installation> GetPendingInstaller(long installerId);
        IEnumerable<Installation> GetCompletedByInstaller(long installerId);
        IEnumerable<Installation> GetRejetcedByInstaller(long installerId);
        IEnumerable<Installation> GetDiscoRejected(long installerId);
        IEnumerable<Installation> GetDiscoApproved(long installerId);
        Task<Installation> GetInstallation(long installationId);
        Task UpdateMeterInfo(long installationId, string meterType, string meterNo);
        Task<string> UploadImage(long installationId, string imageFieldName, IFormFile file);
        Task DeleteImage(long installationId, string imageFieldName);
        Task CompleteInstallation(long installationId, string comment = null);
        Task AddInstallationLog(long installationId, long? actionBy, long statusId, string description = null, string comment = null, bool save = true);
        Task RejectInstallation(long installationId, string comment);
        Task DiscoApproveInstallation(long installationId, string comment = null);
        Task DiscoRejectInstallation(long installationId, string comment);
    }
}
