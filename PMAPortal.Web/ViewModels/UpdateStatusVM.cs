using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.ViewModels
{
    public class UpdateStatusVM
    {
        [Required]
        public long ApplicationId { get; set; }
        [Required]
        public long StatusId { get; set; }
        public string Comment { get; set; }
    }
}
