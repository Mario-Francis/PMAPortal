using PMAPortal.Web.DTOs;
using System;

namespace PMAPortal.Web.UIServices
{
    public interface ISessionService
    {
        public string BaseUrl { get; }
        public string ControllerName { get; }
        public string ActionName { get; }

        public SessionObject UserSession { get; }

        public bool UserSessionExist { get; }
        public string Initial { get; }

        public string DisplayRoles { get; }
        public bool IsAdmin
        {
            get;
        }
        public bool IsDisco
        {
            get;
        }

        public bool IsInstaller
        {
            get;
        }
        public string PaystackPKey { get; }
        public string Culture { get; }

        public DateTimeOffset ConverDatetToClientTimeZone(DateTimeOffset date);
    }
}