using ItalWebConsulting.Infrastructure.BusinessLogic;
using Microsoft.Extensions.Logging;
using OnlineSchool.Contract.Lessons;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Chapters_.Services
{
    public class ChapterService : CoreBase, IChapterService
    {
        public IChapterService ChapterRepository { get; set; }

        public async Task<LessonModel> CreateChapterAsync(LessonModel model)
        {
            try
            {
                return await ChapterRepository.CreateChapterAsync(model);
            }
            catch (Exception ex)
            {

                Logger.LogError("errore di imprevisto.", ex.Message);
                return model;
            }
        }

        public async Task<bool> DeleteChapterAsync(LessonModel model)
        {
            try
            {
                return await ChapterRepository.DeleteChapterAsync(model);
            }
            catch (Exception ex)
            {

                Logger.LogError("errore di imprevisto.", ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteChapterByIdAsync(int chapterId)
        {
            try
            {
                return await ChapterRepository.DeleteChapterByIdAsync(chapterId);
            }
            catch (Exception ex)
            {

                Logger.LogError("errore di imprevisto.", ex.Message);
                return false;
            }
        }

        public async Task<IEnumerable<LessonModel>> GetAllChaptersAsync()
        {
            try
            {
                return await ChapterRepository.GetAllChaptersAsync();
            }
            catch (Exception ex)
            {

                Logger.LogError("errore di imprevisto.", ex.Message);
                return new List<LessonModel>();
            }
        }

        public async Task<LessonModel> GetChapterByIdAsync(int chapterId)
        {
            try
            {
                return await ChapterRepository.GetChapterByIdAsync(chapterId);
            }
            catch (Exception ex)
            {

                Logger.LogError("errore di imprevisto.", ex.Message);
                return null;
            }
        }

        public async Task<bool> UpdateChapterAsync(LessonModel model)
        {
            try
            {
                return await ChapterRepository.UpdateChapterAsync(model);
            }
            catch (Exception ex)
            {

                Logger.LogError("errore di imprevisto.", ex.Message);
                return false;
            }
        }
    }
}
