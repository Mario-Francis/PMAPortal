using Microsoft.AspNetCore.Mvc.Rendering;
using PMAPortal.Web.Services;
using PMAPortal.Web.Extensions;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace PMAPortal.Web.UIServices
{
    public class DropdownService:IDropdownService
    {
        private readonly IListService listService;
        private readonly IUserService userService;

        public DropdownService(IListService listService, IUserService userService)
        {
            this.listService = listService;
            this.userService = userService;
        }

        public IEnumerable<SelectListItem> GetAppliances(string value=null)
        {
            List<SelectListItem> appliances =  listService.GetAppliances()
                .Select(c => new SelectListItem { Text = c.Name, Value = c.Name.ToString(), Selected=c.Name.ToString() == value }).ToList();
            appliances.Insert(0, new SelectListItem { Text = "- Select appliance -", Value = "" });
            appliances.Add(new SelectListItem { Text = "Others", Value = "Others" });

            return appliances;
        }

        public IEnumerable<SelectListItem> GetPets(string value = null)
        {
            List<SelectListItem> pets = listService.GetPets()
                .Select(c => new SelectListItem { Text = c.Name, Value = c.Name.ToString(), Selected = c.Name.ToString() == value }).ToList();
            pets.Insert(0, new SelectListItem { Text = "- Select pet -", Value = "" });
            pets.Add(new SelectListItem { Text = "Others", Value = "Others" });

            return pets;
        }

        public IEnumerable<SelectListItem> GetAreas(string value = null, string emptyText=null)
        {
            List<SelectListItem> areas = listService.GetAreas()
                .Select(c => new SelectListItem { Text = c.Name, Value = c.Name.ToString(), Selected = c.Name.ToString() == value }).ToList();
            areas.Insert(0, new SelectListItem { Text = emptyText ?? "- Select area -", Value = "" });

            return areas;
        }

        public IEnumerable<SelectListItem> GetHouseTypes(string value = null)
        {
            List<SelectListItem> houseTypes = listService.GetHouseTypes()
                .Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString(), Selected = c.Id.ToString() == value }).ToList();
            houseTypes.Insert(0, new SelectListItem { Text = "- Select house type -", Value = "" });

            return houseTypes;
        }

        public IEnumerable<SelectListItem> GetMeters(string value = null, string emptyText = null)
        {
            List<SelectListItem> meters = listService.GetMeters()
                .Select(c => new SelectListItem { Text = $"{c.Name} - {c.Amount.Format()}", Value = c.Id.ToString(), Selected = c.Id.ToString() == value }).ToList();
            meters.Insert(0, new SelectListItem { Text = emptyText ?? "- Select meter -", Value = "" });

            return meters;
        }

        public IEnumerable<SelectListItem> GetRoles(string value = null)
        {
            List<SelectListItem> roles = listService.GetRoles()
                .Select(l => new SelectListItem { Text = l.Name, Value = l.Id.ToString(), Selected = l.Id.ToString() == value }).ToList();

            roles.Insert(0, new SelectListItem { Text = "- Select role -", Value = "" });
            return roles;
        }
        public IEnumerable<SelectListItem> GetApplicationStatuses(string value = null, string emptyText = null)
        {
            List<SelectListItem> statuses = listService.GetApplicationStatuses()
                .Select(c => new SelectListItem { Text =c.Name, Value = c.Id.ToString(), Selected = c.Id.ToString() == value }).ToList();
            statuses.Insert(0, new SelectListItem { Text = emptyText ?? "- Select status -", Value = "" });

            return statuses;
        }
        public IEnumerable<SelectListItem> GetInstallers(string value = null, string emptyText = null)
        {
            List<SelectListItem> installers = userService.GetUsers().Where(u=>u.UserRoles.Any(ur=>ur.RoleId == (int)AppRoles.INSTALLER)).OrderBy(u=>u.FirstName)
                .Select(u => new SelectListItem { Text = $"{u.FirstName} {u.LastName} ({u.Email})", Value = u.Id.ToString(), Selected = u.Id.ToString() == value }).ToList();
            installers.Insert(0, new SelectListItem { Text = emptyText ?? "- Select installer -", Value = "" });

            return installers;
        }

        public IEnumerable<SelectListItem> GetSurveyStaffs(string value = null, string emptyText = null)
        {
            List<SelectListItem> surveystaffs = userService.GetUsers().Where(u => u.UserRoles.Any(ur => ur.RoleId == (int)AppRoles.SURVEY_STAFF)).OrderBy(u => u.FirstName)
                .Select(u => new SelectListItem { Text = $"{u.FirstName} {u.LastName} ({u.Email})", Value = u.Id.ToString(), Selected = u.Id.ToString() == value }).ToList();
            surveystaffs.Insert(0, new SelectListItem { Text = emptyText ?? "- Select survey staff -", Value = "" });

            return surveystaffs;
        }

    }
}
