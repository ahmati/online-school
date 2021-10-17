using Microsoft.EntityFrameworkCore;
using OnlineSchool.Contract.Lessons;
using OnlineSchool.Domain.OnlineSchoolDB;
using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EF = OnlineSchool.Domain.OnlineSchoolDB.EF;

namespace OnlineSchool.Core.Chapters_.Repository
{
    public class ChapterRepository : OnlineSchoolBaseRepository, IChapterRepository
    {
        public async Task<LessonModel> CreateChapterAsync(LessonModel model)
        {
            try
            {
                var entity = Mapper.Map<EF.Lesson>(model);
                return Mapper.Map<LessonModel>(await AddAsync(entity));

            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                return model;
            }
        }

        public async Task<bool> DeleteChapterAsync(LessonModel model)
        {
            try
            {
                var entity = Mapper.Map<Lesson>(model);
                await DeleteAsync(entity);
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteChapterByIdAsync(int chapterId)
        {
            try
            {
                await DeleteByIdAsync<Lesson>(chapterId);
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                return false;
            }
        }

        public async Task<IEnumerable<LessonModel>> GetAllChaptersAsync()
        {
            try
            {
                var subjects = await Context.Lesson.ToListAsync();

                return Mapper.Map<List<LessonModel>>(subjects);
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                return new List<LessonModel>();
            }
        }

        public async Task<LessonModel> GetChaptersByIdAsync(int chapterId)
        {
            try
            {
                var model = await GetByIdAsync<EF.Lesson>(chapterId);
                return Mapper.Map<LessonModel>(model);
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                return null;
            }
        }

        public async Task<bool> UpdateChapterAsync(LessonModel model)
        {
            try
            {
                var entity = Mapper.Map<Lesson>(model);
                var result = await UpdateAsync(entity);
                return result > 0;

            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                return false;
            }
        }
    }
}
