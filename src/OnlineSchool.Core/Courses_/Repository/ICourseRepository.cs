using ItalWebConsulting.Infrastructure.DataAccess.EntityFramework;
using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Subjects_.Repository
{
    public interface ICourseRepository : IRepositoryBase
    {
        Task<Course> GetByIdAsync(int id);
        Task<IEnumerable<Course>> GetAllAsync();
        Task<Course> CreateAsync(Course course);
        Task<bool> UpdateAsync(Course course);
        Task<bool> DeleteAsync(int id);
        Task<bool> GetByTeacherSubjectIdAsync(int id);
        Task<IEnumerable<BookedCourse>> GetBookedCoursesByTeacherSubjectIdAsync(int teacherSubjectId);
        Task<BookedCourse> CreateBookedCourseAsync(BookedCourse bookedCourse);
        Task<IEnumerable<Session>> GetSessionsAsync(int courseId);
        Task<Session> CreateSessionAsync(Session session);
        Task<IEnumerable<Course>> GetAllPublishedAsync();
        Task<IEnumerable<Course>> GetAllPublishedAsync(string searchString);
    }
}
