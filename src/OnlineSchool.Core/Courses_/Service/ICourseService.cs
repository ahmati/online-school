using OnlineSchool.Contract.Contacts;
using OnlineSchool.Contract.Courses;
using OnlineSchool.Contract.Infrastructure;
using OnlineSchool.Contract.Session;
using OnlineSchool.Contract.Students;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Courses_
{
    public interface ICourseService : IPurchasableItem
    {
        Task<CourseModel> GetByIdAsync(int id);
        Task<IEnumerable<CourseModel>> GetAllAsync();
        Task<ResponseBase<CreateCourseModel>> CreateAsync(CreateCourseModel model);
        Task<ResponseBase<bool>> UpdateAsync(UpdateCourseModel model);
        Task<ResponseBase<bool>> DeleteAsync(int id);
        Task<ResponseBase<BookedCourseModel>> CreateBookedCourseAsync(BookedCourseModel model);
        Task<IEnumerable<BookedCourseModel>> GetBookedCoursesByTeacherSubjectIdAsync(int teacherSubjectId);
        Task<ResponseBase<bool>> PublishAsync(int courseId);
        Task<ResponseBase<bool>> UnpublishAsync(int id);
        Task<IEnumerable<SessionModel>> GetSessionsAsync(int courseId);
        Task<ResponseBase<SessionModel>> CreateSessionAsync(CreateSessionModel model);
        Task<ResponseBase<bool>> DeleteSessionAsync(int sessionId);
        Task<IEnumerable<CourseModel>> GetAllPublishedAsync();
        Task<IEnumerable<CourseModel>> GetAllPublishedAsync(string searchString);
    }
}
