using OnlineSchool.Contract.Infrastructure;
using OnlineSchool.Contract.Webex;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Webex_.Services
{
    public interface IWebexService
    {
        Task<WebexIntegrationModel> GetIntegrationAsync();
        Task<ResponseBase<bool>> UpdateIntegrationAsync(WebexIntegrationModel model);
        Task<ResponseBase<MeetingModel>> CreateMeetingAsync(CreateMeetingModel model);
        Task<ResponseBase<bool>> CheckHasMeetingStartedAsync(string meetingId);
        Task<ResponseBase<bool>> DeleteMeetingAsync(string meetingId);
        Task<WebexGuestIssuerModel> GetGuestIssuerAsync();
        Task<ResponseBase<string>> GenerateGuestTokenAsync(string email, string displayName);
    }
}
