using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace PMAPortal.Web.UIServices
{
    public interface IDropdownService
    {
        IEnumerable<SelectListItem> GetAppliances(string value = null);
        IEnumerable<SelectListItem> GetPets(string value = null);
        IEnumerable<SelectListItem> GetAreas(string value = null, string emptyText = null);
        IEnumerable<SelectListItem> GetHouseTypes(string value = null);
        IEnumerable<SelectListItem> GetMeters(string value = null, string emptyText = null);
        IEnumerable<SelectListItem> GetRoles(string value = null);
        IEnumerable<SelectListItem> GetApplicationStatuses(string value = null, string emptyText = null);
        IEnumerable<SelectListItem> GetInstallers(string value = null, string emptyText = null);
        IEnumerable<SelectListItem> GetSurveyStaffs(string value = null, string emptyText = null);
    }
}
