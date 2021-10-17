using Microsoft.AspNetCore.Identity;
using OnlineSchool.Contract.Identity.Models;
using OnlineSchool.Contract.Roles;
using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Roles_.Services
{
    public interface IRoleService
    {
        Task<bool> RoleExists(string roleName);
        Task<bool> UpdateRoleAsync(Ruoli role);
        Task<bool> DeleteRoleAsync(Ruoli role);
        Task<bool> DeleteRoleByIdAsync(string roleId);
        Task<Ruoli> GetRoleByIdAsync(string roleId);
        Task<IEnumerable<Ruoli>> GetAllRolesAsync();
        Task<bool> CreateRole(IdentityRole role);
        Task<Ruoli> GetRoleByNameAsync(string roleName);
        Task<IEnumerable<ApplicationUser>> GetAllUserRoles();
        Task<ApplicationUser> GetUserById(string id);
        Task<int> UpdateUser(ApplicationUser utente);
        Task<bool> DeleteUser(string id);
        Task<bool> CreateUserRole(IdentityRole role);
        Task<bool> DeleteUserRoleAsync(string userId);
    }
}
