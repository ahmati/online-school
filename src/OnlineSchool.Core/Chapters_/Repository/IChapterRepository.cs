using ItalWebConsulting.Infrastructure.DataAccess.EntityFramework;
using OnlineSchool.Contract.Lessons;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Chapters_.Repository
{
    public interface IChapterRepository : IRepositoryBase
    {
        Task<IEnumerable<LessonModel>> GetAllChaptersAsync();
        Task<LessonModel> GetChaptersByIdAsync(int chapterId);
        Task<LessonModel> CreateChapterAsync(LessonModel model);
        Task<bool> UpdateChapterAsync(LessonModel model);
        Task<bool> DeleteChapterAsync(LessonModel model);
        Task<bool> DeleteChapterByIdAsync(int chapterId);


    }
}
