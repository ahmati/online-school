using OnlineSchool.Contract.Infrastructure;
using OnlineSchool.Contract.Material;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Materials_.Services
{
    public interface IMaterialService
    {
        Task<ResponseBase<MaterialModel>> CreateAsync(MaterialModel model);
        Task<ResponseBase<bool>> DeleteAsync(int id);
        Task<ResponseBase<MaterialModel>> GetCourseMaterialByIdAsync(int materialId);
    }
}