using PMAPortal.Web.Extensions;
using PMAPortal.Web.Models;
using System.Collections.Generic;
using System.Linq;

namespace PMAPortal.Web.DTOs
{
    public class SessionObject
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }
        public string Role { get; set; }
        public string UserType { get; set; } // user or student

        public static SessionObject FromUser(User user)
        {
            return new SessionObject
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName=user.LastName,
                FullName = $"{user.FirstName} {user.LastName}",
                Code=user.Code,
                Id = user.Id,
                Role = user.Role.Name,
                UserType = Constants.USER_TYPE_USER
            };
        }

        public static SessionObject FromApplicant(Applicant applicant)
        {
            return new SessionObject
            {
                Email = applicant.Email,
                FirstName = applicant.FirstName,
                LastName = applicant.LastName,
                FullName = $"{applicant.FirstName} {applicant.LastName}",
                Id = applicant.Id,
                UserType = Constants.USER_TYPE_APPLICANT
            };
        }
    }
}
