using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSchool.Core.SpotMeeting_.Repository
{
    public interface ISpotMeetingRepository
    {
        Task<SpotMeeting> GetByIdAsync(int id);
        Task<IEnumerable<SpotMeeting>> GetAllAsync();
        Task<IEnumerable<SpotMeeting>> GetAllPublishedAsync();
        Task<IEnumerable<SpotMeeting>> GetAllRecursiveAsync();
        Task<SpotMeetingTeacher> GetHostTeacherAsync(int id);
        Task<IEnumerable<SpotMeeting>> GetSpotMeetingsThatNeedRemindingAsync();
        Task<IEnumerable<BookedSpotMeeting>> GetUsersBySpotMeetingAsync(int spotMeetingId);
        Task<IEnumerable<BookedSpotMeeting>> GetBookedSpotMeetingsBySpotMeetingIdAsync(int id);
        Task<SpotMeeting> CreateAsync(SpotMeeting spotMeeting);
        Task<bool> UpdateReminderEmailAsync(int spotMeetingId);
        Task<BookedSpotMeeting> CreateBookedSpotMeetingAsync(BookedSpotMeeting bookedSpotMeeting);
        Task<bool> UpdateAsync(SpotMeeting spotMeeting);
        Task<bool> DeleteAsync(int id);

        #region ----------------------------------------- Material Actions -----------------------------------------
        Task<IEnumerable<SpotMeetingMaterial>> GetMaterialsAsync(int id);
        Task<SpotMeetingMaterial> GetSpotMeetingMaterialByIdAsync(int id);
        Task<IEnumerable<SpotMeetingMaterial>> GetSpotMeetingMaterialByIdsAsync(int[] ids);
        Task<SpotMeetingMaterial> GetSpotMeetingMaterialByFileNameAsync(int spotMeetingId, string fileName);
        Task<SpotMeetingMaterial> CreateSpotMeetingMaterialAsync(SpotMeetingMaterial spotMeetingMaterial);
        Task<bool> DeleteSpotMeetingMaterialAsync(int id);
        #endregion

        #region ----------------------------------------- Teacher Actions -----------------------------------------
        Task<IEnumerable<SpotMeetingTeacher>> GetTeachersAsync(int id);
        Task<int[]> GetSpotMeetingIdsByTeacherEmailAsync(string email);
        Task<IEnumerable<SpotMeeting>> GetTeacherSpotMeetingsAsync(int[] spotmeetingIds);
        Task<SpotMeetingTeacher> GetSpotMeetingTeacherAsync(string email, int id);
        Task<SpotMeetingTeacher> GetSpotMeetingTeacherByIdAsync(int id);
        Task<SpotMeetingTeacher> GetSpotMeetingTeacherByIdsAsync(int spotMeetingId, int teacherId);
        Task<SpotMeetingTeacher> CreateSpotMeetingTeacherAsync(SpotMeetingTeacher spotMeetingMaterial);
        Task<bool> UpdateSpotMeetingTeacherAsync(SpotMeetingTeacher spotMeetingTeacher);
        Task<bool> DeleteSpotMeetingTeacherAsync(int id);
        #endregion
    }
}
