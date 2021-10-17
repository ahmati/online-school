using Dapper;
using Microsoft.EntityFrameworkCore;
using OnlineSchool.Domain.OnlineSchoolDB;
using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Webex_.Repository
{
    public class WebexRepository : OnlineSchoolBaseRepository, IWebexRepository
    {
        public async Task<WebexGuestIssuer> GetGuestIssuerAsync()
        {
            return await Context.WebexGuestIssuer.FirstOrDefaultAsync();
        }

        public async Task<WebexIntegration> GetIntegrationAsync()
        {
            return await Context.WebexIntegration.FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateIntegrationAsync(WebexIntegration model)
        {
            var query = @"
                UPDATE WebexIntegration
                SET
                    AccessToken = @AccessToken,
                    ExpiresIn = @ExpiresIn,
                    LastUpdated = @LastUpdated
            ";

            var prms = new DynamicParameters();
            prms.Add("AccessToken", model.AccessToken);
            prms.Add("ExpiresIn", model.ExpiresIn);
            prms.Add("LastUpdated", model.LastUpdated);

            var result = await Context.Connection.ExecuteAsync(query, prms);
            return result > 0;
        }
    }
}


