using OnlineSchool.Contract.SocialNetworks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSchool.Core.SocialNetworks_.Service
{
    public interface ISocialNetworkService
    {
        Task<IEnumerable<SocialNetworkModel>> GetAllAsync();
    }
}
