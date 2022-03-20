using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.ViewModels
{
    public class ImageVM
    {
        public int Id { get; set; }
        public long InstallationId { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public string Caption { get; set; }
        public bool Required { get; set; } = true;
    }

}
