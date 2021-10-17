using OnlineSchool.Contract.Students;
using OnlineSchool.Contract.Subject;
using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Students_.Repository
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetAllAsync();
        Task<Student> GetByIdAsync(int id);
        Task<Student> GetByEmailAsync(string email);
        Task<Student> CreateAsync(Student student);
        Task<bool> UpdateAsync(Student student);
        Task<bool> DeleteByIdAsync(int id);
        Task<int> GetCountAsync();

        // ---------------------------------- Student - Course ----------------------------------
        Task<IEnumerable<Course>> GetStudentCoursesAsync(int studentId);

        // ---------------------------------- Student - Session ---------------------------------
        Task<IEnumerable<Session>> GetUpcomingSessionsAsync(int studentId);
        Task<Session> GetSessionByIdAsync(int studentId, int sessionId);

        // ---------------------------------- OLD -----------------------------------------------
        Task<BookedCourse> GetExistingBookedCourse(string studentEmail, int courseId);
    }
}
