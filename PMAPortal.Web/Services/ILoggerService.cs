using PMAPortal.Web.Models;
using System;
using System.Threading.Tasks;

namespace PMAPortal.Web.Services
{
    public interface ILoggerService<T> where T : class
    {
        void LogException(Exception ex, string message = null);

        void LogInfo(string log);

        void LogError(string log);

        // activity logs
        Task LogActivity(ActivityActionType actionType, string actionBy, string descrirption = null, string actionByType = Constants.USER_TYPE_USER);

        Task LogActivity<L>(ActivityActionType actionType, string actionBy, string tableName, L _from, L _to, string descrirption = null, string actionByType = Constants.USER_TYPE_USER) where L : BaseEntity;
    }
}
