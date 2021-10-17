using OnlineSchool.Contract.Lessons;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Chapters_.Services
{
    public interface IChapterService 
    {
        Task<IEnumerable<LessonModel>> GetAllChaptersAsync();
        Task<LessonModel> GetChapterByIdAsync(int chapterId);
        Task<LessonModel> CreateChapterAsync(LessonModel model);
        Task<bool> UpdateChapterAsync(LessonModel model);
        Task<bool> DeleteChapterAsync(LessonModel model);
        Task<bool> DeleteChapterByIdAsync(int chapterId);
    }
}
