using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSchool.Core.SocialNetworks_.Repository
{
    public interface ISocialNetworkRepository
    {
        Task<IEnumerable<SocialNetwork>> GetAllAsync();
    }
}
