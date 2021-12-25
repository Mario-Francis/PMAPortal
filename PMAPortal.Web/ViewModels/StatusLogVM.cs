using PMAPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.ViewModels
{
    public class StatusLogVM
    {
        public long Id { get; set; }
        public long ApplicationId { get; set; }
        public string ActionBy { get; set; }
        public long StatusId { get; set; }
        public string Status{ get; set; }
        public string Comment { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public string FormattedCreatedDate
        {
            get
            {
                return CreatedDate.ToString("MMM d, yyyy 'at' hh:mmtt");
            }
        }

        public static StatusLogVM FromApplicationStatusLog(ApplicationStatusLog log, int? clientTimeOffset = null)
        {
            return new StatusLogVM
            {
                Id = log.Id,
                ApplicationId = log.ApplicationId,
                StatusId = log.ApplicationStatusId,
                Status = log.ApplicationStatus.Name,
                ActionBy = log.ActionByUser == null ? null : $"{log.ActionByUser.FirstName} {log.ActionByUser.LastName} ({log.ActionByUser.Email})",
                CreatedDate = clientTimeOffset == null ? log.CreatedDate : log.CreatedDate.ToOffset(TimeSpan.FromMinutes(clientTimeOffset.Value)),
                Comment = log.Comment
            };
        }
    }
}
