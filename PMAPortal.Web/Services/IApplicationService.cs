using PMAPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.Services
{
    public interface IApplicationService
    {
        Task<(string trackNo, long applicationId)> AddApplication(Application application, Applicant applicant, ApplicantAddress address, IEnumerable<ApplicationAppliance> appliances, IEnumerable<ApplicationPet> pets);
        Task UpdateApplicationStatus(long applicationId, long statusId, string comment = null);
        Task<Applicant> GetApplicant(string email);
        Task ScheduleNewApplicationMail(long applicationId);
        Task<(bool exist, Applicant applicant)> ApplicantExists(string email);
        Task UpdateAddress(ApplicantAddress address);
        Task UpdateApplicant(Applicant applicant);

        //===================================
        IEnumerable<Application> GetApplications();
        IEnumerable<Application> GetApplications(long[] statuses);
        Task<Application> GetApplication(long id);
        Task<Application> GetApplication(string trackNumber);
    }
}
