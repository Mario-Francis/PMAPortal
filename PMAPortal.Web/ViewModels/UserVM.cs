using PMAPortal.Web.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PMAPortal.Web.ViewModels
{
    public class UserVM
    {
        public long Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public string Code { get; set; }
        public string CompanyName { get; set; }
        [Required]
        public IEnumerable<ItemVM> Roles { get; set; }

        public string FullName { get {
                return $"{FirstName} {LastName}".Replace("  ", " ");
            } }

        public bool IsActive { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public string FormattedCreatedDate
        {
            get
            {
                return CreatedDate.ToString("MMM d, yyyy");
            }
        }
        public string FormattedUpdatedDate
        {
            get
            {
                return UpdatedDate.ToString("MMM d, yyyy");
            }
        }

        public User ToUser()
        {
            return new User
            {
                Id = Id,
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                PhoneNumber = PhoneNumber,
                Code = Code,
                CompanyName=CompanyName,
                IsActive = IsActive,
                CreatedDate = DateTimeOffset.Now,
                UpdatedDate = DateTimeOffset.Now,
                UserRoles = Roles.Select(r=> new UserRole
                {
                    RoleId=r.Id,
                    UserId=Id,
                    CreatedDate=DateTimeOffset.Now
                }).ToList()
            };
        }

        public static UserVM FromUser(User user, int? clientTimeOffset = null)
        {
            return user==null?null: new UserVM
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Code = user.Code ?? "",
                CompanyName = user.CompanyName ?? "",
                IsActive = user.IsActive,
                Roles = user.UserRoles.Select(ur=> ItemVM.FromUserRole(ur)),
                UpdatedBy = user.UpdatedByUser == null ? null : $"{user.UpdatedByUser.FirstName} {user.UpdatedByUser.LastName} ({user.UpdatedByUser.Email})",
                CreatedDate = clientTimeOffset == null ? user.CreatedDate : user.CreatedDate.ToOffset(TimeSpan.FromMinutes(clientTimeOffset.Value)),
                UpdatedDate = clientTimeOffset == null ? user.UpdatedDate : user.UpdatedDate.ToOffset(TimeSpan.FromMinutes(clientTimeOffset.Value)),
            };
        }
    }
}
