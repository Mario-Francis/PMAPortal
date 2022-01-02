using DataTablesParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.ViewModels
{
    public class DataTableResultVM<T>:Results<T> where T:class
    {
        public int MeterCount { get; set; }
        public IEnumerable<ReportItemVM> MeterCountByTypes { get; set; }
        public decimal TotalAmount { get; set; }
        public string FormattedTotalAmount { get; set; }
        public IEnumerable<ReportItemVM> TotalAmountByTypes { get; set; }
        public int AreaCount { get; set; }
        public IEnumerable<string> Areas { get; set; }
    }
}
