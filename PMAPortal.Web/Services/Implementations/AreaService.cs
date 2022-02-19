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
    public class AreaService : IAreaService
    {
        private readonly IRepository<Area> areaRepo;
        private readonly ILoggerService<AreaService> logger;
        private readonly IHttpContextAccessor accessor;

        public AreaService(IRepository<Area> areaRepo,
            ILoggerService<AreaService> logger,
            IHttpContextAccessor accessor)
        {
            this.areaRepo = areaRepo;
            this.logger = logger;
            this.accessor = accessor;
        }

        // add Area
        public async Task CreateArea(Area area)
        {
            if (area == null)
            {
                throw new AppException("Area object cannot be null");
            }

            if (await areaRepo.AnyAsync(m => m.Name.ToLower() == area.Name.ToLower()))
            {
                throw new AppException($"A area with name '{area.Name}' already exist");
            }

            var currentUser = accessor.HttpContext.GetUserSession();
            area.CreatedBy = currentUser.Id;
            area.CreatedDate = DateTimeOffset.Now;

            await areaRepo.Insert(area, false);

            //log action
            await logger.LogActivity(ActivityActionType.CREATE_AREA, currentUser.Email, $"Created area with name {area.Name}");
        }

        // delete Area
        public async Task DeleteArea(long areaId)
        {
            var area = await areaRepo.GetById(areaId);
            if (area == null)
            {
                throw new AppException($"Invalid Area id {areaId}");
            }
            else
            {

                var _area = area.Clone<Area>();
                await areaRepo.Delete(areaId, false);

                var currentUser = accessor.HttpContext.GetUserSession();
                // log activity
                await logger.LogActivity(ActivityActionType.DELETE_AREA, currentUser.Email, areaRepo.TableName, _area, new Area(),
                    $"Deleted area with name {_area.Name}");

            }
        }

        public IEnumerable<Area> GetAreas()
        {
            return areaRepo.GetAll().OrderBy(m => m.Name);
        }

        public async Task<Area> GetArea(long id)
        {
            return await areaRepo.GetById(id);
        }
    }
}
