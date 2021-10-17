using Microsoft.EntityFrameworkCore;
using OnlineSchool.Contract.TeacherSubject;
using OnlineSchool.Domain.OnlineSchoolDB;
using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSchool.Core.TeacherSubject_.Repository
{
    public class TeacherSubjectRepository : OnlineSchoolBaseRepository, ITeacherSubjectRepository
    {
        public async Task<TeacherSubject> GetByIdAsync(int teacherSubjectId)
        {
            try
            {
                var data = await Context.TeacherSubject
                    .Include(t => t.Teacher)
                    .Include(t => t.Subject)
                    .Include(t => t.TeacherSubjectMaterials)
                        .ThenInclude(tsm => tsm.Material)
                    .Include(t => t.Lessons)
                        .ThenInclude(l => l.LessonMaterials)
                    .Where(t => t.Id == teacherSubjectId && t.Deleted == false)
                    .FirstOrDefaultAsync();

                return data;
            }
            catch (Exception e)
            {
                Logger.Error(e, "eErrore di imprevisto");
                return null;
            }
        }

        public async Task<TeacherSubject> GetByIdAsync(int teacherId, int subjectId)
        {
            try
            {
                var data = await Context.TeacherSubject
                    .Include(t => t.Teacher)
                    .Include(t => t.Subject)
                    .Include(t => t.TeacherSubjectMaterials)
                        .ThenInclude(tsm => tsm.Material)
                    .Include(t => t.Lessons)
                        .ThenInclude(l => l.LessonMaterials)
                    .Where(t => t.TeacherId == teacherId && t.SubjectId == subjectId && t.Deleted == false)
                    .FirstOrDefaultAsync();

                return data;
            }
            catch (Exception e)
            {
                Logger.Error(e, "eErrore di imprevisto");
                return null;
            }
        }

        public async Task<IEnumerable<TeacherSubject>> GetAllAsync()
        {
            try
            {
                var data = await Context.TeacherSubject
                    .Include(t => t.Teacher)
                    .Include(t => t.Subject)
                    .ToListAsync();

                return data;
            }
            catch (Exception e)
            {
                Logger.Error(e, "eErrore di imprevisto");
                return null;
            }
        }

        public async Task<IEnumerable<TeacherSubject>> GetBySubjectIdAsync(int subjectId)
        {

            try
            {
                var data = await Context.TeacherSubject
                    .Include(t => t.Teacher)
                    .Where(t => t.SubjectId == subjectId && t.Deleted == false)
                    .ToListAsync();

                return data;
            }
            catch (Exception e)
            {
                Logger.Error(e, "eErrore di imprevisto");
                return null;
            }
        }

        public async Task<TeacherSubject> CreateAsync(TeacherSubject teacherSubject)
        {
            try
            {
                var result = await Context.TeacherSubject
                    .FirstOrDefaultAsync(t => t.TeacherId == teacherSubject.TeacherId && t.SubjectId == teacherSubject.SubjectId);
                if (result != null)
                {
                    result.Deleted = false;
                    Context.TeacherSubject.Update(result);
                    await Context.SaveChangesAsync();
                    return result;
                }
                return await base.AddAsync(teacherSubject);
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                return null;
            }
        }

        public async Task<bool> DeleteAsync(int subjectId, int teacherId)
        {
            try
            {
                var result =await Context.TeacherSubject.FirstOrDefaultAsync(t => t.TeacherId == teacherId && t.SubjectId == subjectId);
                result.Deleted = true;
                Context.TeacherSubject.Update(result);
                await Context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                return false;
            }
        }

        //--------------------------------------------------------Material Actions--------------------------------------------------------------------------------

        public async Task<TeacherSubjectMaterial> GetTeacherSubjectMaterialByIdAsync(int id)
        {
            try
            {
                var data = await Context.TeacherSubjectMaterial
                    .Include(t => t.TeacherSubject)
                    .Include(t => t.Material)
                    .Where(t => t.Id == id)
                    .FirstOrDefaultAsync();

                return data;
            }
            catch (Exception e)
            {
                Logger.Error(e, "eErrore di imprevisto");
                return null;
            }
        }

        public async Task<IEnumerable<TeacherSubjectMaterial>> GetMaterialsAsync(int id)
        {
            try
            {
                var data = await Context.TeacherSubjectMaterial
                    .Include(t => t.Material)
                    .Include(t => t.TeacherSubject.Teacher)
                    .Where(t => t.TeacherSubjectId == id)
                    .ToListAsync();

                return data;
            }
            catch (Exception e)
            {
                Logger.Error(e, "eErrore di imprevisto");
                return null;
            }
        }

        public async Task<TeacherSubjectMaterial> CreateTeacherSubjectMaterialAsync(TeacherSubjectMaterial model)
        {
            var data = await AddAsync(model);
            return data;
        }

        public async Task<LessonMaterial> CreateLessonMaterialAsync(LessonMaterial model)
        {
            var data = await AddAsync(model);
            return data;
        }

        public async Task<IEnumerable<LessonMaterial>> GetLessonMaterialsAsync(int id)
        {
            try
            {
                var data = await Context.LessonMaterial
                    .Include(t => t.Material)
                    .Include(t => t.Lesson)
                    .Where(t => t.Lesson.Id == id)
                    .ToListAsync();

                return data;
            }
            catch (Exception e)
            {
                Logger.Error(e, "eErrore di imprevisto");
                return null;
            }
        }

        public async Task<bool> DeleteMaterialAsync(int id)
        {
            try
            {
                var entity = await Context.TeacherSubjectMaterial.FirstOrDefaultAsync(t => t.Id == id);
                Context.TeacherSubjectMaterial.Remove(entity);
                var result = await Context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                return false;
            }
        }

        public async Task<LessonMaterial> GetLessonMaterialByIdAsync(int id)
        {
            try
            {
                var data = await Context.LessonMaterial
                    .Include(t => t.Lesson)
                    .Include(t => t.Material)
                    .Where(t => t.Id == id)
                    .FirstOrDefaultAsync();

                return data;
            }
            catch (Exception e)
            {
                Logger.Error(e, "eErrore di imprevisto");
                return null;
            }
        }

        public async Task<bool> DeleteLessonMaterialAsync(int id)
        {
            try
            {
                var entity = await Context.LessonMaterial.FirstOrDefaultAsync(t => t.Id == id);
                Context.LessonMaterial.Remove(entity);
                var result = await Context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                return false;
            }
        }

        public async Task<TeacherSubjectMaterial> GetTeacherSubjectMaterialByFileNameAsync(int teacherSubjectId, string fileName)
        {
            try
            {
                var data = await Context.TeacherSubjectMaterial
                    .Include(t => t.Material)
                    .Where(t => t.TeacherSubjectId == teacherSubjectId && t.Material.FileName == fileName)
                    .FirstOrDefaultAsync();

                return data;
            }
            catch (Exception e)
            {
                Logger.Error(e, "eErrore di imprevisto");
                return null;
            }
        } 

        public async Task<LessonMaterial> GetLessonMaterialByFileNameAsync(int lessonId, string fileName)
        {
            try
            {
                var data = await Context.LessonMaterial
                    .Include(t => t.Material)
                    .Include(t => t.Lesson)
                    .Where(t => t.LessonId == lessonId && t.Material.FileName == fileName)
                    .FirstOrDefaultAsync();

                return data;
            }
            catch (Exception e)
            {
                Logger.Error(e, "eErrore di imprevisto");
                return null;
            }
        }

        public async Task<Lesson> GetLessonByTeacherSubjectIdAsync(int teacherSubjectId)
        {
            try
            {
                var data = await Context.Lesson
                    .Include(t => t.TeacherSubject)
                    .Where(t => t.TeacherSubjectId == teacherSubjectId)
                    .FirstOrDefaultAsync();

                return data;
            }
            catch (Exception e)
            {
                Logger.Error(e, "eErrore di imprevisto");
                return null;
            }
        }

        public async Task<TeacherSubjectMaterial> GetTeacherSubjectMaterialByMaterialIdAsync(int materialId)
        {
            try
            {
                var data = await Context.TeacherSubjectMaterial
                    .Include(t => t.Material)
                    .Where(t => t.MaterialId == materialId)
                    .FirstOrDefaultAsync();

                return data;
            }
            catch (Exception e)
            {
                Logger.Error(e, "eErrore di imprevisto");
                return null;
            }
        }

        public async Task<IEnumerable<TeacherSubjectMaterial>> GetTeacherSubjectMaterialsByIdsAsync(int[] ids)
        {
            var data = await Context.TeacherSubjectMaterial
                .Include(d => d.Material)
                .Include(d => d.TeacherSubject.Subject)
                .Where(d => ids.Contains(d.Id))
                .ToListAsync();
            return data;
        }
        
        public async Task<IEnumerable<LessonMaterial>> GetLessonMaterialsByIdsAsync(int[] ids)
        {
            var data = await Context.LessonMaterial
                .Include(d => d.Material)
                .Include(d => d.Lesson.TeacherSubject.Subject)
                .Where(d => ids.Contains(d.Id))
                .ToListAsync();
            return data;
        }
    }
}
