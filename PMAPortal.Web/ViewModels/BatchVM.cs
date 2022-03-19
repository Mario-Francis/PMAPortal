using PMAPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.ViewModels
{
    public class BatchVM
    {
        public long Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        [Required]
        public DateTimeOffset DateShared { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }

        public int CustomerCount { get; set; }

        public string FormattedDateShared
        {
            get
            {
                return DateShared.ToString("MMM d, yyyy 'at' hh:mmtt");
            }
        }
        public string FormattedCreatedDate
        {
            get
            {
                return CreatedDate.ToString("MMM d, yyyy 'at' hh:mmtt");
            }
        }


        public static BatchVM FromBatch(Batch batch, int? clientTimeOffset = null)
        {
            return new BatchVM
            {
                Id = batch.Id,
                FileName = batch.FileName,
                FilePath = batch.FilePath,
                CustomerCount = batch.Customers.Count(),
                CreatedBy = batch.CreatedByUser == null ? null : $"{batch.CreatedByUser.FirstName} {batch.CreatedByUser.LastName} ({batch.CreatedByUser.Email})",
                CreatedDate = clientTimeOffset == null ? batch.CreatedDate : batch.CreatedDate.ToOffset(TimeSpan.FromMinutes(clientTimeOffset.Value)),
                DateShared = clientTimeOffset == null ? batch.DateShared : batch.DateShared.ToOffset(TimeSpan.FromMinutes(clientTimeOffset.Value))
            };
        }

        public static BatchVM FromInstallationBatch(InstallationBatch batch, int? clientTimeOffset = null)
        {
            return new BatchVM
            {
                Id = batch.Id,
                FileName = batch.FileName,
                FilePath = batch.FilePath,
                CustomerCount = batch.InstallationBatchItems.Count(),
                CreatedBy = batch.CreatedByUser == null ? null : $"{batch.CreatedByUser.FirstName} {batch.CreatedByUser.LastName} ({batch.CreatedByUser.Email})",
                CreatedDate = clientTimeOffset == null ? batch.CreatedDate : batch.CreatedDate.ToOffset(TimeSpan.FromMinutes(clientTimeOffset.Value)),
                DateShared = clientTimeOffset == null ? batch.DateShared : batch.DateShared.ToOffset(TimeSpan.FromMinutes(clientTimeOffset.Value))
            };
        }
    }
}
