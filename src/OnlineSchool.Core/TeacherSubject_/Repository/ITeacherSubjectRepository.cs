using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineSchool.Core.TeacherSubject_.Repository
{
    public interface ITeacherSubjectRepository
    {
        Task<TeacherSubject> GetByIdAsync(int teacherSubjectId);
        Task<TeacherSubject> GetByIdAsync(int teacherId, int subjectId);
        Task<IEnumerable<TeacherSubject>> GetAllAsync();
        Task<IEnumerable<TeacherSubject>> GetBySubjectIdAsync(int teacherId);
        Task<TeacherSubject> CreateAsync(TeacherSubject teacherSubject);
        Task<bool> DeleteAsync(int subjectId, int teacherId);
        Task<IEnumerable<TeacherSubjectMaterial>> GetMaterialsAsync(int id);
        Task<IEnumerable<LessonMaterial>> GetLessonMaterialsAsync(int id);
        Task<bool> DeleteMaterialAsync(int id);
        Task<bool> DeleteLessonMaterialAsync(int id);
        Task<TeacherSubjectMaterial> GetTeacherSubjectMaterialByFileNameAsync(int teacherSubjectId, string fileName);
        Task<LessonMaterial> GetLessonMaterialByFileNameAsync(int lessonId, string fileName);
        Task<TeacherSubjectMaterial> CreateTeacherSubjectMaterialAsync(TeacherSubjectMaterial model);
        Task<LessonMaterial> CreateLessonMaterialAsync(LessonMaterial model);
        Task<TeacherSubjectMaterial> GetTeacherSubjectMaterialByIdAsync(int id);
        Task<Lesson> GetLessonByTeacherSubjectIdAsync(int teacherSubjectId);
        Task<LessonMaterial> GetLessonMaterialByIdAsync(int id);
        Task<TeacherSubjectMaterial> GetTeacherSubjectMaterialByMaterialIdAsync(int materialId);
        Task<IEnumerable<TeacherSubjectMaterial>> GetTeacherSubjectMaterialsByIdsAsync(int[] ids);
        Task<IEnumerable<LessonMaterial>> GetLessonMaterialsByIdsAsync(int[] ids);
    }
}