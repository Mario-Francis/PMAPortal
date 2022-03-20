using PMAPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.ViewModels
{
    public class InstallationLogVM
    {
        public long Id { get; set; }
        public long InstallationId { get; set; }
        public long? ActionById { get; set; }
        public long InstallationStatusId { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }

        public string ActionBy { get; set; }
        public virtual UserVM ActionByUser { get; set; }
        public virtual InstallationVM Installation { get; set; }
        public virtual string InstallationStatus { get; set; }

        public long? CreatedById { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string FormattedCreatedDate
        {
            get
            {
                return CreatedDate.ToString("MMM d, yyyy 'at' hh:mmtt");
            }
        }

        public static InstallationLogVM FromInstallationLog(InstallationLog log, int? clientTimeOffset = null)
        {
            return new InstallationLogVM
            {
                Id = log.Id,
                ActionBy = log.ActionByUser == null ? null : $"{log.ActionByUser.FirstName} {log.ActionByUser.LastName} ({log.ActionByUser.Email})",
                ActionById = log.ActionBy,
                ActionByUser = UserVM.FromUser(log.ActionByUser),
                Comment = log.Comment,
                Description = log.Description,
                InstallationId = log.InstallationId,
                InstallationStatusId = log.InstallationStatusId,
                InstallationStatus = log.InstallationStatus.Name,
                CreatedById = log.CreatedBy,
                CreatedDate = clientTimeOffset == null ? log.CreatedDate : log.CreatedDate.ToOffset(TimeSpan.FromMinutes(clientTimeOffset.Value)),
                //Installation = InstallationVM.FromInstallation(log.Installation)
            };
        }
    }
}
