using ItalWebConsulting.Infrastructure.DataAccess.EntityFramework;
using OnlineSchool.Contract.Material;
using OnlineSchool.Contract.SocialNetworks;
using OnlineSchool.Contract.Teachers;
using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Teachers_.Repository
{
    public interface ITeachersRepository : IRepositoryBase
    {
        Task<IEnumerable<Teacher>> GetAllAsync();
        Task<Teacher> GetByIdAsync(int id);
        Task<Teacher> GetByEmailAsync(string email);
        Task<Teacher> CreateAsync(Teacher model);
        Task<bool> UpdateAsync(Teacher teacher);
        Task<bool> DeleteByIdAsync(int id);
        Task<IEnumerable<TeacherSubject>> GetTeacherSubjectsAsync(int teacherId);
        Task<IEnumerable<Course>> GetTeacherCoursesAsync(int teacherId);

        // ----------------------------------- Teacher - Session -------------------------------------
        Task<Session> GetSessionByIdAsync(int teacherId, int sessionId);
        Task<IEnumerable<Session>> GetUpcomingSessionsAsync(int teacherId);

        // ----------------------------------- Teacher - TeacherSocialNetwork ------------------------
        Task<IEnumerable<TeacherSocialNetwork>> GetTeacherSocialNetworksAsync(int teacherId);
        Task<TeacherSocialNetwork> GetTeacherSocialNetworkByIdAsync(int teacherId, int socialNetworkId);
        Task<TeacherSocialNetwork> CreateTeacherSocialNetworkAsync(TeacherSocialNetwork teacherSocial);
        Task<bool> UpdateTeacherSocialNetworkAsync(TeacherSocialNetwork teacherSocial);
        Task<bool> DeleteTeacherSocialNetworkAsync(int teacherId, int socialNetworkId);

        // ----------------------------------- OLD ---------------------------------------------------
        Task<IEnumerable<MaterialModel>> GetTeacherComment();
        Task<IEnumerable<TeacherModel>> GetAllTeacherById(int id);
        Task<MaterialModel> CreateCommentAsync(MaterialModel model, string name, string body, int id, string teacherName);
        Task<TeacherModel> GetTeacherProfileById(int id);
        Task<List<TeacherModel>> GetAllTeachersAsync();
        Task<IEnumerable<TeacherModel>> GetTeacherAsync(int Id);
    }
}
