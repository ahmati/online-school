using ItalWebConsulting.Infrastructure.BusinessLogic;
using Microsoft.Extensions.Logging;
using OnlineSchool.Contract.Infrastructure;
using OnlineSchool.Contract.Lessons;
using OnlineSchool.Core.Lessons_.Repository;
using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Lessons_.Service
{
    public class LessonService : CoreBase, ILessonService
    {
        public ILessonRepository LessonRepository { get; set; }

        public async Task<LessonModel> GetByIdAsync(int id)
        {
            try
            {
                var data = await LessonRepository.GetByIdAsync(id);
                return Mapper.Map<LessonModel>(data);
            }
            catch (Exception ex)
            {
                Logger.LogError($"An error occurred.", ex.Message);
                throw ex;
            }
        }

        public async Task<ResponseBase<LessonModel>> CreateAsync(LessonModel model)
        {
            var output = new ResponseBase<LessonModel>();
            try
            {
                var exists = (await GetByNameAndTeacherSubjectIdAsync(model.Name, model.TeacherSubjectId)) != null;
                if (exists)
                {
                    output.AddError($"A lesson named '{model.Name}' already exists.");
                    return output;
                }

                var lesson = Mapper.Map<Lesson>(model);

                lesson = await LessonRepository.CreateAsync(lesson);

                output.Output = Mapper.Map<LessonModel>(lesson);
                return output;
            }
            catch (Exception ex)
            {
                Logger.LogError($"An error occurred.", ex.Message);
                output.AddError(ex.Message);
                return output;
            }
        }
        
        public async Task<ResponseBase<LessonModel>> UpdateAsync(LessonModel model)
        {
            var output = new ResponseBase<LessonModel>();
            var errorMessage = "A lesson with this name already exists. Try a different one.";
            try
            {
                var lesson = await LessonRepository.GetByIdAsync(model.Id);

                var lessonNameConflicts = await LessonRepository.GetByNameAndTeacherSubjectIdAsync(model.Name, lesson.TeacherSubjectId);
                if(lessonNameConflicts != null && lessonNameConflicts.Id != model.Id)
                {
                    output.AddError(errorMessage);
                    return output;
                }

                lesson.Name = model.Name;
                lesson.Description = model.Description;
               
                var result = await LessonRepository.UpdateAsync(lesson);
                if (!result)
                {
                    output.AddError(errorMessage);
                    return output;
                }

                output.Output = Mapper.Map<LessonModel>(lesson);
                return output;

            }
            catch (Exception ex)
            {
                Logger.LogError("errore di imprevisto.", ex.Message);
                output.AddError(errorMessage);
                return output;
            }
        }

        public async Task<ResponseBase<bool>> DeleteAsync(int id)
        {
            var output = new ResponseBase<bool>();
            var errorMessage = "An error occurred. Lesson could not be deleted.";
            try
            {
                var deleted = await LessonRepository.DeleteAsync(id);
                if (!deleted)
                {
                    output.AddError(errorMessage);
                    return output;
                }

                output.Output = true;
                return output;
            }
            catch (Exception ex)
            {
                Logger.LogError($"An error occurred.", ex.Message);
                output.AddError(errorMessage);
                return output;
            }
        }

        public async Task<IEnumerable<LessonModel>> GetLessonsByTeacherSubjectId(int teacherSubjectId)
        {
            try
            {
                var data = await LessonRepository.GetLessonsByTeacherSubjectId(teacherSubjectId);

                return Mapper.Map<IEnumerable<LessonModel>>(data);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<LessonModel> GetByNameAsync(string name)
        {
            try
            {
                var data = await LessonRepository.GetByNameAsync(name);
                return Mapper.Map<LessonModel>(data);
            }
            catch (Exception ex)
            {
                Logger.LogError($"An error occurred.", ex.Message);
                throw ex;
            }
        }

        public async Task<LessonModel> GetByNameAndTeacherSubjectIdAsync(string name, int teacherSubjectId)
        {
            try
            {
                var data = await LessonRepository.GetByNameAndTeacherSubjectIdAsync(name, teacherSubjectId);
                return Mapper.Map<LessonModel>(data);
            }
            catch (Exception ex)
            {
                Logger.LogError($"An error occurred.", ex.Message);
                throw ex;
            }
        }
    }
}
