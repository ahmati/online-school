using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineSchool.Contract.Identity.Models;
using OnlineSchool.Contract.Roles;
using OnlineSchool.Core.Roles_.Repository;
using OnlineSchool.Domain.OnlineSchoolDB;
using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace On.Core.Roles_.Repository
{
    public class RoleRepository : OnlineSchoolBaseRepository, IRoleRepository
    {
        public UserManager<ApplicationUser> UserManager { get; set; }

        public async Task<bool> CreateUserRoleAsync(AspNetUserRoles role)
        {
            try
            {       
                var entity = await base.AddAsync<AspNetUserRoles>(role);
                return entity != null;
            }
            catch (Exception e)
            {
                Logger.ErrorFormat("errore di imprevisto", e.Message);
                return false;
            }
        }

        public async Task<bool> DeleteUserRoleAsync(string userId)
        {
            try
            {
                var user = await Context.AspNetUserRoles.FirstOrDefaultAsync(u => u.UserId == userId);
                if (user == null)
                    return false;

                await base.DeleteAsync<AspNetUserRoles>(user);
                return true;
            }
            catch (Exception e)
            {
                Logger.ErrorFormat("errore di imprevisto", e.Message);
                return false;
            }
        }

        public async Task<bool> CreateRoleAsync(Ruoli role)
        {
            try
            {
                var entity = await base.AddAsync<AspNetRoles>(Mapper.Map<AspNetRoles>(role));
                return entity != null;
            }
            catch (Exception e)
            {
                Logger.ErrorFormat("errore di imprevisto", e.Message);
                return false;
            }
        }

        public async Task<bool> DeleteRoleAsync(Ruoli role)
        {
            try
            {
                await base.DeleteAsync<AspNetRoles>(Mapper.Map<AspNetRoles>(role));
                return true;
            }
            catch (Exception e)
            {
                Logger.ErrorFormat("errore di imprevisto", e.Message);
                return false;
            }
        }

        public async Task<bool> DeleteRoleByIdAsync(string roleId)
        {
            try
            {
                var entity = await (from u in Context.AspNetRoles
                                    where u.Id == roleId
                                    select u).FirstOrDefaultAsync();
                if (entity == null)
                {
                    return false;
                }
                await base.DeleteAsync<AspNetRoles>(entity);
                return true;
            }
            catch (Exception e)
            {
                Logger.ErrorFormat("errore di imprevisto", e.Message);
                return false;
            }
        }

        public async Task<IEnumerable<Ruoli>> GetAllRolesAsync()
        {
            try
            {
                var entities = await ListAllAsync<AspNetRoles>();
                return base.Mapper.Map<IEnumerable<Ruoli>>(entities);
            }
            catch (Exception e)
            {
                Logger.ErrorFormat("errore di imprevisto", e.Message);
                return null;

            }
        }

        //public async Task<IEnumerable<RoleUserModel>> GetAllUserRoles()
        //{
        //    try
        //    {
        //        var sQuery = new StringBuilder();
        //        sQuery.AppendLine("Select u.Id as UserId, u.UserName as Name, u.Email as Email,  ur.Name as RoleName");
        //        sQuery.AppendLine("From AspNetUsers u");
        //        sQuery.AppendLine("Inner Join AspNetUserRoles r on u.Id=r.UserId");
        //        sQuery.AppendLine("Inner Join AspNetRoles ur on r.RoleId=ur.Id");

        //        var entity = await base.Connection.QueryAsync<RoleUserModel>(sQuery.ToString()) as List<RoleUserModel>;
        //        return entity;
        //    }
        //    catch (Exception ex)
        //    {

        //        Logger.ErrorFormat("Errore di imprevisto", ex.Message);
        //        return new List<RoleUserModel>();
        //    }
        //}
        public async Task<IEnumerable<ApplicationUser>> GetAllUserRoles()
        {
            try
            {
                var sQuery = new StringBuilder();
                sQuery.AppendLine("Select u.Id , u.UserName, u.Email,  ur.Name as Role");
                sQuery.AppendLine("From AspNetUsers u");
                sQuery.AppendLine("Inner Join AspNetUserRoles r on u.Id=r.UserId");
                sQuery.AppendLine("Inner Join AspNetRoles ur on r.RoleId=ur.Id");

                var entity = await base.Connection.QueryAsync<ApplicationUser>(sQuery.ToString()) as List<ApplicationUser>;
                return entity;
            }
            catch (Exception ex)
            {

                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                return new List<ApplicationUser>();
            }
        }

        public async Task<ApplicationUser> GetUserById(string id)
        {
            var prms = new DynamicParameters();
            prms.Add("@UserId ", id);
            try
            {

                var sQuery = new StringBuilder();
                sQuery.AppendLine("Select *  from AspNetUsers ");
                sQuery.AppendLine($"Where Id = @UserId");
                var entities = base.Connection.QueryFirstOrDefaultAsync<ApplicationUser>(sQuery.ToString(), prms);
                return await entities;


            }
            catch (Exception e)
            {
                base.Logger.Error(e, "eErrore di imprevisto");
                return default;
            }
        }
        public async Task<Ruoli> GetRoleByIdAsync(string roleId)
        {
            try
            {
                var entity = await (from u in Context.AspNetRoles
                                    where u.Id == roleId
                                    select u).FirstOrDefaultAsync();
                return Mapper.Map<Ruoli>(entity);
            }
            catch (Exception e)
            {
                Logger.ErrorFormat("errore di imprevisto", e.Message);
                return null;

            }
        }

        public async Task<Ruoli> GetRoleByNameAsync(string roleName)
        {
            try
            {
                var entity = await (from r in Context.AspNetRoles
                                    where r.Name == roleName
                                    select r).FirstOrDefaultAsync();
                return Mapper.Map<Ruoli>(entity);
            }
            catch (Exception e)
            {
                Logger.ErrorFormat("errore di imprevisto", e.Message);
                return null;

            }
        }


        public async Task<bool> RoleExists(string roleName)
        {
            try
            {
                var entity = await (from r in Context.AspNetRoles
                                    where r.Name == roleName
                                    select r).FirstOrDefaultAsync();
                return entity != null;
            }
            catch (Exception e)
            {
                Logger.ErrorFormat("errore di imprevisto", e.Message);
                return false;

            }
        }

        public async Task<bool> UpdateRoleAsync(Ruoli role)
        {
            try
            {
                var entity = await base.UpdateAsync<AspNetRoles>(Mapper.Map<AspNetRoles>(role));
                return entity > 0;
            }
            catch (Exception e)
            {
                Logger.ErrorFormat("errore di imprevisto", e.Message);
                return false;
            }
        }

        public async Task<int> UpdateUser(ApplicationUser utente)
        {
            throw new NotImplementedException();
            //try
            //{
            //    var prms = new DynamicParameters();
            //    prms.Add("@UserId ", utente.Id);
            //    prms.Add("@Username", utente.UserName);
            //    prms.Add("@Email ", utente.Email);
            //    prms.Add("@Role ", utente.Role);

            //    var sb = new StringBuilder();
            //    sb.Append("UPDATE AspNetUserRoles set RoleId = @Role from AspNetUserRoles u inner join AspNetUsers s on u.UserId = s.ID Where s.Id = @UserId ; SELECT @@ROWCOUNT;");


            //    var result = await base.Connection.ExecuteScalarAsync<int>(sb.ToString(), prms);
            //    return result;

            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        public async Task<bool> DeleteUser(string id)
        {
            try
            {
                var entity = await (from u in Context.AspNetUsers
                                    where u.Id == id
                                    select u).FirstOrDefaultAsync();
                if (entity == null)
                {
                    return false;
                }
                await base.DeleteAsync<AspNetUsers>(entity);
                return true;
            }
            catch (Exception e)
            {
                Logger.ErrorFormat("errore di imprevisto", e.Message);
                return false;
            }
        }
    }
}

