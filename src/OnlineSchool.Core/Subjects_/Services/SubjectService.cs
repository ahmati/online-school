using ItalWebConsulting.Infrastructure.BusinessLogic;
using Microsoft.Extensions.Logging;
using OnlineSchool.Contract.Infrastructure;
using OnlineSchool.Contract.Infrastructure.Enums;
using OnlineSchool.Contract.Material;
using OnlineSchool.Contract.Subject;
using OnlineSchool.Contract.SubjectCategory;
using OnlineSchool.Core.Subjects_.Repository;
using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ef1 = OnlineSchool.Domain.OnlineSchoolDB.EF;

namespace OnlineSchool.Core.Subjects_.Services
{
    public class SubjectService : CoreBase, ISubjectService
    {
        public ISubjectRepository SubjectRepository { get; set; }

        public async Task<IEnumerable<SubjectModel>> GetAllAsync()
        {
            try
            {
                var data = await SubjectRepository.GetAllAsync();
                return Mapper.Map<IEnumerable<SubjectModel>>(data);

            }
            catch (Exception ex)
            {
                Logger.LogError("errore di imprevisto.", ex.Message);
                throw ex;
            }
        }

        public async Task<SubjectModel> GetByIdAsync(int id)
        {
            try
            {
                var data = await SubjectRepository.GetByIdAsync(id);
                return Mapper.Map<SubjectModel>(data);
            }
            catch (Exception ex)
            {
                Logger.LogError($"An error occurred.", ex.Message);
                throw ex;
            }
        }

        public async Task<SubjectModel> GetByNameAsync(string name)
        {
            try
            {
                var data = await SubjectRepository.GetByNameAsync(name);
                return Mapper.Map<SubjectModel>(data);
            }
            catch (Exception ex)
            {
                Logger.LogError($"An error occurred.", ex.Message);
                throw ex;
            }
        }

        public async Task<ResponseBase<SubjectModel>> CreateAsync(CreateSubjectModel model)
        {
            var output = new ResponseBase<SubjectModel>();
            try
            {
                var exists = (await GetByNameAsync(model.Name)) != null;
                if(exists)
                {
                    output.AddError($"A subject named '{model.Name}' already exists.");
                    return output;
                }

                var subject = Mapper.Map<Subject>(model);
                subject.CategoryId = (byte)SubjectCategories.Subject;

                subject = await SubjectRepository.CreateAsync(subject);

                output.Output = Mapper.Map<SubjectModel>(subject);
                return output;
            }
            catch (Exception ex)
            {
                Logger.LogError($"An error occurred.", ex.Message);
                output.AddError(ex.Message);
                return output;
            }
        }

        public async Task<ResponseBase<bool>> UpdateAsync(UpdateSubjectModel model)
        {
            var output = new ResponseBase<bool>();
            try
            {
                var subject = await SubjectRepository.GetByIdAsync(model.Id);
                if(subject is null)
                {
                    output.AddError("An error occurred. The subject was not updated.");
                    return output;
                }
                    
                Mapper.Map(model, subject);
                var updated = await SubjectRepository.UpdateAsync(subject);
                if(!updated)
                {
                    output.AddError("An error occurred. The subject was not updated.");
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

        public async Task<ResponseBase<bool>> DeleteAsync(int id)
        {
            var output = new ResponseBase<bool>();
            try
            {
                var deleted = await SubjectRepository.DeleteAsync(id);
                if (!deleted)
                {
                    output.AddError("An error occurred. Subject could not be deleted.");
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

        public async Task<SubjectCategoryModel> GetCategoryByIdAsync(int categoryId)
        {
            try
            {
                var data = await SubjectRepository.GetCategoryByIdAsync(categoryId);
                return Mapper.Map<SubjectCategoryModel>(data);
            }
            catch (Exception ex)
            {
                Logger.LogError($"An error occurred.", ex.Message);
                throw ex;
            }
        }

        // --------------------------------------- OLD ---------------------------------------
    }
}
