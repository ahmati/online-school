using ItalWebConsulting.Infrastructure.DataAccess.EntityFramework;
using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Webex_.Repository
{
    public interface IWebexRepository : IRepositoryBase
    {
        Task<WebexIntegration> GetIntegrationAsync();
        Task<WebexGuestIssuer> GetGuestIssuerAsync();
        Task<bool> UpdateIntegrationAsync(WebexIntegration model);
    }
}
