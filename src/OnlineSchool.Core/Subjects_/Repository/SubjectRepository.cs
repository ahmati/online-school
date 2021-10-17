using Dapper;
using Microsoft.EntityFrameworkCore;
using OnlineSchool.Contract.Material;
using OnlineSchool.Contract.Subject;
using OnlineSchool.Domain.OnlineSchoolDB;
using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ef1 = OnlineSchool.Domain.OnlineSchoolDB.EF;

namespace OnlineSchool.Core.Subjects_.Repository
{
    public class SubjectRepository : OnlineSchoolBaseRepository, ISubjectRepository
    {
        public async Task<IEnumerable<Subject>> GetAllAsync()
        {
            try
            {
                return await ListAllAsync<Subject>();
            }
            catch (Exception ex)
            {

                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                throw ex;
            }
        }

        public async Task<Subject> GetByIdAsync(int id)
        {
            var data = await Context.Subject.FirstOrDefaultAsync(s => s.Id == id);
            return data;
        }

        public async Task<Subject> GetByNameAsync(string name)
        {
            var data = await Context.Subject.FirstOrDefaultAsync(s => s.Name == name);
            return data;
        }

        public async Task<Subject> CreateAsync(Subject model)
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

        public async Task<bool> UpdateAsync(Subject model)
        {
            var result = await base.UpdateAsync(model);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var result = await DeleteByIdAsync<Subject>(id);
            return result > 0;
        }

        public async Task<SubjectCategory> GetCategoryByIdAsync(int categoryId)
        {
            var data = await Context.SubjectCategory.FirstOrDefaultAsync(sC => sC.Id == categoryId);
            return data;
        }

        // ---------------------------------- OLD ----------------------------------
    }
}


