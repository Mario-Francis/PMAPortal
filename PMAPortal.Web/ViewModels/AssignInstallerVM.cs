using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.ViewModels
{
    public class AssignInstallerVM
    {
        [Required]
        public long? ApplicationId { get; set; }
        [Required]
        public long? InstallerId { get; set; }

    }
}
