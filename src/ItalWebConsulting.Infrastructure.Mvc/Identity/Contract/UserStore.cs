using OnlineSchool.Contract.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ItalWebConsulting.Infrastructure.Mvc.Identity.Contract
{
    public class UserStore 
        //: IUserStore<Utente, string>
                       
    {
        public Task CreateAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> FindByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> FindByNameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }
    }
}
