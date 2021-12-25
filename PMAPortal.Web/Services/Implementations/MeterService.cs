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
    public class MeterService:IMeterService
    {
        private readonly IRepository<Meter> meterRepo;
        private readonly IRepository<Application> appRepo;
        private readonly ILoggerService<MeterService> logger;
        private readonly IHttpContextAccessor accessor;

        public MeterService(IRepository<Meter> meterRepo,
            IRepository<Application> appRepo,
            ILoggerService<MeterService> logger,
            IHttpContextAccessor accessor)
        {
            this.meterRepo = meterRepo;
            this.appRepo = appRepo;
            this.logger = logger;
            this.accessor = accessor;
        }

        // add Meter
        public async Task CreateMeter(Meter meter)
        {
            if (meter == null)
            {
                throw new AppException("Meter object cannot be null");
            }

            if (await meterRepo.Any(m => m.Name.ToLower() == meter.Name.ToLower()))
            {
                throw new AppException($"A meter with name '{meter.Name}' already exist");
            }

            var currentUser = accessor.HttpContext.GetUserSession();
            meter.CreatedBy = currentUser.Id;
            meter.CreatedDate = DateTimeOffset.Now;
            meter.UpdatedBy = currentUser.Id;
            meter.UpdatedDate = DateTimeOffset.Now;

            await meterRepo.Insert(meter, false);

            //log action
            await logger.LogActivity(ActivityActionType.CREATE_METER, currentUser.Email, $"Created meter with name {meter.Name}");
          }

        // delete Meter
        public async Task DeleteMeter(long meterId)
        {
            var meter = await meterRepo.GetById(meterId);
            if (meter == null)
            {
                throw new AppException($"Invalid Meter id {meterId}");
            }
            else
            {
                if (await appRepo.Any(a=>a.MeterId==meterId))
                {
                    throw new AppException("Meter cannot be deleted as it has one or more applications attached to it");
                }
                else
                {
                    var _meter = meter.Clone<Meter>();
                    await meterRepo.Delete(meterId, false);

                    var currentUser = accessor.HttpContext.GetUserSession();
                    // log activity
                    await logger.LogActivity(ActivityActionType.DELETE_METER, currentUser.Email, meterRepo.TableName, _meter, new Meter(), 
                        $"Deleted meter with name {_meter.Name}");
                }
            }
        }

        // update Meter
        public async Task UpdateMeter(Meter meter)
        {
            var _meter = await meterRepo.GetById(meter.Id);
            if (_meter == null)
            {
                throw new AppException($"Invalid Meter id {meter.Id}");
            }
            else if (await meterRepo.Any(m => (m.Name.ToLower() == meter.Name.ToLower()) &&
            !(_meter.Name.ToLower() == meter.Name.ToLower())))
            {
                throw new AppException($"A Meter with name '{meter.Name}' already exist");
            }
            else
            {
                var currentUser = accessor.HttpContext.GetUserSession();
                var _oldMeter = _meter.Clone<Meter>();

                _meter.Name = meter.Name;
                _meter.Amount = meter.Amount;
                _meter.Description = meter.Description;
                _meter.UpdatedBy = currentUser.Id;
                _meter.UpdatedDate = DateTimeOffset.Now;

                await meterRepo.Update(_meter, false);


                // log activity
                await logger.LogActivity(ActivityActionType.UPDATE_METER, currentUser.Email, meterRepo.TableName, _meter, _oldMeter,
                        $"UUpdated meter");
            }
        }

        public IEnumerable<Meter> GetMeters()
        {
            return meterRepo.GetAll().OrderBy(m => m.Name);
        }

        public async Task<Meter> GetMeter(long id)
        {
            return await meterRepo.GetById(id);
        }
    }
}
