using Microsoft.AspNetCore.Http;
using PMAPortal.Web.Data.Repositories;
using PMAPortal.Web.Extensions;
using PMAPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.Services.Implementations
{
    public class ApplianceService:IApplianceService
    {
        private readonly IRepository<Appliance> applianceRepo;
        private readonly ILoggerService<ApplianceService> logger;
        private readonly IHttpContextAccessor accessor;

        public ApplianceService(IRepository<Appliance> applianceRepo,
          ILoggerService<ApplianceService> logger,
          IHttpContextAccessor accessor)
        {
            this.applianceRepo = applianceRepo;
            this.logger = logger;
            this.accessor = accessor;
        }

        // add Appliance
        public async Task CreateAppliance(Appliance appliance)
        {
            if (appliance == null)
            {
                throw new AppException("Appliance object cannot be null");
            }

            if (await applianceRepo.AnyAsync(m => m.Name.ToLower() == appliance.Name.ToLower()))
            {
                throw new AppException($"A appliance with name '{appliance.Name}' already exist");
            }

            var currentUser = accessor.HttpContext.GetUserSession();
            appliance.CreatedBy = currentUser.Id;
            appliance.CreatedDate = DateTimeOffset.Now;

            await applianceRepo.Insert(appliance, false);

            //log action
            await logger.LogActivity(ActivityActionType.CREATE_APPLIANCE, currentUser.Email, $"Created appliance with name {appliance.Name}");
        }

        // delete Appliance
        public async Task DeleteAppliance(long applianceId)
        {
            var appliance = await applianceRepo.GetById(applianceId);
            if (appliance == null)
            {
                throw new AppException($"Invalid Appliance id {applianceId}");
            }
            else
            {

                var _appliance = appliance.Clone<Appliance>();
                await applianceRepo.Delete(applianceId, false);

                var currentUser = accessor.HttpContext.GetUserSession();
                // log activity
                await logger.LogActivity(ActivityActionType.DELETE_APPLIANCE, currentUser.Email, applianceRepo.TableName, _appliance, new Appliance(),
                    $"Deleted appliance with name {_appliance.Name}");

            }
        }

        public IEnumerable<Appliance> GetAppliances()
        {
            return applianceRepo.GetAll().OrderBy(m => m.Name);
        }

        public async Task<Appliance> GetAppliance(long id)
        {
            return await applianceRepo.GetById(id);
        }
    }
}
