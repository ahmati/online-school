using OnlineSchool.Contract.Infrastructure;
using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Session_.Repository
{
    public interface ISessionRepository
    {
        Task<IEnumerable<Session>> GetSessionsThatNeedRemindingAsync();
        Task<Session> GetByIdAsync(int sessionId);
        Task<bool> DeleteAsync(int sessionId);
        Task<IEnumerable<Student>> GetStudentsByCourseIdAsync(int courseId);
        Task<bool> UpdateReminderEmailAsync(int sessionId);
    }
}
