using OnlineSchool.Contract.Material;
using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Materials_.Repository
{
    public interface IMaterialRepository
    {
        Task<Material> CreateAsync(Material material);
        Task<bool> DeleteAsync(int id);
        Task<Material> GetCourseMaterialByIdAsync(int id);
    }
}