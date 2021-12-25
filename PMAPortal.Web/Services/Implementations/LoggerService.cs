using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PMAPortal.Web.Extensions;
using PMAPortal.Web.Data.Repositories;
using PMAPortal.Web.Models.Audit;
using PMAPortal.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PMAPortal.Web.Models;

namespace PMAPortal.Web.Service.Implementations
{
    public class LoggerService<T>:ILoggerService<T> where T:class
    {
        private readonly ILogger<T> fileLogger;
        private readonly IRepository<ActivityLog> activityLogRepo;
        private readonly IHttpContextAccessor contextAccessor;

        public LoggerService(ILogger<T> fileLogger, IRepository<ActivityLog> activityLogRepo, IHttpContextAccessor contextAccessor)
        {
            this.fileLogger = fileLogger;
            this.activityLogRepo = activityLogRepo;
            this.contextAccessor = contextAccessor;
        }
        public void LogException(Exception ex, string message=null)
        {
            fileLogger.LogError(ex, message ?? ex.Message);
        }

        public void LogInfo(string log)
        {
            fileLogger.LogInformation(log);
        }

        public void LogError(string log)
        {
            fileLogger.LogError(log);
        }

        // activity logs
        public async Task LogActivity(ActivityActionType actionType, string actionBy, string descrirption = null, string actionByType = Constants.USER_TYPE_USER)
        {
            var log = new ActivityLog()
            {
                ActionBy = actionBy,
                ActionByType=actionByType,
                ActionType = actionType.ToString(),
                Description = descrirption,
                IPAddress = contextAccessor?.HttpContext.GetIPAddress(),
                CreatedDate = DateTimeOffset.Now,
            };
            await activityLogRepo.Insert(log);
        }

        public async Task LogActivity<L>(ActivityActionType actionType, string actionBy, string tableName, L _from, L _to, string descrirption = null, string actionByType = Constants.USER_TYPE_USER) where L : BaseEntity
        {
            var activityLog = new ActivityLog()
            {
                ActionBy = actionBy,
                ActionByType = actionByType,
                ActionType = actionType.ToString(),
                Description = descrirption,
                IPAddress = contextAccessor?.HttpContext.GetIPAddress(),
                CreatedDate = DateTimeOffset.Now,
            };
           // await logRepo.LogActivity(activitylog);

            var checkResult = _from.CheckChanges(_to, Constants.IGNORED_COLUMNS);
            if (checkResult.HasChanges)
            {
                var auditLog = new AuditLog()
                {
                    TableName = tableName,
                    ItemId = _from?.Id,
                    CreatedDate = DateTimeOffset.Now
                };
                //await logRepo.LogAudit(auditLog);

                var auditLogchanges = new List<AuditLogChange>();
                var froms = checkResult.FromProperties.ToList();
                var tos = checkResult.ToProperties.ToList();
                froms.ForEach((f) =>
                {
                    var index = froms.IndexOf(f);
                    var t = tos[index];
                    var change = new AuditLogChange
                    {
                        ColumnName = f.Key,
                        From = f.Value,
                        To = t.Value,
                        CreatedDate = DateTimeOffset.Now
                    };
                    auditLogchanges.Add(change);
                });
                auditLog.AuditLogChanges = auditLogchanges;
                activityLog.AuditLog = auditLog;
            }
            await activityLogRepo.Insert(activityLog);
        }

    }
}
