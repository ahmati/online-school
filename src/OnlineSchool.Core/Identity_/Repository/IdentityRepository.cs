using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineSchool.Contract.Identity.Models;
using OnlineSchool.Contract.Roles;
using OnlineSchool.Domain.OnlineSchoolDB;
using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EF = OnlineSchool.Domain.OnlineSchoolDB.EF;

namespace OnlineSchool.Core.Identity_.Repository
{
    public class IdentityRepository : OnlineSchoolBaseRepository, IIdentityRepository
    {
        public UserManager<ApplicationUser> UserManager { get; set; }

        public IdentityRepository(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public async Task<bool> ConfirmEmailAsync(string userId)
        {
            var user = await Context.AspNetUsers.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return false;

            user.EmailConfirmed = true;
            Context.AspNetUsers.Update(user);
            var res = await Context.SaveChangesAsync();
            return res > 0;
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            var user = await UserManager.FindByEmailAsync(email);
            return user;
        }

        public async Task<ApplicationUser> FindByNameAsync(string userName)
        {
            var utente = await Context.AspNetUsers.FirstOrDefaultAsync(u => u.UserName == userName);
            return Mapper.Map<ApplicationUser>(utente);
        }

        public async Task<ApplicationUser> FindByIdAsync(string userId)
        {
            var user = await Context.AspNetUsers.FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null)
                return null;
            return Mapper.Map<ApplicationUser>(user);
        }

        public async Task<bool> IsEmailConfirmedAsync(string userId)
        {
            return (await Context.AspNetUsers.FirstOrDefaultAsync(u => u.Id == userId))?.EmailConfirmed ?? false;
        }

        public Task CreateAsync(ApplicationUser user)
        {
            throw new NotImplementedException();

        }

        public Task DeleteAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(ApplicationUser user)
        {
            try
            {
                var entity = await base.UpdateAsync(Mapper.Map<EF.AspNetUsers>(user));
                return entity > 0;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                return false;
            }
        }

        public async Task<IEnumerable<Ruoli>> GetUserRolesAsync(string userId)
        {
            try
            {
                var user = await Context.AspNetUsers
                    .Include(u => u.AspNetUserRoles)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user is null)
                    return null;

                return Mapper.Map<IEnumerable<Ruoli>>(user.AspNetUserRoles);
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                return new List<Ruoli>();
            }
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            try
            {
                var user = await Context.AspNetUsers.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                    return false;

                await base.DeleteAsync<EF.AspNetUsers>(user);
                return true;
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                return false;
            }
        }

        public async Task<bool> AddToRoleAsync(string userId, string roleId)
        {
            try
            {
                var data = new RuoliUtente() { RoleId = roleId, UserId = userId };
                var entity = Mapper.Map<EF.AspNetUserRoles>(data);

                var result = await AddAsync(entity);
                return result != null;
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                return false;
            }
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            try
            {
                var entities = await base.ListAllAsync<EF.AspNetUsers>();
                return base.Mapper.Map<IEnumerable<ApplicationUser>>(entities);
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                return new List<ApplicationUser>();
            }
        }

        //-----------------------------------------------------------------------------
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #region User - SpotMeeting
        public async Task<BookedSpotMeeting> GetExistingBookedSpotMeetingAsync(string userEmail, int spotMeetingId)
        {
            var bookedSpotMeeting = await Context.BookedSpotMeeting
                .Include(s => s.SpotMeeting)
                .FirstOrDefaultAsync(s => s.User.Email == userEmail && s.SpotMeetingId == spotMeetingId);
            return bookedSpotMeeting;
        }

        public async Task<IEnumerable<SpotMeeting>> GetUserSpotMeetingsAsync(string id)
        {
            try
            {
                var entities = await Context.BookedSpotMeeting
                    .Include(sm => sm.SpotMeeting)
                    .Where(sm => sm.UserId == id)
                    .ToListAsync();
                var result = Mapper.Map<IEnumerable<BookedSpotMeeting>>(entities);

                return result.Select(sm => sm.SpotMeeting);
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                throw e;
            }
        }
        #endregion
    }
}

