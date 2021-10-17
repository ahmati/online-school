using ItalWebConsulting.Infrastructure.Comunication;
using ItalWebConsulting.Infrastructure.Extension;
using ItalWebConsulting.Infrastructure.Mvc.Identity.Contract;
using ItalWebConsulting.Infrastructure.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineSchool.Contract.Identity.Models;
using OnlineSchool.Contract.Infrastructure.Enums;
using OnlineSchool.Contract.Students;
using OnlineSchool.Core.Identity_.Services;
using OnlineSchool.Core.Roles_.Services;
using OnlineSchool.Core.Students_.Service;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using IW = ItalWebConsulting.Infrastructure.Mvc;
using OnlineSchool.Contract.Resources;
using System.Text;
using OnlineSchool.Contract.Identity.Validators;

namespace OnlineSchool.Site.Controllers
{

    [AllowAnonymous]
    public class AccountController : IW.ControllerBase<AccountController>
    {
        public IEmailService EmailService { get; set; }
        public IIdentityService IdentityService { get; set; }
        public IRoleService RoleService { get; set; }
        public IStudentService StudentService { get; set; }
        
        public UserManager<ApplicationUser> _userManager { get; set; }

        
        public IActionResult Register()
        {
            return View(new RegisterUtente());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUtente input)
        {
            if (ModelState.IsValid)
            {
                if (!Helpers.IsGenderValid(input.Gender))
                {
                    ModelState.AddModelError(string.Empty, "Invalid gender value.");
                    return View(input);
                }

                var user = Mapper.Value.Map<CreateStudentModel>(input);

                var result = await StudentService.CreateAsync(user);
                if (!result.HasErrors)
                {
                    Logger.LogInformation($"User `{user.Email}` created successfully.");

                    var aspuser = await IdentityService.FindByEmailAsync(user.Email);
                    var code = await IdentityService.GenerateEmailConfirmationTokenAsync(aspuser);

                    var _callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = aspuser.Id, code = code }, protocol: Request.Scheme);

                    StringBuilder confirmEmail = new StringBuilder(EmailSharedResources.ConfirmEmailLink);
                    confirmEmail.Replace("HtmlEncoder", HtmlEncoder.Default.Encode(_callbackUrl));

                    await EmailService.SendEmailAsync(input.Email, EmailSharedResources.ConfirmEmail, confirmEmail.ToString());

                    TempData["MessageSuccess"] = EmailSharedResources.UserCreationSucces;
                    return Redirect("/Account/Login"); ;
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(input);
        }

        public IActionResult Login()
        {
            return View(new LoginUtente());
        }

        [HttpPost]
        public JsonResult Index(string Prefix)
        {
            var cookie = Request.Cookies.Where(x => x.Key == Prefix).FirstOrDefault();
            if (!cookie.Equals(null))
            {
                return Json(cookie.Value);
            }

            return Json("");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUtente model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var user = await IdentityService.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    await _userManager.UpdateSecurityStampAsync(user);
                    if (!await IdentityService.IsEmailConfirmedAsync(user.Id))
                    {
                        ModelState.AddModelError(string.Empty, ValidationSharedResources.MustConfirmEmailError);

                        var code = await IdentityService.GenerateEmailConfirmationTokenAsync(user);

                        var _callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Scheme);

                        StringBuilder confirmEmail = new StringBuilder (EmailSharedResources.ConfirmEmailLink);
                        confirmEmail.Replace("HtmlEncoder", HtmlEncoder.Default.Encode(_callbackUrl));

                        await EmailService.SendEmailAsync(model.Email, EmailSharedResources.ConfirmEmail,
                            confirmEmail.ToString() );
                        
                        return View();
                    }
                }

                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await IdentityService.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (model.RememberMe == true)
                {
                    Response.Cookies.Append(model.Email, model.Password);
                }

                if (result.Succeeded)
                {
                    Logger.LogInformation("User logged in.");

                    var roles = await IdentityService.GetRolesForLoggedUserAsync(user);

                    if (returnUrl == null)
                        return LocalRedirect("/");

                    else
                        return LocalRedirect(returnUrl);
                }
                else
                {
                    Logger.LogError(ValidationSharedResources.InvalidLogin);

                    ModelState.AddModelError(string.Empty, ValidationSharedResources.InvalidLogin);
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View(new ForgotPassword());
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPassword Input)
        {
            if (ModelState.IsValid)
            {
                var user = await IdentityService.FindByEmailAsync(Input.Email);
                // Don't reveal that the user does not exist or is not confirmed
                if (user == null || !(await IdentityService.IsEmailConfirmedAsync(user.Id)))
                {
                    ViewData["MessageError"] = ValidationSharedResources.UserNotRegistredOrEmailNotConfirmed;
                    return View();
                }

                var code = await IdentityService.GeneratePasswordResetTokenAsync(user);

                var callbackUrl = $"/Account/ResetPassword?userId={user.Id}&code={code}";
                callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Scheme);

                var resetPass = EmailSharedResources.ResetPasswordEmail;
                resetPass = resetPass.Replace("HtmlEncoder", HtmlEncoder.Default.Encode(callbackUrl));

                await EmailService.SendEmailAsync(Input.Email, EmailSharedResources.ResetPassword,resetPass);

                ViewData["MessageSuccess"] = ValidationSharedResources.ResetPasswordEmailSent;
                return View();
            }

            ViewData["Message"] = ValidationSharedResources.CheckEmailError;
            ModelState.AddModelError(string.Empty, ValidationSharedResources.CheckEmailError);

            return View();
        }

        [AllowAnonymous]
        public IActionResult ResetPassword(string code)
        {
            ViewBag.Code = code;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPassword Input)
        {
            if (ModelState.IsValid)
            {
                var user = await IdentityService.FindByEmailAsync(Input.Email);
                if (user == null || !(await IdentityService.IsEmailConfirmedAsync(user.Id)))
                {
                    if (user == null)
                        ViewData["MessageError"] = ValidationSharedResources.UserNotFoundError;
                    else
                        ViewData["MessageError"] = ValidationSharedResources.MustConfirmEmailError;

                    return View();
                }

                var result = await IdentityService.ResetPasswordAsync(user, Input.Code, Input.Password);
                if (result.Succeeded)
                {
                    TempData["MessageSuccess"] = ValidationSharedResources.ResetPasswordSuccess;

                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);

                    ViewData["MessageError"] = ValidationSharedResources.TryAgainError;
                }
            }

            return View();
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            var user = await IdentityService.FindByIdAsync(userId);
            if (user == null)
            {
                StringBuilder loadUserError = new StringBuilder(ValidationSharedResources.LoadUserError);
                loadUserError.Replace("userId", userId);
                ViewData["MessageError"] = loadUserError.ToString();
                return View();
            }

            var isEmailConfirmed1 = user.EmailConfirmed ;
            if (isEmailConfirmed1)
            {
                ViewData["MessageSuccess"] = ValidationSharedResources.EmailConfirmedOnce;
                return View();
            }

            var result = await IdentityService.ConfirmEmailAsync(user.Id);
            if (result)
            {
                ViewBag.MesaggeSuccess = ValidationSharedResources.ConfirmEmailSuccess;
                return Redirect("/Account/Login");
            }
            else
            {
                ViewData["MessageError"] = ValidationSharedResources.TryAgainEmailConfirmation;
                //foreach (var error in result.Errors)
                //{
                //    ModelState.AddModelError(string.Empty, error.Description);
                //}
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await IdentityService.Logout();
            return RedirectToAction("Login", "Account");
        }
    }
}