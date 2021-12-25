using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using PMAPortal.Web.DTOs;
using PMAPortal.Web.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PMAPortal.Web.UIServices
{
    public class SessionService:ISessionService
    {
        private readonly IHttpContextAccessor accessor;
        private readonly IOptionsSnapshot<AppSettings> appSettingsDelegate;

        public SessionService(IHttpContextAccessor accessor, IOptionsSnapshot<AppSettings> appSettingsDelegate)
        {
            this.accessor = accessor;
            this.appSettingsDelegate = appSettingsDelegate;
        }

        public string BaseUrl
        {
            get
            {
                return $"{accessor.HttpContext.Request.Scheme}://{accessor.HttpContext.Request.Host}{accessor.HttpContext.Request.PathBase}/";
            }
        }
        public string ControllerName
        {
            get
            {
                var path = accessor.HttpContext.Request.Path.ToString();
                if (path.Length > 1)
                {
                    path = path.Substring(1);
                    return path.Split(new char[] { '/' })[0];
                }
                else
                {
                    return "";
                }
            }
        }
        public string ActionName
        {
            get
            {
                var path = accessor.HttpContext.Request.Path.ToString();
                if (path.Length > 1)
                {
                    path = path.Substring(1);
                    var arr = path.Split(new char[] { '/' });
                    if (arr.Length > 1)
                    {
                        return arr[1];
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }
            }
        }

        public SessionObject UserSession
        {
            get
            {
                return accessor.HttpContext.GetUserSession();
            }
        }

        public bool UserSessionExist
        {
            get
            {
                return accessor.HttpContext.IsUserSessionExist();
            }
        }
        public string Initial
        {
            get
            {
                var session = UserSession;
                return session.FirstName.Substring(0, 1) + session.LastName.Substring(0, 1);
            }
        }

        public string DisplayRoles
        {
            get
            {
                var session = UserSession;
                if (session.Role == Constants.ROLE_ADMIN)
                {
                    return Constants.ROLE_ADMIN;
                }
                else if(session.Role== Constants.ROLE_INSTALLER)
                {
                    return Constants.ROLE_INSTALLER;
                }
                else
                {
                    return Constants.ROLE_DISCO;
                }
            }
        }

        public bool IsAdmin
        {
            get
            {
                return accessor.HttpContext.User.IsInRole(Constants.ROLE_ADMIN);
            }
        }
        public bool IsDisco
        {
            get
            {
                return accessor.HttpContext.User.IsInRole(Constants.ROLE_DISCO);
            }
        }

        public bool IsInstaller
        {
            get
            {
                return accessor.HttpContext.User.IsInRole(Constants.ROLE_INSTALLER);
            }
        }

        public string PaystackPKey
        {
            get
            {
                return appSettingsDelegate.Value.Paystack.PKey;
            }
        }
        public string Culture
        {
            get
            {
                var rcf= accessor.HttpContext.Request.HttpContext.Features.Get<IRequestCultureFeature>();
                return rcf.RequestCulture.Culture.Name;
            }
        }

        public DateTimeOffset ConverDatetToClientTimeZone(DateTimeOffset date)
        {
            int defaultOffset = appSettingsDelegate.Value.DefaultTimeZoneOffset;
            var val = accessor.HttpContext.Request.Cookies["clientTimeZoneOffset"];
            int offsetInMminutes = string.IsNullOrEmpty(val) ? defaultOffset : Convert.ToInt32(val);
            return date.ToOffset(new TimeSpan(0, offsetInMminutes, 0));
        }

    }
}
