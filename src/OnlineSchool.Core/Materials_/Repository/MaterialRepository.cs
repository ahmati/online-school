using Dapper;
using Microsoft.EntityFrameworkCore;
using OnlineSchool.Contract.Material;
using OnlineSchool.Domain.OnlineSchoolDB;
using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System;
using System.Text;
using System.Threading.Tasks;
using Ef = OnlineSchool.Domain.OnlineSchoolDB.EF;

namespace OnlineSchool.Core.Materials_.Repository
{
    class MaterialRepository : OnlineSchoolBaseRepository, IMaterialRepository
    {
        public async Task<Material> CreateAsync(Material material)
        {
            var data = await AddAsync(material);
            return data;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var material = await Context.Material.FirstOrDefaultAsync(t => t.Id == id);

                Context.Entry(material).State = EntityState.Deleted;
                var result = await Context.SaveChangesAsync();

                return result > 0;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                return false;
            }
        }

        public async Task<Material> GetCourseMaterialByIdAsync(int id)
        {
            var data = await Context.Material.FirstOrDefaultAsync(d => d.Id == id);
            return data;
        }
    }
}
