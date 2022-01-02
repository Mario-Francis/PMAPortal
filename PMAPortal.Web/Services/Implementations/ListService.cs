using Microsoft.AspNetCore.Http;
using PMAPortal.Web.Data.Repositories;
using PMAPortal.Web.Extensions;
using PMAPortal.Web.Models;
using System.Collections.Generic;
using System.Linq;

namespace PMAPortal.Web.Services.Implementations
{
    public class ListService:IListService
    {
        private readonly IRepository<Appliance> applianceRepo;
        private readonly IRepository<Area> areaRepo;
        private readonly IRepository<HouseType> houseTypeRepo;
        private readonly IRepository<Role> roleRepo;
        private readonly IRepository<Meter> meterRepo;
        private readonly IRepository<Pet> petRepo;
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IRepository<ApplicationStatus> applicatioStatusRepo;

        public ListService(IRepository<Appliance> applianceRepo,
            IRepository<Area> areaRepo,
            IRepository<HouseType> houseTypeRepo,
            IRepository<Role> roleRepo,
            IRepository<Meter> meterRepo,
            IRepository<Pet> petRepo,
            IHttpContextAccessor contextAccessor,
            IRepository<ApplicationStatus> applicatioStatusRepo)
        {
            this.applianceRepo = applianceRepo;
            this.areaRepo = areaRepo;
            this.houseTypeRepo = houseTypeRepo;
            this.roleRepo = roleRepo;
            this.meterRepo = meterRepo;
            this.petRepo = petRepo;
            this.contextAccessor = contextAccessor;
            this.applicatioStatusRepo = applicatioStatusRepo;
        }

        public IEnumerable<Role> GetRoles()
        {
            var roles = roleRepo.GetAll();
            if (!contextAccessor.HttpContext.User.IsInRole(Constants.ROLE_ADMIN))
            {
                roles = roles.Where(r => r.Name != Constants.ROLE_ADMIN);
            }

            return roles;
        }

        public IEnumerable<Appliance> GetAppliances()
        {
            return applianceRepo.GetAll().OrderBy(a => a.Name);
        }

        public IEnumerable<Area> GetAreas()
        {
            return areaRepo.GetAll().OrderBy(a => a.Name);
        }

        public IEnumerable<HouseType> GetHouseTypes()
        {
            return houseTypeRepo.GetAll().OrderBy(a => a.Name);
        }

        public IEnumerable<Meter> GetMeters()
        {
            return meterRepo.GetAll().OrderBy(a => a.Name);
        }

        public IEnumerable<Pet> GetPets()
        {
            return petRepo.GetAll().OrderBy(p => p.Name);
        }

        public IEnumerable<ApplicationStatus> GetApplicationStatuses()
        {
            var statuses = applicatioStatusRepo.GetAll();
            long[] ids = null;
            var currentUser = contextAccessor.HttpContext.GetUserSession();
            if (currentUser != null)
            {
                if(currentUser.Role == Constants.ROLE_ADMIN || currentUser.Role == Constants.ROLE_SUPERVISOR)
                {
                    ids = new long[] { 2, 3, 4, 5, 6, 7, 8 };
                }else if(currentUser.Role == Constants.ROLE_INSTALLER)
                {
                    ids = new long[] { 3, 4, 5, 6 };
                }
                else
                {
                    ids = new long[] { 7,8 };
                }
                statuses = statuses.Where(s => ids.Contains(s.Id));
            }

            return statuses.OrderBy(p => p.Id);
        }
    }
}
