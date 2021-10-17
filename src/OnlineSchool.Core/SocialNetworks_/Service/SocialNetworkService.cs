using ItalWebConsulting.Infrastructure.BusinessLogic;
using Microsoft.Extensions.Logging;
using OnlineSchool.Contract.SocialNetworks;
using OnlineSchool.Core.SocialNetworks_.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSchool.Core.SocialNetworks_.Service
{
    public class SocialNetworkService : CoreBase, ISocialNetworkService
    {
        public ISocialNetworkRepository SocialNetworkRepository { get; set; }

        public async Task<IEnumerable<SocialNetworkModel>> GetAllAsync()
        {
            try
            {
                var data = await SocialNetworkRepository.GetAllAsync();
                return Mapper.Map<IEnumerable<SocialNetworkModel>>(data);

            }
            catch (Exception ex)
            {
                Logger.LogError("errore di imprevisto.", ex.Message);
                return null;
            }
        }
    }
}
