using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.ViewModels
{
    public class ReportItemVM
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public decimal Amount { get; set; }
        public string FormattedAmount { get; set; }
    }
}
