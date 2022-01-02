using AutoMapper;
using DataTablesParser;
using PMAPortal.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web
{
    public class AutoMapping:Profile
    {
        
        public AutoMapping()
        {
            CreateMap<Results<ApplicationItemVM>, DataTableResultVM<ApplicationItemVM>>();
             // .ForAllOtherMembers(x => x.AllowNull());


        }
    }
}
