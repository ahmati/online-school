using Dapper;
using Kendo.Mvc.Extensions;
using Microsoft.EntityFrameworkCore;
using OnlineSchool.Contract.Lessons;
using OnlineSchool.Domain.OnlineSchoolDB;
using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Lessons_.Repository
{
    class LessonRepository : OnlineSchoolBaseRepository, ILessonRepository
    {
        public async Task<Lesson> GetByIdAsync(int id)
        {
            try
            {
                var lesson = await Context.Lesson.FirstOrDefaultAsync(a => a.Id == id);
                return lesson;
            }
            catch (Exception e)
            {
                Logger.Error(e, "eErrore di imprevisto");
                return null;
            }
        }

        public async Task<Lesson> CreateAsync(Lesson model)
        {
            try
            {
                return await AddAsync(model);
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                return model;
            }
        }

        public async Task<bool> UpdateAsync(Lesson lesson)
        {
            try
            {
                var result = await base.UpdateAsync(lesson);
                return result > 0;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var lessonMaterials = await Context.LessonMaterial
                .Include(lm => lm.Material)
                .Where(lm => lm.LessonId == id)
                .ToListAsync();

            // 1. Remove from `LessonMaterial`
            foreach(var lessonMaterial in lessonMaterials)
            {
                Context.LessonMaterial.Remove(lessonMaterial);
            }
            await Context.SaveChangesAsync();

            // 2. Remove from `Material`
            foreach (var lessonMaterial in lessonMaterials)
            {
                Context.Material.Remove(lessonMaterial.Material);
            }
            await Context.SaveChangesAsync();

            // 3. Remove from `Lesson`
            var lesson = await Context.Lesson.FirstOrDefaultAsync(t => t.Id == id);
            if (lesson is null)
                return false;

            Context.Lesson.Remove(lesson);
            var result = await Context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<IEnumerable<Lesson>> GetLessonsByTeacherSubjectId(int teacherSubjectId)
        {
            var data = await Context.Lesson
                    .Include(t => t.TeacherSubject)
                    .Include(t => t.LessonMaterials)
                        .ThenInclude(lm => lm.Material)
                    .Where(t => t.TeacherSubjectId == teacherSubjectId)
                    .ToListAsync();

            return data;
        }

        public async Task<Lesson> GetByNameAsync(string name)
        {
            var data = await Context.Lesson.FirstOrDefaultAsync(s => s.Name == name);
            return data;
        }

        public async Task<Lesson> GetByNameAndTeacherSubjectIdAsync(string name, int teacherSubjectId)
        {
            var data = await Context.Lesson.FirstOrDefaultAsync(s => (s.Name == name && s.TeacherSubjectId == teacherSubjectId));
            return data;
        }
    }    
}
