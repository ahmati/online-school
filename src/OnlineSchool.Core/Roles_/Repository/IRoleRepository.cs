using ItalWebConsulting.Infrastructure.DataAccess.EntityFramework;
using OnlineSchool.Contract.Identity.Models;
using OnlineSchool.Contract.Roles;
using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Roles_.Repository
{
    public interface IRoleRepository : IRepositoryBase
    {
        Task<bool> RoleExists(string roleName);
        Task<bool> CreateRoleAsync(Ruoli role);
        Task<bool> UpdateRoleAsync(Ruoli role);
        Task<bool> DeleteRoleAsync(Ruoli role);
        Task<bool> DeleteRoleByIdAsync(string roleId);
        Task<Ruoli> GetRoleByIdAsync(string roleId);
        Task<Ruoli> GetRoleByNameAsync(string roleName);
        Task<IEnumerable<Ruoli>> GetAllRolesAsync();
        Task<IEnumerable<ApplicationUser>> GetAllUserRoles();
        Task<ApplicationUser> GetUserById(string id);
        Task<int> UpdateUser(ApplicationUser utente);
        Task<bool> DeleteUser(string id);
        Task<bool> CreateUserRoleAsync(AspNetUserRoles role);
        Task<bool> DeleteUserRoleAsync(string userId);
    }
}
