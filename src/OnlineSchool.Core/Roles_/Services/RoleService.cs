using ItalWebConsulting.Infrastructure.BusinessLogic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using OnlineSchool.Contract.Identity.Models;
using OnlineSchool.Contract.Roles;
using OnlineSchool.Core.Roles_.Repository;
using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Roles_.Services
{
    public class RoleService : CoreBase, IRoleService
    {
        public IRoleRepository RoleRepository { get; set; }

        //public RoleService(IRoleRepository roleRepository, IRoleStore<IdentityRole> store, IEnumerable<IRoleValidator<IdentityRole>> roleValidators=null, ILookupNormalizer keyNormalizer=null, IdentityErrorDescriber errors=null, ILogger<RoleManager<IdentityRole>> logger=null)
        //    : base(store, roleValidators, keyNormalizer, errors, logger)
        //{
        //    RoleRepository = roleRepository;
        //}
            
        public async Task<bool> CreateUserRole(IdentityRole role)
        {
            try
            {
                
                return await RoleRepository.CreateUserRoleAsync(Mapper.Map<AspNetUserRoles>(role));
            }
            catch (Exception e)
            {
                Logger.LogError("Errore di imprevisto", e.Message);
                return false;
            }
        }

        public async Task<bool> CreateRole(IdentityRole role)
        {
            try
            {
                return await RoleRepository.CreateRoleAsync(Mapper.Map<Ruoli>(role));
            }
            catch (Exception e)
            {
                Logger.LogError("Errore di imprevisto", e.Message);
                return false;
            }
        }

        public async Task<bool> DeleteRoleAsync(Ruoli role)
        {
            try
            {
                return await RoleRepository.DeleteRoleAsync(role);
            }
            catch (Exception e)
            {
                Logger.LogError("Errore di imprevisto", e.Message);
                return false;
            }
        }

        public async Task<bool> DeleteUserRoleAsync(string userId)
        {
            try
            {
                return await RoleRepository.DeleteUserRoleAsync(userId);
            }
            catch (Exception e)
            {
                Logger.LogError("Errore di imprevisto", e.Message);
                return false;
            }
        }

        public async Task<bool> DeleteRoleByIdAsync(string roleId)
        {
            try
            {
                return await RoleRepository.DeleteRoleByIdAsync(roleId);
            }
            catch (Exception e)
            {
                Logger.LogError("Errore di imprevisto", e.Message);
                return false;
            }
        }

        public async Task<IEnumerable<Ruoli>> GetAllRolesAsync()
        {
            try
            {
                return await RoleRepository.GetAllRolesAsync();
            }
            catch (Exception e)
            {
                Logger.LogError("Errore di imprevisto", e.Message);
                return null;
            }
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUserRoles()
        {
            try
            {
                return await RoleRepository.GetAllUserRoles();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<ApplicationUser> GetUserById(string id)
        {
            try
            {
                return await RoleRepository.GetUserById(id);

            }
            catch (Exception e)
            {
                Logger.LogError("Errore di imprevisto", e.Message);
                return null;
            }
        }

        public async Task<Ruoli> GetRoleByIdAsync(string roleId)
        {
            try
            {
                return await RoleRepository.GetRoleByIdAsync(roleId);
            }
            catch (Exception e)
            {
                Logger.LogError("Errore di imprevisto", e.Message);
                return null;
            }
        }

        public async Task<Ruoli> GetRoleByNameAsync(string roleName)
        {
            try
            {
                return await RoleRepository.GetRoleByNameAsync(roleName);
            }
            catch (Exception e)
            {
                Logger.LogError("Errore di imprevisto", e.Message);
                return null;
            }
        }

        public async Task<bool> RoleExists(string roleName)
        {
            try
            {
                return await RoleRepository.RoleExists(roleName);
            }
            catch (Exception e)
            {
                Logger.LogError("Errore di imprevisto", e.Message);
                return false;
            }
        }

        public async Task<bool> UpdateRoleAsync(Ruoli role)
        {
            try
            {
                return await RoleRepository.UpdateRoleAsync(role);
            }
            catch (Exception e)
            {
                Logger.LogError("Errore di imprevisto", e.Message);
                return false;
            }
        }

        public async Task<int> UpdateUser(ApplicationUser utente)
        {
            try
            {
                return await RoleRepository.UpdateUser(utente);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> DeleteUser(string id)
        {
            try
            {
                return await RoleRepository.DeleteUser(id);
            }
            catch (Exception e)
            {
                Logger.LogError("Errore di imprevisto", e.Message);
                return false;
            }
        }
    }
}
