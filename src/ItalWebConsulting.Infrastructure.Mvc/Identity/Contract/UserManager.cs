using OnlineSchool.Contract.Identity.Models;
using ItalWebConsulting.Infrastructure.BusinessLogic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ItalWebConsulting.Infrastructure.Mvc.Identity.Contract
{
    public class UserManager : UserManager<ApplicationUser>, IUserManager
    {
        public UserManager(IUserStore<ApplicationUser> store, IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<ApplicationUser> passwordHasher, IEnumerable<IUserValidator<ApplicationUser>> userValidators,
            IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<ApplicationUser>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        public override async Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
        {
            return await base.CreateAsync(user, password);
        }

        public override async Task<IdentityResult> UpdateAsync(ApplicationUser user)
        {
            try
            {
                return await base.UpdateAsync(user);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public override async Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user)
        {
            return await base.GenerateEmailConfirmationTokenAsync(user);
        }
    }
}
   
