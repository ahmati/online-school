using OnlineSchool.Contract.Lessons;
using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Lessons_.Repository
{
    public interface ILessonRepository
    {
        Task<Lesson> GetByIdAsync(int id);
        Task<Lesson> CreateAsync(Lesson model);
        Task<Lesson> GetByNameAsync(string name);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateAsync(Lesson lesson);
        Task<IEnumerable<Lesson>> GetLessonsByTeacherSubjectId(int teacherSubjectId);
        Task<Lesson> GetByNameAndTeacherSubjectIdAsync(string name, int teacherSubjectId);
    }
}