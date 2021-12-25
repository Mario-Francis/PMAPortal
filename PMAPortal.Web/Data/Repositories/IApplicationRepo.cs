using PMAPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PMAPortal.Web.Data.Repositories
{
    public interface IApplicationRepo: IRepository<Application>
    {
        IEnumerable<Application> _GetAll();
        IEnumerable<Application> _GetWhere(Expression<Func<Application, bool>> expression);
    }
}
