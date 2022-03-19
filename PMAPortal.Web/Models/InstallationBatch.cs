using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.Models
{
    public class InstallationBatch:BaseEntity
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTimeOffset DateShared { get; set; }

        // navigation
        public virtual ICollection<InstallationBatchItem> InstallationBatchItems { get; set; }
    }
}
