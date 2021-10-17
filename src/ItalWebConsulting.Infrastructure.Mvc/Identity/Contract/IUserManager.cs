
using OnlineSchool.Contract.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ItalWebConsulting.Infrastructure.Mvc.Identity.Contract
{
    public interface IUserManager
    {
        Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user);
        Task<IdentityResult> CreateAsync(ApplicationUser user,string password);
        Task<IdentityResult> UpdateAsync(ApplicationUser user);
    }
}
