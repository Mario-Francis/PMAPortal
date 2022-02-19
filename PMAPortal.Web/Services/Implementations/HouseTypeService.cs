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
    public class HouseTypeService:IHouseTypeService
    {
        private readonly IRepository<HouseType> houseTypeRepo;
        private readonly IRepository<Application> appRepo;
        private readonly ILoggerService<HouseTypeService> logger;
        private readonly IHttpContextAccessor accessor;

        public HouseTypeService(IRepository<HouseType> houseTypeRepo,
          IRepository<Application> appRepo,
          ILoggerService<HouseTypeService> logger,
          IHttpContextAccessor accessor)
        {
            this.houseTypeRepo = houseTypeRepo;
            this.appRepo = appRepo;
            this.logger = logger;
            this.accessor = accessor;
        }

        // add HouseType
        public async Task CreateHouseType(HouseType houseType)
        {
            if (houseType == null)
            {
                throw new AppException("HouseType object cannot be null");
            }

            if (await houseTypeRepo.AnyAsync(m => m.Name.ToLower() == houseType.Name.ToLower()))
            {
                throw new AppException($"A houseType with name '{houseType.Name}' already exist");
            }

            var currentUser = accessor.HttpContext.GetUserSession();
            houseType.CreatedBy = currentUser.Id;
            houseType.CreatedDate = DateTimeOffset.Now;

            await houseTypeRepo.Insert(houseType, false);

            //log action
            await logger.LogActivity(ActivityActionType.CREATE_HOUSE_TYPE, currentUser.Email, $"Created houseType with name {houseType.Name}");
        }

        // delete HouseType
        public async Task DeleteHouseType(long houseTypeId)
        {
            var houseType = await houseTypeRepo.GetById(houseTypeId);
            if (houseType == null)
            {
                throw new AppException($"Invalid HouseType id {houseTypeId}");
            }
            else
            {
                if (await appRepo.AnyAsync(a => a.HouseTypeId == houseTypeId))
                {
                    throw new AppException("HouseType cannot be deleted as it has one or more applications attached to it");
                }
                else
                {
                    var _houseType = houseType.Clone<HouseType>();
                    await houseTypeRepo.Delete(houseTypeId, false);

                    var currentUser = accessor.HttpContext.GetUserSession();
                    // log activity
                    await logger.LogActivity(ActivityActionType.DELETE_HOUSE_TYPE, currentUser.Email, houseTypeRepo.TableName, _houseType, new HouseType(),
                        $"Deleted houseType with name {_houseType.Name}");
                }
            }
        }

        // update HouseType
        public async Task UpdateHouseType(HouseType houseType)
        {
            var _houseType = await houseTypeRepo.GetById(houseType.Id);
            if (_houseType == null)
            {
                throw new AppException($"Invalid HouseType id {houseType.Id}");
            }
            else if (await houseTypeRepo.AnyAsync(m => (m.Name.ToLower() == houseType.Name.ToLower()) &&
            !(_houseType.Name.ToLower() == houseType.Name.ToLower())))
            {
                throw new AppException($"A HouseType with name '{houseType.Name}' already exist");
            }
            else
            {
                var currentUser = accessor.HttpContext.GetUserSession();
                var _oldHouseType = _houseType.Clone<HouseType>();

                _houseType.Name = houseType.Name;

                await houseTypeRepo.Update(_houseType, false);


                // log activity
                await logger.LogActivity(ActivityActionType.UPDATE_HOUSE_TYPE, currentUser.Email, houseTypeRepo.TableName, _houseType, _oldHouseType,
                        $"UUpdated houseType");
            }
        }

        public IEnumerable<HouseType> GetHouseTypes()
        {
            return houseTypeRepo.GetAll().OrderBy(m => m.Name);
        }

        public async Task<HouseType> GetHouseType(long id)
        {
            return await houseTypeRepo.GetById(id);
        }
    }
}
