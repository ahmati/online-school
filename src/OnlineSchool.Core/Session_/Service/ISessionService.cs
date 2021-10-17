using OnlineSchool.Contract.Infrastructure;
using OnlineSchool.Contract.Session;
using OnlineSchool.Contract.Students;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Session_.Service
{
    public interface ISessionService
    {
        Task<IEnumerable<SessionModel>> GetSessionsThatNeedRemindingAsync();
        Task<SessionModel> GetByIdAsync(int sessionId);
        Task<ResponseBase<bool>> DeleteAsync(int sessionId);
        Task<IEnumerable<StudentModel>> GetStudentsByCourseIdAsync(int courseId);
        Task<ResponseBase<bool>> UpdateReminderEmailAsync(int sessionId);
    }
}
