using ClosedXML.Excel;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PMAPortal.Web.Data.Repositories;
using PMAPortal.Web.DTOs;
using PMAPortal.Web.Extensions;
using PMAPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> userRepo;
        private readonly IRepository<Role> roleRepo;
        private readonly IRepository<UserRole> userRoleRepo;
        private readonly IPasswordService passwordService;
        private readonly ILoggerService<UserService> logger;
        private readonly IHttpContextAccessor accessor;
        private readonly ITokenService tokenService;
        private readonly IMailService mailService;
        private readonly IOptionsSnapshot<AppSettings> appSettingsDelegate;
        private readonly Random Rand;

        public UserService(
            IRepository<User> userRepo,
            IRepository<Role> roleRepo,
            IRepository<UserRole> userRoleRepo,
            IPasswordService passwordService,
            ILoggerService<UserService> logger,
            IHttpContextAccessor accessor,
            ITokenService tokenService,
            IMailService mailService,
            IOptionsSnapshot<AppSettings> appSettingsDelegate)
        {
            this.userRepo = userRepo;
            this.roleRepo = roleRepo;
            this.userRoleRepo = userRoleRepo;
            this.passwordService = passwordService;
            this.logger = logger;
            this.accessor = accessor;
            this.tokenService = tokenService;
            this.mailService = mailService;
            this.appSettingsDelegate = appSettingsDelegate;

            this.Rand = new Random();
        }

        // Initial sysadmin user setup
        public async Task<long> InitialUserSetup(User user)
        {
            if (user == null)
            {
                throw new AppException("User object is required");
            }
            if (!passwordService.ValidatePassword(user.Password, out string passwordValidationMessage))
            {
                throw new AppException(passwordValidationMessage);
            }

            //var user = req.ToUser();
            user.UserRoles = new List<UserRole> { new UserRole
                {
                    RoleId = (int)AppRoles.ADMINISTRATOR,
                    CreatedDate=DateTimeOffset.Now
                }
            };
            user.Password = passwordService.Hash(user.Password);
            user.IsActive = true;
            user.CreatedDate = DateTimeOffset.Now;
            user.UpdatedDate = DateTimeOffset.Now;

            await userRepo.Insert(user, false);

            // log action
            await logger.LogActivity(
                ActivityActionType.CREATE_USER,
                user.Email, descrirption: $"System Administrator User Setup with email '{user.Email}'");

            return user.Id;
        }
        private string GeneratePassword()
        {
            var bytes = new byte[8];
            Rand.NextBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        // create user
        public async Task<long> CreateUser(User user)
        {
            if (user == null)
            {
                throw new AppException("User object is required");
            }
            if (user.UserRoles.Count() == 0)
            {
                throw new AppException($"Role is required");
            }
            if (await userRepo.AnyAsync(u => u.Email == user.Email))
            {
                throw new AppException($"A user with email '{user.Email}' already exist");
            }

            var currentUser = accessor.HttpContext.GetUserSession();
            var password = GeneratePassword();
            user.Password = passwordService.Hash(password);
            user.IsActive = true;
            user.CreatedBy = currentUser.Id;
            user.UpdatedBy = currentUser.Id;
            await userRepo.Insert(user, false);


            // log action
            await logger.LogActivity(ActivityActionType.CREATE_USER,
                currentUser.Email,
                 $"Created new user with email '{user.Email}'");

            // send welcome/email verification mail

            var mail = new MailObject
            {
                Recipients = new List<Recipient> {
                    new Recipient {
                        FirstName=user.FirstName,
                        LastName=user.LastName,
                        Email=user.Email,
                        Password= password
                    }
                }
            };
            await mailService.ScheduleWelcomeMail(mail);

            return user.Id;
        }

        // update user
        public async Task UpdateUser(User user)
        {
            if (user == null)
            {
                throw new AppException("User object is required");
            }
            var _user = await userRepo.GetById(user.Id);
            if (_user == null)
            {
                throw new AppException($"User with id '{user.Id}' does not exist");
            }
            if (user.UserRoles.Count() == 0)
            {
                throw new AppException($"Role is required");
            }
            if (await userRepo.AnyAsync(u => u.Email == user.Email) && user.Email != _user.Email)
            {
                throw new AppException($"A user with email '{user.Email}' already exist");
            }

            var currentUser = accessor.HttpContext.GetUserSession();

            var oldUser = _user.Clone<User>();
            if (user.Id == currentUser.Id && _user.UserRoles.Any(ur=>ur.RoleId== (long)AppRoles.ADMINISTRATOR) && !(user.UserRoles.Any(r=>r.RoleId== (long)AppRoles.ADMINISTRATOR)))
            {
                throw new AppException($"You cannot unassign the Administartor role from yourself. You can make another user an administrator and then the user can unassign the role from you");
            }

           await  userRoleRepo.DeleteWhere(x => x.UserId == _user.Id && !user.UserRoles.Select(ur=>ur.RoleId).Contains(x.RoleId));
            var userRolesToAdd = user.UserRoles.Where(r => !userRoleRepo.Any(_r => _r.RoleId == r.RoleId));
            await userRoleRepo.InsertRange(userRolesToAdd);

            _user.FirstName = user.FirstName;
            _user.LastName = user.LastName;
            _user.PhoneNumber = user.PhoneNumber;
            _user.Email = user.Email;
            _user.Code = user.Code;
            _user.CompanyName = user.CompanyName;

            _user.UpdatedBy = currentUser.Id;
            _user.UpdatedDate = DateTimeOffset.Now;

            await userRepo.Update(_user, false);
            // log action
            await logger.LogActivity(
                ActivityActionType.UPDATE_USER,
                currentUser.Email, userRepo.TableName, oldUser, user, $"Updated user");
        }

        // update password
        public async Task UpdatePassword(PasswordRequestObject req)
        {
            if (req == null)
            {
                throw new AppException("Password object is required");
            }
            var user = await userRepo.GetById(req.UserId);
            if (user == null)
            {
                throw new AppException($"User with id '{req.UserId}' does not exist");
            }
            if (!passwordService.Verify(req.Password, user.Password))
            {
                throw new AppException($"Invalid current password");
            }
            if (!passwordService.ValidatePassword(req.NewPassword, out string passwordValidationMessage))
            {
                throw new AppException($"New password validation error: {passwordValidationMessage}");
            }
            if (!req.NewPasswordMatch)
            {
                throw new AppException($"New passwords don't match");
            }
            var currentUser = accessor.HttpContext.GetUserSession();
            user.Password = passwordService.Hash(req.NewPassword);
            user.UpdatedBy = currentUser.Id;
            user.UpdatedDate = DateTimeOffset.Now;

            await userRepo.Update(user, false);

            // log action
            await logger.LogActivity(ActivityActionType.UPDATE_PASSWORD,
                currentUser.Email, $"Updated password");
        }

        // reset password
        public async Task ResetPassword(long userId)
        {
            var user = await userRepo.GetById(userId);
            if (user == null)
            {
                throw new AppException($"User with id '{userId}' does not exist");
            }
            var newPassword = GeneratePassword();
            var currentUser = accessor.HttpContext.GetUserSession();
            user.Password = passwordService.Hash(newPassword);
            user.UpdatedBy = currentUser.Id;
            user.UpdatedDate = DateTimeOffset.Now;

            await userRepo.Update(user, false);

            // log action
            await logger.LogActivity(ActivityActionType.UPDATE_PASSWORD,
                currentUser.Email, $"Updated password");

            var mail = new MailObject
            {
                Recipients = new List<Recipient> {
                    new Recipient {
                        FirstName=user.FirstName,
                        LastName=user.LastName,
                        Email=user.Email,
                        Password= newPassword
                    }
                }
            };
            await mailService.SchedulePasswordResetMail(mail);

        }


        // authenticate user
        public async Task<bool> IsUserAuthentic(LoginCredential credential)
        {
            if (string.IsNullOrEmpty(credential?.Email))
            {
                throw new AppException($"Email/Username is required");
            }
            if (string.IsNullOrEmpty(credential?.Password))
            {
                throw new AppException($"Password is required");
            }
            var user = await userRepo.GetSingleWhere(u => u.Email == credential.Email);
            if (user == null)
            {
                throw new AppException($"Email/username is invalid");
            }
            if (!passwordService.Verify(credential.Password, user.Password))
            {
                throw new AppException($"Email/username is invalid");
            }
            return true;
        }

        // get user
        public async Task<User> GetUser(long userId)
        {
            var user = await userRepo.GetById(userId);
            //if (user == null)
            //    throw new AppException($"User with id: '{userId}' does not exist");
            //else
            return user;
        }

        public async Task<User> GetUser(string email)
        {
            var user = await userRepo.GetSingleWhere(u => u.Email == email);
            //if (user == null)
            //    throw new AppException($"User with email or username: '{email}' does not exist");
            //else
            return user;
        }

        // get users
        public IEnumerable<User> GetUsers(bool includeInactive = true)
        {
            var users = userRepo.GetAll();
            if (!includeInactive)
            {
                users = users.Where(u => u.IsActive);
            }
            return users;
        }

        public IEnumerable<User> GetUsers(long roleId, bool includeInactive = true)
        {
            var users = userRepo.GetWhere(u => u.UserRoles.Any(ur=>ur.RoleId==roleId));
            if (!includeInactive)
            {
                users = users.Where(u => u.IsActive);
            }
            return users;
        }

#warning send password recovery mail

        // delete user
        public async Task DeleteUser(long userId)
        {
            try
            {
                var user = await userRepo.GetById(userId);
                if (user == null)
                {
                    throw new AppException($"No user with id '{userId}' exist");
                }

                await userRepo.Delete(user.Id, false);

                var currentUser = accessor.HttpContext.GetUserSession();
                // log action
                await logger.LogActivity(ActivityActionType.DELETE_USER,
                    currentUser.Email, $"Deleted user with email '{user.Email}'");
            }
            catch (Exception ex)
            {
                logger.LogException(ex, ex.Message);
                throw new AppException($"User cannot be deleted as user is still associated with one or more entities");
            }
        }


        public async Task<bool> AnyUserExists()
        {
            return (await userRepo.Count()) > 0;
        }


        public async Task UpdateUserStatus(long userId, bool isActive)
        {
            var _user = await userRepo.GetById(userId);
            if (_user == null)
            {
                throw new AppException($"Invalid user id {userId}");
            }
            else
            {
                var currentUser = accessor.HttpContext.GetUserSession();
                if (userId == currentUser.Id)
                {
                    throw new AppException($"Sorry! You cannot change your own status");
                }
                var _olduser = _user.Clone<User>();


                _user.IsActive = isActive;
                _user.UpdatedBy = currentUser.Id;
                _user.UpdatedDate = DateTimeOffset.Now;

                await userRepo.Update(_user, false);


                // log activity
                await logger.LogActivity(ActivityActionType.UPDATE_USER, currentUser.Email,
                    userRepo.TableName, _olduser, _user,
                     $"Updated user status");

            }
        }


    }
}
