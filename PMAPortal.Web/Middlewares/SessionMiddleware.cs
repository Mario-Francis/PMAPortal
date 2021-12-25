using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PMAPortal.Web.DTOs;
using PMAPortal.Web.Extensions;
using PMAPortal.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PMAPortal.Web.Middlewares
{
    public class SessionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IOptionsMonitor<AppSettings> appSettingsDelegate;

        public SessionMiddleware(RequestDelegate next, IOptionsMonitor<AppSettings> appSettingsDelegate)
        {
            _next = next;
            this.appSettingsDelegate = appSettingsDelegate;
        }

        public async Task Invoke(HttpContext context, IUserService userService, ILogger<SessionMiddleware> logger)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                await AttachUserToContext(context, userService, logger);
            }

            await _next(context);
        }

        private async Task AttachUserToContext(HttpContext context, IUserService userService, ILogger<SessionMiddleware> logger)
        {
            try
            {
                // attach user to context 

                var id = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value;
                var type = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Actor).Value;
                if (id != null && type != null)
                {
                    SessionObject sessionObject = null;
                    if (type == Constants.USER_TYPE_USER)
                    {
                        var user = await userService.GetUser(Convert.ToInt64(id));
                        sessionObject = SessionObject.FromUser(user);
                        context.SetUserSession(sessionObject);
                    }
                    else if (type == Constants.USER_TYPE_APPLICANT)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error was encountered while attaching user to the current Http Context");
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }
    }
}
