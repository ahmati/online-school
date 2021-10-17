using OnlineSchool.Contract.Courses;
using OnlineSchool.Contract.Infrastructure;
using OnlineSchool.Contract.Material;
using OnlineSchool.Contract.Session;
using OnlineSchool.Contract.SocialNetworks;
using OnlineSchool.Contract.Subject;
using OnlineSchool.Contract.Teachers;
using OnlineSchool.Contract.TeacherSubject;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Teachers_.Service
{
    public interface ITeacherService
    {
        Task<IEnumerable<TeacherModel>> GetAllAsync();
        Task<TeacherModel> GetByIdAsync(int id);
        Task<TeacherModel> GetByEmailAsync(string email);
        Task<ResponseBase<TeacherModel>> CreateAsync(CreateTeacherModel model);
        Task<ResponseBase<bool>> UpdateAsync(UpdateTeacherModel model);
        Task<ResponseBase<bool>> DeleteByIdAsync(int id);
        Task<IEnumerable<TeacherSubjectModel>> GetTeacherSubjectsAsync(int teacherId);
        Task<IEnumerable<CourseModel>> GetTeacherCoursesAsync(int teacherId);

        // --------------------------------------- Teacher Sessions ---------------------------------------
        Task<IEnumerable<SessionModel>> GetUpcomingSessionsAsync(int teacherId);
        Task<SessionModel> GetSessionByIdAsync(int teacherId, int sessionId);

        // --------------------------------------- TeacherSocialNetwork ---------------------------------------
        Task<IEnumerable<TeacherSocialNetworkModel>> GetTeacherSocialNetworksAsync(int teacherId);
        Task<ResponseBase<TeacherSocialNetworkModel>> CreateTeacherSocialNetworkAsync(TeacherSocialNetworkModel socialModel);
        Task<ResponseBase<bool>> UpdateTeacherSocialNetworkAsync(TeacherSocialNetworkModel socialModel);
        Task<ResponseBase<bool>> DeleteTeacherSocialNetworkAsync(int teacherId, int socialNetworkId);

        // ------------------------------------------------ OLD ------------------------------------------------
        Task<IEnumerable<MaterialModel>> GetTeacherComment();
        Task<IEnumerable<TeacherModel>> GetAllTeacherById(int id);
        Task<MaterialModel> CreateCommentAsync(MaterialModel model, string name, string body, int id, string teacherName);
        Task<TeacherModel> GetTeacherProfileById(int id);
        Task<List<TeacherModel>> GetAllTeachersAsync();
        Task<IEnumerable<TeacherModel>> GetTeacherAsync(int Id);
    }
}
