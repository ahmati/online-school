using Models= OnlineSchool.Contract.Identity.Models;
using OnlineSchool.Contract.Roles;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OnlineSchool.Contract.Identity.Models;
using OnlineSchool.Contract.SpotMeeting;

namespace OnlineSchool.Core.Identity_.Services
{
    public interface IIdentityService
    {
        Task<IList<string>> GetRolesForLoggedUserAsync(ApplicationUser user);
        Task<bool> Logout();
        Task<string> CreateUtenteAsync(ApplicationUser user, string password);
        Task<ApplicationUser> FindByIdAsync(string userId);
        Task<bool> IsEmailConfirmedAsync(string userId);
        Task<bool> ConfirmEmailAsync(string userId);
        Task<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string token);
        Task<ApplicationUser> FindByEmailAsync(string email);
        Task<IdentityResult> UpdateAsync(ApplicationUser user);
        Task<bool> DeleteUserAsync(string userId);
        Task<ApplicationUser> FindByNameAsync(string userName);
        Task<bool> CheckAuthenticatedUserAsync(string userId, string token);
       
        Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user);
        Task<IdentityResult> CreateAsync(ApplicationUser user, string password);
        Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user);
        Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword);
        Task<bool> AddToRoleAsync(string userId, string roleId);
        Task<IdentityResult> RemoveFromRoleAsync(ApplicationUser user, string roleName);
        Task<IEnumerable<Ruoli>> GetUserRolesAsync(string userId);
        Task<SignInResult> PasswordSignInAsync(ApplicationUser user, string password, bool isPersistent, bool lockoutOnFailure);
        Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure);
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();

        // --------------------------------------- User SpotMeetings ---------------------------------------
        Task<BookedSpotMeetingModel> GetExistingBookedSpotMeetingAsync(string userEmail, int spotMeetingId);
        Task<IEnumerable<SpotMeetingModel>> GetUserSpotMeetingsAsync(string id);
    }

}
