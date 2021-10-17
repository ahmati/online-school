using Dapper;
using OnlineSchool.Domain.OnlineSchoolDB;
using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSchool.Core.SocialNetworks_.Repository
{
    public class SocialNetworkRepository : OnlineSchoolBaseRepository, ISocialNetworkRepository
    {
        public async Task<IEnumerable<SocialNetwork>> GetAllAsync()
        {
            try
            {
                var sQuery = new StringBuilder();
                sQuery.AppendLine("SELECT * FROM SocialNetwork");

                var entities = await Connection.QueryAsync<SocialNetwork>(sQuery.ToString());
                return entities;
            }
            catch (Exception e)
            {
                Logger.Error(e, "eErrore di imprevisto");
                return null;
            }
        }
    }
}
