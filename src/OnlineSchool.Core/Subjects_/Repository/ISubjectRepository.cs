using ItalWebConsulting.Infrastructure.DataAccess.EntityFramework;
using OnlineSchool.Contract.Material;
using OnlineSchool.Contract.Subject;
using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Subjects_.Repository
{
    public interface ISubjectRepository : IRepositoryBase
    {
        Task<IEnumerable<Subject>> GetAllAsync();
        Task<Subject> GetByIdAsync(int id);
        Task<Subject> GetByNameAsync(string name);
        Task<Subject> CreateAsync(Subject model);
        Task<bool> UpdateAsync(Subject model);
        Task<bool> DeleteAsync(int id);
        Task<SubjectCategory> GetCategoryByIdAsync(int categoryId);

        // ------------------------------- OLD -------------------------------
    }
}
