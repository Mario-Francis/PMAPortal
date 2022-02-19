using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using PMAPortal.Web.DTOs;
using PMAPortal.Web.Extensions;
using PMAPortal.Web.Services;
using PMAPortal.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace PMAPortal.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILoggerService<AuthController> logger;
        private readonly IUserService userService;

        public AuthController(ILoggerService<AuthController> logger,
            IUserService userService)
        {
            this.logger = logger;
            this.userService = userService;
        }
        public async Task<IActionResult> Index(string returnUrl = null)
        {
            if (!await userService.AnyUserExists())
            {
                return RedirectToAction("Setup");
            }
            else if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        public async Task<IActionResult> Setup()
        {
            if (await userService.AnyUserExists())
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Setup(SetupVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errs = ModelState.Values.Where(v => v.Errors.Count > 0).Select(v => v.Errors.First().ErrorMessage);
                    return StatusCode(400, new { IsSuccess = false, Message = "One or more fields failed validation", ErrorItems = errs });
                }
                else
                {
                    if (model.Password != model.ConfirmPassword)
                    {
                        throw new AppException($"Passwords don't match");
                    }

                    await userService.InitialUserSetup(model.ToUser());
                    return Ok(new { IsSuccess = true, Message = "Setup completed successfully", ErrorItems = new string[] { } });
                }
            }
            catch (AppException ex)
            {
                return StatusCode(400, new { IsSuccess = false, Message = ex.Message, ErrorDetail = JsonSerializer.Serialize(ex.InnerException) });
            }
            catch (Exception ex)
            {
                //await loggerService.LogException(ex);
                //await loggerService.LogError(ex.GetErrorDetails());
                logger.LogException(ex, "An error was encountered during initial app setup");

                return StatusCode(500, new { IsSuccess = false, Message = ex.Message, ErrorDetail = JsonSerializer.Serialize(ex.InnerException) });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errs = ModelState.Values.Where(v => v.Errors.Count > 0).Select(v => v.Errors.First().ErrorMessage);
                    return StatusCode(400, new { IsSuccess = false, Message = "One or more fields failed validation", ErrorItems = errs });
                }
                else
                {
                    var isValid = await userService.IsUserAuthentic(new LoginCredential { Email = model.Email, Password = model.Password });
                    var user = await userService.GetUser(model.Email);
                    var sessionObject = SessionObject.FromUser(user);

                    ClaimsIdentity identity = new ClaimsIdentity(Constants.AUTH_COOKIE_ID);
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, sessionObject.Email));
                    identity.AddClaim(new Claim(ClaimTypes.Email, sessionObject.Email));
                    identity.AddClaim(new Claim(ClaimTypes.Surname, sessionObject.LastName));
                    identity.AddClaim(new Claim(ClaimTypes.GivenName, sessionObject.FirstName));
                    identity.AddClaim(new Claim(ClaimTypes.Name, sessionObject.FullName));
                    identity.AddClaim(new Claim(ClaimTypes.Sid, sessionObject.Id.ToString()));
                    identity.AddClaim(new Claim(ClaimTypes.Actor, Constants.USER_TYPE_USER));

                    foreach(var r in sessionObject.Roles)
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, r));
                    }
                    

                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(Constants.AUTH_COOKIE_ID, principal);

                    return Ok(new { IsSuccess = true, Message = "You were logged in successfully", ErrorItems = new string[] { } });
                }
            }
            catch (AppException ex)
            {
                return StatusCode(400, new { IsSuccess = false, Message = ex.Message, ErrorDetail = JsonSerializer.Serialize(ex.InnerException) });
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "An error was encountered during initial app setup");

                return StatusCode(500, new { IsSuccess = false, Message = ex.Message, ErrorDetail = JsonSerializer.Serialize(ex.InnerException) });
            }
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.ClearUserSession();
            await HttpContext.SignOutAsync(Constants.AUTH_COOKIE_ID);

            return RedirectToAction("Index");
        }

    }
}
