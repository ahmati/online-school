using OnlineSchool.Contract.Infrastructure;
using OnlineSchool.Contract.Lessons;
using OnlineSchool.Contract.TeacherSubject;
using OnlineSchool.Core.TeacherSubject_.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineSchool.Core.TeacherSubject_.Service
{
    public interface ITeacherSubjectService
    {
        Task<TeacherSubjectModel> GetByIdAsync(int teacherSubjectId);
        Task<TeacherSubjectModel> GetByIdAsync(int teacherId, int subjectId);
        Task<IEnumerable<TeacherSubjectModel>> GetAllAsync();
        Task<IEnumerable<TeacherSubjectModel>> GetBySubjectIdAsync(int subjectId);
        Task<ResponseBase<TeacherSubjectModel>> CreateAsync(TeacherSubjectModel model);
        Task<ResponseBase<bool>> DeleteAsync(int teacherId, int socialNetworkId);
        Task<IEnumerable<TeacherSubjectMaterialModel>> GetMaterialsAsync(int subjectId);
        Task<IEnumerable<LessonMaterialModel>> GetLessonMaterialsAsync(int subjectId);
        Task<ResponseBase<bool>> DeleteMaterialAsync(int id);
        Task<ResponseBase<bool>> DeleteLessonMaterialAsync(int id);
        Task<ResponseBase<TeacherSubjectMaterialModel>> GetTeacherSubjectMaterialByFileNameAsync(int teacherSubjectId, string fileName);
        Task<ResponseBase<LessonMaterialModel>> GetLessonMaterialByFileNameAsync(int lessonId, string fileName);
        Task<ResponseBase<TeacherSubjectMaterialModel>> CreateTeacherSubjectMaterialAsync(TeacherSubjectMaterialModel model);
        Task<ResponseBase<LessonMaterialModel>> CreateLessonMaterialAsync(LessonMaterialModel model);
        Task<ResponseBase<TeacherSubjectMaterialModel>> GetTeacherSubjectMaterialByIdAsync(int id);
        Task<ResponseBase<LessonModel>> GetLessonByTeacherSubjectIdAsync(int teacherSubjectId);
        Task<IEnumerable<LessonModel>> GetLessonsByTeacherSubjectAsync(int teacherSubjectId);
        Task<ResponseBase<LessonMaterialModel>> GetLessonMaterialByIdAsync(int id);
        Task<ResponseBase<TeacherSubjectMaterialModel>> GetTeacherSubjectMaterialByMaterialIdAsync(int materialId);
        Task<ResponseBase<IEnumerable<TeacherSubjectMaterialModel>>> GetTeacherSubjectMaterialsByIdsAsync(int[] ids);
        Task<ResponseBase<IEnumerable<LessonMaterialModel>>> GetLessonMaterialsByIdsAsync(int[] ids);
    }
}