using PMAPortal.Web.DTOs;
using PMAPortal.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMAPortal.Web.Services
{
    public interface IUserService
    {
        Task<long> InitialUserSetup(User user);
        Task<long> CreateUser(User user);
        Task UpdateUser(User user);
        Task UpdatePassword(PasswordRequestObject req);
        Task<bool> IsUserAuthentic(LoginCredential credential);
        Task<User> GetUser(long userId);
        Task<User> GetUser(string email);
        IEnumerable<User> GetUsers(bool includeInactive = true);
        IEnumerable<User> GetUsers(long roleId, bool includeInactive = true);
        Task DeleteUser(long userId);
        Task<bool> AnyUserExists();
        Task UpdateUserStatus(long userId, bool isActive);
        Task ResetPassword(long userId);

    }
}
