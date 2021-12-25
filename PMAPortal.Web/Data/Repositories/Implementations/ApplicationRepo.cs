using Microsoft.EntityFrameworkCore;
using PMAPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PMAPortal.Web.Data.Repositories.Implementations
{
    public class ApplicationRepo : Repository<Application>,IApplicationRepo
    {
        public ApplicationRepo(AppDbContext db) : base(db)
        {
        }

        public IEnumerable<Application> _GetAll()
        {
            return db.Applications.Include(a => a.Applicant)
                .Include(a => a.ApplicationAppliances)
                .Include(a => a.ApplicationPets)
                .Include(a => a.ApplicationStatus)
                .Include(a => a.ApplicationStatusLogs)
                .Include(a => a.HouseType)
                .Include(a => a.Meter)
                .Include(a => a.Payments)
                .Join(db.Users, a => a.UpdatedBy, u => u.Id, (a, u) => new { a, u }).Select(au => new Application
                {
                    Id = au.a.Id,
                    Applicant = au.a.Applicant,
                    ApplicantId = au.a.ApplicantId,
                    ApplicationAppliances = au.a.ApplicationAppliances,
                    ApplicationPets = au.a.ApplicationPets,
                    ApplicationStatus = au.a.ApplicationStatus,
                    ApplicationStatusId = au.a.ApplicationStatusId,
                    ApplicationStatusLogs = au.a.ApplicationStatusLogs,
                    CreatedBy = au.a.CreatedBy,
                    HasPets = au.a.HasPets,
                    HouseType = au.a.HouseType,
                    HouseTypeId = au.a.HouseTypeId,
                    Meter = au.a.Meter,
                    MeterId = au.a.MeterId,
                    Payments = au.a.Payments,
                    RoomsCount = au.a.RoomsCount,
                    TrackNumber = au.a.TrackNumber,
                    UpdatedBy = au.a.UpdatedBy,
                    CreatedDate = au.a.CreatedDate,
                    UpdatedByUser = au.u,
                    UpdatedDate = au.a.UpdatedDate
                }
           );
        }

        public IEnumerable<Application> _GetWhere(Expression<Func<Application, bool>> expression)
        {
            return db.Applications.Include(a => a.Applicant)
                 .Include(a => a.ApplicationAppliances)
                 .Include(a => a.ApplicationPets)
                 .Include(a => a.ApplicationStatus)
                 .Include(a => a.ApplicationStatusLogs)
                 .Include(a => a.HouseType)
                 .Include(a => a.Meter)
                 .Include(a => a.Payments)
                 .Where(expression)
                 .Join(db.Users, a => a.UpdatedBy, u => u.Id, (a, u) => new { a, u }).Select(au => new Application
                 {
                     Id = au.a.Id,
                     Applicant = au.a.Applicant,
                     ApplicantId = au.a.ApplicantId,
                     ApplicationAppliances = au.a.ApplicationAppliances,
                     ApplicationPets = au.a.ApplicationPets,
                     ApplicationStatus = au.a.ApplicationStatus,
                     ApplicationStatusId = au.a.ApplicationStatusId,
                     ApplicationStatusLogs = au.a.ApplicationStatusLogs,
                     CreatedBy = au.a.CreatedBy,
                     HasPets = au.a.HasPets,
                     HouseType = au.a.HouseType,
                     HouseTypeId = au.a.HouseTypeId,
                     Meter = au.a.Meter,
                     MeterId = au.a.MeterId,
                     Payments = au.a.Payments,
                     RoomsCount = au.a.RoomsCount,
                     TrackNumber = au.a.TrackNumber,
                     UpdatedBy = au.a.UpdatedBy,
                     CreatedDate = au.a.CreatedDate,
                     UpdatedByUser = au.u,
                     UpdatedDate = au.a.UpdatedDate
                 }
            );
        }
    }
}
