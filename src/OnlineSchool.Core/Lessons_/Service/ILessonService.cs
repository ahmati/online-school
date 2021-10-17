using OnlineSchool.Contract.Infrastructure;
using OnlineSchool.Contract.Lessons;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Lessons_.Service
{
    public interface ILessonService
    {
        Task<LessonModel> GetByIdAsync(int id);
        Task<ResponseBase<LessonModel>> CreateAsync(LessonModel model);
        Task<LessonModel> GetByNameAsync(string name);
        Task<ResponseBase<bool>> DeleteAsync(int id);
        Task<ResponseBase<LessonModel>> UpdateAsync(LessonModel model);
        Task<IEnumerable<LessonModel>> GetLessonsByTeacherSubjectId(int teacherSubjectId);
        Task<LessonModel> GetByNameAndTeacherSubjectIdAsync(string name, int teacherSubjectId);
    }
}