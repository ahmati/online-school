using OnlineSchool.Contract.Infrastructure;
using OnlineSchool.Contract.Lessons;
using OnlineSchool.Contract.Material;
using OnlineSchool.Contract.Subject;
using OnlineSchool.Contract.SubjectCategory;
using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSchool.Contract.Subject
{
    public interface ISubjectService
    {
        Task<IEnumerable<SubjectModel>> GetAllAsync();
        Task<SubjectModel> GetByIdAsync(int id);
        Task<SubjectModel> GetByNameAsync(string name);
        Task<ResponseBase<SubjectModel>> CreateAsync(CreateSubjectModel model);
        Task<ResponseBase<bool>> UpdateAsync(UpdateSubjectModel model);
        Task<ResponseBase<bool>> DeleteAsync(int id);

        // ------------------------------- OLD -------------------------------

        Task<SubjectCategoryModel> GetCategoryByIdAsync(int categoryId);
    }
}
