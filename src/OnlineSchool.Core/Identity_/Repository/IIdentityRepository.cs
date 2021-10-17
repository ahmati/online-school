using ItalWebConsulting.Infrastructure.DataAccess.EntityFramework;
using OnlineSchool.Contract.Identity.Models;
using OnlineSchool.Contract.Roles;
using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Identity_.Repository
{
    public interface IIdentityRepository : IRepositoryBase
    {
        Task<bool> UpdateAsync(ApplicationUser user);
        Task<bool> IsEmailConfirmedAsync(string userId);
        Task<bool> ConfirmEmailAsync(string userId);
        Task<ApplicationUser> FindByEmailAsync(string email);
        Task<ApplicationUser> FindByNameAsync(string userName);
        Task<bool> DeleteUserAsync(string userId);
        Task<ApplicationUser> FindByIdAsync(string userId);
        Task<bool> AddToRoleAsync(string userId, string roleId);
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        Task<IEnumerable<Ruoli>> GetUserRolesAsync(string userId);

        //Task<IEnumerable<AgentModelDdl>> GetAllAgentDllAsync();

        //IEnumerable<role> GetUserRole(string userId, bool includeSubscription);

        //Task<int> ForzaRefreshLastRequestDateAsync(long userId);

        //Task<IEnumerable<UserModelDdl>> GetAllUserDllAsync();

        // --------------------------------------- User SpotMeetings ---------------------------------------
        Task<BookedSpotMeeting> GetExistingBookedSpotMeetingAsync(string userEmail, int spotMeetingId);
        Task<IEnumerable<SpotMeeting>> GetUserSpotMeetingsAsync(string id);

    }
}
