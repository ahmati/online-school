using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OnlineSchool.Contract.Identity.Models;
using OnlineSchool.Contract.Roles;
using OnlineSchool.Contract.SpotMeeting;
using OnlineSchool.Core.Identity_.Repository;
//using InfoWeb.Infrastructure.Mvc.Identity.Contract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Identity_.Services
{
    public class IdentityService : SignInManager<ApplicationUser>, IIdentityService
    {
        public IIdentityRepository IdentityRepository { get; set; }
        public IMapper Mapper { get; set; }

        //private readonly SignInManager<IdentityUser> _signInManager;
        //public IdentityService(IUserStore<Utente> store, IOptions<IdentityOptions> optionsAccessor,
        //     IPasswordHasher<Utente> passwordHasher, IEnumerable<IUserValidator<Utente>> userValidators,
        //     IEnumerable<IPasswordValidator<Utente>> passwordValidators, ILookupNormalizer keyNormalizer,
        //     IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<Utente>> logger) 
        //    : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        //{

        //}

        public IdentityService(IIdentityRepository identityRepository, UserManager<ApplicationUser> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<ApplicationUser>> logger, IAuthenticationSchemeProvider schemes, IUserConfirmation<ApplicationUser> confirmation, IMapper mapper)
        : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {
            IdentityRepository = identityRepository;
            Mapper = mapper;
        }

        public async Task<IList<string>> GetRolesForLoggedUserAsync(ApplicationUser user)
        {
            try
            {
                return await base.UserManager.GetRolesAsync(user);
            }
            catch (Exception e)
            {
                Logger.LogError("Errore di imprevisto", e.Message);
                return null;
            }
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
        {
            try
            {
                return await UserManager.CreateAsync(user, password);
            }
            catch (Exception e)
            {
                Logger.LogError("Errore di imprevisto", e.Message);
                return IdentityResult.Failed();
            }
        }

        public async Task<string> CreateUtenteAsync(ApplicationUser user, string password)
        {
            try
            {
                var result = await UserManager.CreateAsync(user, password);
                return (await IdentityRepository.FindByEmailAsync(user.Email)).Id;

            }
            catch (Exception e)
            {
                Logger.LogError("Errore di imprevisto", e.Message);
                return null;
            }
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user)
        {
            try
            {
                return await base.UserManager.GenerateEmailConfirmationTokenAsync(user);
            }
            catch (Exception e)
            {
                Logger.LogError("Errore di imprevisto", e.Message);
                return string.Empty;
            }
        }

        public async Task<bool> AddToRoleAsync(string userId, string role)
        {
            try
            {
                return await IdentityRepository.AddToRoleAsync(userId, role);
            }
            catch (Exception e)
            {
                Logger.LogError("Errore di imprevisto", e.Message);
                return false;
            }
        }

        public Task<bool> CheckAuthenticatedUserAsync(string userId, string token)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ConfirmEmailAsync(string userId)
        {
            try
            {
                return await IdentityRepository.ConfirmEmailAsync(userId);
            }
            catch (Exception e)
            {
                Logger.LogError("Errore di imprevisto", e.Message);
                return false;
            }
        }

        public async Task<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string token)
        {
            try
            {
                return await base.UserManager.ConfirmEmailAsync(user, token);
            }
            catch (Exception e)
            {
                Logger.LogError("errore di imprevisto", e.Message);
                return IdentityResult.Failed();
            }
        }

        public async Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user)
        {
            try
            {
                return await base.UserManager.GeneratePasswordResetTokenAsync(user);
            }
            catch (Exception e)
            {
                Logger.LogError("Errore di imprevisto", e.Message);
                return string.Empty;
            }
        }

        public async Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword)
        {
            try
            {
                return await base.UserManager.ResetPasswordAsync(user, token, newPassword);
            }
            catch (Exception e)
            {
                Logger.LogError("Errore di imprevisto", e.Message);
                return IdentityResult.Failed();
            }
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            try
            {
                return await IdentityRepository.DeleteUserAsync(userId);
            }
            catch (Exception e)
            {
                Logger.LogError("Errore di imprevisto", e.Message);
                return false;
            }
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            try
            {
                return await IdentityRepository.FindByEmailAsync(email);
            }
            catch (Exception e)
            {
                Logger.LogError("Errore di imprevisto", e.Message);
                return null;
            }
        }

        public override Task<SignInResult> PasswordSignInAsync(ApplicationUser user, string password, bool isPersistent, bool lockoutOnFailure)
        {
            return base.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
        }

        public override Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
        {
            return base.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);
        }

        public async Task<ApplicationUser> FindByIdAsync(string userId)
        {
            try
            {
                return await IdentityRepository.FindByIdAsync(userId);
            }
            catch (Exception e)
            {
                Logger.LogError("Errore di imprevisto", e.Message);
                return null;
            }
        }

        public async Task<ApplicationUser> FindByNameAsync(string userName)
        {
            try
            {
                return await IdentityRepository.FindByNameAsync(userName);
            }
            catch (Exception e)
            {
                Logger.LogError("Errore di imprevisto", e.Message);
                return null;
            }
        }

        public async Task<bool> IsEmailConfirmedAsync(string userId)
        {
            try
            {
                return await IdentityRepository.IsEmailConfirmedAsync(userId);
            }
            catch (Exception e)
            {
                Logger.LogError("Errore di imprevisto", e.Message);
                return false;
            }
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationUser user)
        {
            try
            {
                
                return await base.UserManager.UpdateAsync(user);

            }
            catch (Exception e)
            {
                Logger.LogError("Errore di imprevisto", e.Message);
                return IdentityResult.Failed();
            }
        }

        public async Task<IEnumerable<Ruoli>> GetUserRolesAsync(string userId)
        {
            try
            {
                return await IdentityRepository.GetUserRolesAsync(userId);
            }
            catch (Exception e)
            {
                Logger.LogError("Errore di imprevisto", e.Message);
                return null;
            }
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            try
            {
                return await IdentityRepository.GetAllUsersAsync();
            }
            catch (Exception e)
            {
                Logger.LogError("Errore di imprevisto", e.Message);
                return new List<ApplicationUser>();
            }
        }

        public async Task<IdentityResult> RemoveFromRoleAsync(ApplicationUser user, string roleName)
        {
            try
            {
                return await base.UserManager.RemoveFromRoleAsync(user, roleName);
            }
            catch (Exception e)
            {
                Logger.LogError("Errore di imprevisto", e.Message);
                return IdentityResult.Failed();
            }
        }

        public async Task<bool> Logout()
        {
            try
            {
                await base.SignOutAsync();
                return true;
            }
            catch (Exception e)
            {
                Logger.LogError("Errore di imprevisto", e.Message);
                return false;
            }
        }


        #region User - SpotMeeting
        public async Task<BookedSpotMeetingModel> GetExistingBookedSpotMeetingAsync(string userEmail, int spotMeetingId)
        {
            try
            {
                var data = await IdentityRepository.GetExistingBookedSpotMeetingAsync(userEmail, spotMeetingId);
                return Mapper.Map<BookedSpotMeetingModel>(data);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error encountered.", ex.Message);
                throw ex;
            }
        }

        public async Task<IEnumerable<SpotMeetingModel>> GetUserSpotMeetingsAsync(string id)
        {
            try
            {
                var data = await IdentityRepository.GetUserSpotMeetingsAsync(id);
                return Mapper.Map<IEnumerable<SpotMeetingModel>>(data);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error encountered.", ex.Message);
                throw ex;
            }
        }
        #endregion
    }
}
