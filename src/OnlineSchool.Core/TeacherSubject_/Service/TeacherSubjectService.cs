using AutoMapper;
using ItalWebConsulting.Infrastructure.BusinessLogic;
using Microsoft.Extensions.Logging;
using NLog;
using OnlineSchool.Contract.Infrastructure;
using OnlineSchool.Contract.Lessons;
using OnlineSchool.Contract.TeacherSubject;
using OnlineSchool.Core.Lessons_.Repository;
using OnlineSchool.Core.TeacherSubject_.Repository;
using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSchool.Core.TeacherSubject_.Service
{
    public class TeacherSubjectService : CoreBase, ITeacherSubjectService
    {
        public ITeacherSubjectRepository TeacherSubjectRepository { get; set; }
        public ILessonRepository LessonRepository { get; set; }

        public async Task<TeacherSubjectModel> GetByIdAsync(int teacherSubjectId)
        {
            try
            {
                var data = await TeacherSubjectRepository.GetByIdAsync(teacherSubjectId);
                return Mapper.Map<TeacherSubjectModel>(data);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "eErrore di imprevisto");
                return null;
            }
        }

        public async Task<TeacherSubjectModel> GetByIdAsync(int teacherId, int subjectId)
        {
            try
            {
                var data = await TeacherSubjectRepository.GetByIdAsync(teacherId, subjectId);
                return Mapper.Map<TeacherSubjectModel>(data);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "eErrore di imprevisto");
                return null;
            }
        }

        public async Task<IEnumerable<TeacherSubjectModel>> GetAllAsync()
        {
            try
            {
                var data = await TeacherSubjectRepository.GetAllAsync();
                return Mapper.Map<IEnumerable<TeacherSubjectModel>>(data);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "eErrore di imprevisto");
                return null;
            }
        }
         public async Task<IEnumerable<TeacherSubjectModel>> GetBySubjectIdAsync(int subjectId)
        {
            try
            {
                var data = await TeacherSubjectRepository.GetBySubjectIdAsync(subjectId);
                return Mapper.Map<IEnumerable<TeacherSubjectModel>>(data);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "eErrore di imprevisto");
                return null;
            }
        }

        public async Task<ResponseBase<TeacherSubjectModel>> CreateAsync(TeacherSubjectModel model)
        {
            var output = new ResponseBase<TeacherSubjectModel>();
            
            try
            {
                var exists = (await TeacherSubjectRepository.GetByIdAsync(model.TeacherId, model.SubjectId)) != null;
                if (exists)
                {
                    output.AddError($"This teacher is already assigned to this subject");
                    return output;
                }

                var teacherSubject = Mapper.Map<TeacherSubject>(model);
                teacherSubject = await TeacherSubjectRepository.CreateAsync(teacherSubject);
                output.Output = Mapper.Map<TeacherSubjectModel>(teacherSubject);

                return output;

            }
            catch (Exception ex)
            {
                Logger.LogError("errore di imprevisto.", ex.Message);
                output.AddError(ex.Message);
                return output;
            }
        }

        public async Task<ResponseBase<bool>> DeleteAsync(int subjectId, int teacherId)
        {
            var output = new ResponseBase<bool>();
            try
            {
                var deleted = await TeacherSubjectRepository.DeleteAsync( subjectId, teacherId);
                if (!deleted)
                {
                    output.AddError("An error occurred. Teacher social could not be deleted.");
                    return output;
                }

                output.Output = true;
                return output;
            }
            catch (Exception ex)
            {
                Logger.LogError($"An error occurred.", ex.Message);
                output.AddError("");
                return output;
            }
        }

        #region ----------------------------------------- Material Actions -----------------------------------------

        public async Task<IEnumerable<TeacherSubjectMaterialModel>> GetMaterialsAsync(int subjectId)
        {
            try
            {
                var data = await TeacherSubjectRepository.GetMaterialsAsync(subjectId);
                return Mapper.Map<IEnumerable<TeacherSubjectMaterialModel>>(data);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "eErrore di imprevisto");
                return null;
            }
        }

        public async Task<ResponseBase<TeacherSubjectMaterialModel>> CreateTeacherSubjectMaterialAsync(TeacherSubjectMaterialModel model)
        {
            var output = new ResponseBase<TeacherSubjectMaterialModel>();

            try
            {
                var teacherSubject = Mapper.Map<TeacherSubjectMaterial>(model);
                teacherSubject = await TeacherSubjectRepository.CreateTeacherSubjectMaterialAsync(teacherSubject);
                output.Output = Mapper.Map<TeacherSubjectMaterialModel>(teacherSubject);

                return output;

            }
            catch (Exception ex)
            {
                Logger.LogError("errore di imprevisto.", ex.Message);
                output.AddError(ex.Message);
                return output;
            }
        }

        public async Task<IEnumerable<LessonMaterialModel>> GetLessonMaterialsAsync(int subjectId)
        {
            try
            {
                var data = await TeacherSubjectRepository.GetLessonMaterialsAsync(subjectId);
                return Mapper.Map<IEnumerable<LessonMaterialModel>>(data);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "eErrore di imprevisto");
                return null;
            }
        }

        public async Task<IEnumerable<LessonModel>> GetLessonsByTeacherSubjectAsync(int teacherSubjectId)
        {
            try
            {
                var data = await LessonRepository.GetLessonsByTeacherSubjectId(teacherSubjectId);
                return Mapper.Map<IEnumerable<LessonModel>>(data);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "eErrore di imprevisto");
                return null;
            }
        }

        public async Task<ResponseBase<bool>> DeleteMaterialAsync(int id)
        {
            var output = new ResponseBase<bool>();
            try
            {
                var deleted = await TeacherSubjectRepository.DeleteMaterialAsync(id);
                if (!deleted)
                {
                    output.AddError("An error occurred. Material could not be deleted.");
                    return output;
                }

                output.Output = true;
                return output;
            }
            catch (Exception ex)
            {
                Logger.LogError($"An error occurred.", ex.Message);
                output.AddError(ex.Message);
                return output;
            }
        }

        public async Task<ResponseBase<LessonMaterialModel>> CreateLessonMaterialAsync(LessonMaterialModel model)
        {
            var output = new ResponseBase<LessonMaterialModel>();

            try
            {
                var data = Mapper.Map<LessonMaterial>(model);
                data = await TeacherSubjectRepository.CreateLessonMaterialAsync(data);
                output.Output = Mapper.Map<LessonMaterialModel>(data);

                return output;

            }
            catch (Exception ex)
            {
                Logger.LogError("errore di imprevisto.", ex.Message);
                output.AddError(ex.Message);
                return output;
            }
        }

        public async Task<ResponseBase<bool>> DeleteLessonMaterialAsync(int id)
        {
            var output = new ResponseBase<bool>();
            try
            {
                var deleted = await TeacherSubjectRepository.DeleteLessonMaterialAsync(id);
                if (!deleted)
                {
                    output.AddError("An error occurred. This lesson's material could not be deleted.");
                    return output;
                }

                output.Output = true;
                return output;
            }
            catch (Exception ex)
            {
                Logger.LogError($"An error occurred.", ex.Message);
                output.AddError(ex.Message);
                return output;
            }
        }

        public async Task<ResponseBase<TeacherSubjectMaterialModel>> GetTeacherSubjectMaterialByFileNameAsync(int teacherSubjectId, string fileName)
        {
            var output = new ResponseBase<TeacherSubjectMaterialModel>();

            try
            {
                var data = await TeacherSubjectRepository.GetTeacherSubjectMaterialByFileNameAsync(teacherSubjectId, fileName);
                var result =     Mapper.Map<TeacherSubjectMaterialModel>(data);
                output.Output = result;

            }
            catch (Exception ex)
            {
                Logger.LogError("Error encountered.", ex.Message);
                output.AddError("An error occurred while retrieving materials.");
            }
            return output;
        }

        public async Task<ResponseBase<LessonMaterialModel>> GetLessonMaterialByFileNameAsync(int lessonId, string fileName)
        {
            var output = new ResponseBase<LessonMaterialModel>();

            try
            {
                var data = await TeacherSubjectRepository.GetLessonMaterialByFileNameAsync(lessonId, fileName);
                var result =     Mapper.Map<LessonMaterialModel>(data);
                output.Output = result;

            }
            catch (Exception ex)
            {
                Logger.LogError("Error encountered.", ex.Message);
                output.AddError("An error occurred while retrieving materials.");
            }
            return output;
        }

        public async Task<ResponseBase<TeacherSubjectMaterialModel>> GetTeacherSubjectMaterialByIdAsync(int id)
        {
            var output = new ResponseBase<TeacherSubjectMaterialModel>();

            try
            {
                var data = await TeacherSubjectRepository.GetTeacherSubjectMaterialByIdAsync(id);
                var result = Mapper.Map<TeacherSubjectMaterialModel>(data);
                output.Output = result;
            }
            catch (Exception ex)
            {
                Logger.LogError("Error encountered.", ex.Message);
                output.AddError("An error occurred while retrieving material.");
            }
            return output;
        }

        #endregion

        public async Task<ResponseBase<LessonModel>> GetLessonByTeacherSubjectIdAsync(int teacherSubjectId)
        {
            var output = new ResponseBase<LessonModel>();

            try
            {
                var data = await TeacherSubjectRepository.GetLessonByTeacherSubjectIdAsync(teacherSubjectId);
                var result = Mapper.Map<LessonModel>(data);
                output.Output = result;
            }
            catch (Exception ex)
            {
                Logger.LogError("Error encountered.", ex.Message);
                output.AddError("An error occurred while retrieving lesson.");
            }
            return output;
        }

        public async Task<ResponseBase<LessonMaterialModel>> GetLessonMaterialByIdAsync(int id)
        {
            var output = new ResponseBase<LessonMaterialModel>();

            try
            {
                var data = await TeacherSubjectRepository.GetLessonMaterialByIdAsync(id);
                var result = Mapper.Map<LessonMaterialModel>(data);
                output.Output = result;
            }
            catch (Exception ex)
            {
                Logger.LogError("Error encountered.", ex.Message);
                output.AddError("An error occurred while retrieving material.");
            }
            return output;
        }

        public async Task<ResponseBase<TeacherSubjectMaterialModel>> GetTeacherSubjectMaterialByMaterialIdAsync(int materialId)
        {
            var output = new ResponseBase<TeacherSubjectMaterialModel>();

            try
            {
                var data = await TeacherSubjectRepository.GetTeacherSubjectMaterialByMaterialIdAsync(materialId);
                var result = Mapper.Map<TeacherSubjectMaterialModel>(data);
                output.Output = result;

            }
            catch (Exception ex)
            {
                Logger.LogError("Error encountered.", ex.Message);
                output.AddError("An error occurred while retrieving materials.");
            }
            return output;
        }

        public async Task<ResponseBase<IEnumerable<TeacherSubjectMaterialModel>>> GetTeacherSubjectMaterialsByIdsAsync(int[] ids)
        {
            var output = new ResponseBase<IEnumerable<TeacherSubjectMaterialModel>>();
            if (ids is null)
                throw new ArgumentNullException("TeacherSubject material ids required");
            if (ids.Length == 0)
                return output;
            try
            {
                var data = await TeacherSubjectRepository.GetTeacherSubjectMaterialsByIdsAsync(ids);
                var result = Mapper.Map<IEnumerable<TeacherSubjectMaterialModel>>(data);
                output.Output = result;
            }
            catch (Exception ex)
            {
                Logger.LogError("Error encountered.", ex.Message);
                output.AddError("An error occurred while retrieving materials.");
            }
            return output;
        }
        
        public async Task<ResponseBase<IEnumerable<LessonMaterialModel>>> GetLessonMaterialsByIdsAsync(int[] ids)
        {
            var output = new ResponseBase<IEnumerable<LessonMaterialModel>>();
            if (ids is null)
                throw new ArgumentNullException("Lesson Material ids required");
            if (ids.Length == 0)
                return output;
            try
            {
                var data = await TeacherSubjectRepository.GetLessonMaterialsByIdsAsync(ids);
                var result = Mapper.Map<IEnumerable<LessonMaterialModel>>(data);
                output.Output = result;
            }
            catch (Exception ex)
            {
                Logger.LogError("Error encountered.", ex.Message);
                output.AddError("An error occurred while retrieving materials.");
            }
            return output;
        }

    }
}
