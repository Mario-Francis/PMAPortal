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

        public DropdownService(IListService listService)
        {
            this.listService = listService;
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

        public IEnumerable<SelectListItem> GetAreas(string value = null)
        {
            List<SelectListItem> areas = listService.GetAreas()
                .Select(c => new SelectListItem { Text = c.Name, Value = c.Name.ToString(), Selected = c.Name.ToString() == value }).ToList();
            areas.Insert(0, new SelectListItem { Text = "- Select area -", Value = "" });

            return areas;
        }

        public IEnumerable<SelectListItem> GetHouseTypes(string value = null)
        {
            List<SelectListItem> houseTypes = listService.GetHouseTypes()
                .Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString(), Selected = c.Id.ToString() == value }).ToList();
            houseTypes.Insert(0, new SelectListItem { Text = "- Select house type -", Value = "" });

            return houseTypes;
        }

        public IEnumerable<SelectListItem> GetMeters(string value = null)
        {
            List<SelectListItem> meters = listService.GetMeters()
                .Select(c => new SelectListItem { Text = $"{c.Name} - {c.Amount.Format()}", Value = c.Id.ToString(), Selected = c.Id.ToString() == value }).ToList();
            meters.Insert(0, new SelectListItem { Text = "- Select meter -", Value = "" });

            return meters;
        }

        public IEnumerable<SelectListItem> GetRoles(string value = null)
        {
            List<SelectListItem> roles = listService.GetRoles()
                .Select(l => new SelectListItem { Text = l.Name, Value = l.Id.ToString(), Selected = l.Id.ToString() == value }).ToList();

            roles.Insert(0, new SelectListItem { Text = "- Select role -", Value = "" });
            return roles;
        }
        public IEnumerable<SelectListItem> GetApplicationStatuses(string value = null)
        {
            List<SelectListItem> statuses = listService.GetApplicationStatuses()
                .Select(c => new SelectListItem { Text =c.Name, Value = c.Id.ToString(), Selected = c.Id.ToString() == value }).ToList();
            statuses.Insert(0, new SelectListItem { Text = "- Select status -", Value = "" });

            return statuses;
        }

    }
}
