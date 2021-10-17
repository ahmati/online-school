using Microsoft.AspNetCore.Http;
using OnlineSchool.Contract.Courses;
using OnlineSchool.Contract.Infrastructure;
using OnlineSchool.Contract.Session;
using OnlineSchool.Contract.Students;
using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Students_.Service
{
    public interface IStudentService
    {
        Task<ResponseBase<StudentModel>> CreateAsync(CreateStudentModel student);
        Task<IEnumerable<StudentModel>> GetAllAsync();
        Task<StudentModel> GetByIdAsync(int id);
        Task<StudentModel> GetByEmailAsync(string email);
        Task<int> GetCountAsync();
        Task<ResponseBase<bool>> UpdateAsync(UpdateStudentModel model);
        Task<ResponseBase<bool>> DeleteByIdAsync (int id);
        /// <summary>
        /// Use this method to replace student image. It removes old image.
        /// </summary>
        /// <param name="file"> Image file. </param>
        /// <param name="studentId"> Student id. Neccessary to navigate to the appropriate folder. </param>
        /// <returns></returns>
        Task<ResponseBase<bool>> ReplaceImageAsync(IFormFile file, int studentId);

        // --------------------------------------- Student Courses ---------------------------------------
        Task<IEnumerable<CourseModel>> GetStudentCoursesAsync(int studentId);
        Task<BookedCourseModel> GetExistingBookedCourseAsync(string studentEmail, int courseId);

        // --------------------------------------- Student Sessions ---------------------------------------
        Task<IEnumerable<SessionModel>> GetUpcmonigSessions(int studentId);
        Task<SessionModel> GetSessionByIdAsync(int studentId, int sessionId);
       
    }
}
