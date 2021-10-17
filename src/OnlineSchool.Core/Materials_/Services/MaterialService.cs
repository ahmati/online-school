using ItalWebConsulting.Infrastructure.BusinessLogic;
using Microsoft.Extensions.Logging;
using OnlineSchool.Contract.Infrastructure;
using OnlineSchool.Contract.Material;
using OnlineSchool.Core.Materials_.Repository;
using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Materials_.Services
{
    class MaterialService : CoreBase, IMaterialService
    {
        public IMaterialRepository MaterialRepository { get; set; }

        public async Task<ResponseBase<MaterialModel>> CreateAsync(MaterialModel model)
        {
            var output = new ResponseBase<MaterialModel>();
            try
            {
                var data = Mapper.Map<Material>(model);
                var result = await MaterialRepository.CreateAsync(data);
                output.Output = Mapper.Map<MaterialModel>(result);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error encountered.", ex.Message);
                output.AddError("An error occurred while storing this material.");
            }
            return output;
        }

        public async Task<ResponseBase<bool>> DeleteAsync(int id)
        {
            var output = new ResponseBase<bool>();
            try
            {
                var deleted = await MaterialRepository.DeleteAsync(id);
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

        public async Task<ResponseBase<MaterialModel>> GetCourseMaterialByIdAsync(int id)
        {
            var output = new ResponseBase<MaterialModel>();
            try
            {
                var data = await MaterialRepository.GetCourseMaterialByIdAsync(id);
                var result = Mapper.Map<MaterialModel>(data);
                output.Output = result;
            }
            catch (Exception ex)
            {
                Logger.LogError("Error encountered.", ex.Message);
                output.AddError("An error occurred while retrieving teacher document.");
            }
            return output;
        }
    }
}
