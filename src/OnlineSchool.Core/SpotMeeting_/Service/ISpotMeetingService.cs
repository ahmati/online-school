using Microsoft.AspNetCore.Http;
using OnlineSchool.Contract.Contacts;
using OnlineSchool.Contract.Infrastructure;
using OnlineSchool.Contract.SpotMeeting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSchool.Core.SpotMeeting_.Service
{
    public interface ISpotMeetingService : IPurchasableItem
    {
        Task<SpotMeetingModel> GetByIdAsync(int id);
        Task<IEnumerable<SpotMeetingModel>> GetAllAsync();
        Task<IEnumerable<SpotMeetingModel>> GetAllPublishedAsync();
        Task<IEnumerable<SpotMeetingModel>> GetAllRecursiveAsync();
        Task<IEnumerable<SpotMeetingModel>> GetSpotMeetingsThatNeedRemindingAsync();
        Task<IEnumerable<BookedSpotMeetingModel>> GetUsersBySpotMeetingAsync(int spotMeetingId);
        Task<IEnumerable<BookedSpotMeetingModel>> GetBookedSpotMeetingsBySpotMeetingIdAsync(int id);
        Task<ResponseBase<CreateSpotMeetingModel>> CreateAsync(CreateSpotMeetingModel model);
        Task<ResponseBase<bool>> UpdateReminderEmailAsync(int spotMeetingId);
        Task<ResponseBase<BookedSpotMeetingModel>> CreateBookedSpotMeetingAsync(BookedSpotMeetingModel model);
        Task<ResponseBase<bool>> UpdateAsync(UpdateSpotMeetingModel model);
        Task<ResponseBase<bool>> RenewSpotMeetingAsync(UpdateSpotMeetingModel model);
        Task<ResponseBase<bool>> DeleteAsync(int id);
        Task<ResponseBase<bool>> PublishAsync(int id);
        Task<ResponseBase<bool>> UnpublishAsync(int id);
        Task<ResponseBase<bool>> ReplaceImageAsync(IFormFile file, int spotMeetingId);

        #region ----------------------------------------- Material Actions -----------------------------------------
        Task<IEnumerable<SpotMeetingMaterialModel>> GetMaterialsAsync(int spotMeetingId);
        Task<ResponseBase<SpotMeetingMaterialModel>> GetSpotMeetingMaterialByIdAsync(int id);
        Task<ResponseBase<IEnumerable<SpotMeetingMaterialModel>>> GetSpotMeetingMaterialByIdsAsync(int[] ids);
        Task<ResponseBase<SpotMeetingMaterialModel>> GetSpotMeetingMaterialByFileNameAsync(int spotMeetingId, string fileName);
        Task<ResponseBase<SpotMeetingMaterialModel>> CreateSpotMeetingMaterialAsync(SpotMeetingMaterialModel model);
        Task<ResponseBase<bool>> DeleteSpotMeetingMaterialAsync(int id);
        #endregion

        #region ----------------------------------------- Teacher Actions -----------------------------------------
        Task<IEnumerable<SpotMeetingTeacherModel>> GetTeachersAsync(int spotMeetingId);
        Task<IEnumerable<SpotMeetingModel>> GetTeacherSpotMeetingsAsync(string name);
        Task<SpotMeetingTeacherModel> GetSpotMeetingTeacherAsync(string email, int id);
        Task<SpotMeetingTeacherModel> GetSpotMeetingTeacherByIdAsync(int id);
        Task<ResponseBase<SpotMeetingTeacherModel>> CreateSpotMeetingTeacherAsync(SpotMeetingTeacherModel model);
        Task<ResponseBase<bool>> DeleteSpotMeetingTeacherAsync(int id);
        #endregion
    }
}
