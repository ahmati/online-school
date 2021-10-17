using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItalWebConsulting.Infrastructure.DataAccess.EntityFramework
{
    public interface IRepositoryBase
    {
        #region Sync Methods
        T GetById<T>(int id) where T : class;
        T GetByColumns<T>(T entity) where T : class;
        IEnumerable<T> ListAll<T>() where T : class;
        //IEnumerable<T> List<T>(ISpecification<T> spec) where T : class;
        //int Count<T>(ISpecification<T> spec) where T : class;
        int Count<T>() where T : class;
        T Add<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        #endregion

        #region Async Methods
        Task<T> GetByIdAsync<T>(int id) where T : class;
        Task<IEnumerable<T>> GetByIdsAsync<T>(params int[] ids) where T : class;
        Task<T> GetByColumnsAsync<T>(T entity) where T : class;
        Task<IEnumerable<T>> ListAllAsync<T>() where T : class;
        //Task<IReadOnlyList<T>> ListAsync<T>(ISpecification<T> spec) where T : class;
        //Task<int> CountAsync<T>(ISpecification<T> spec) where T : class;
        Task<T> AddAsync<T>(T entity) where T : class;
        Task<T[]> AddAsync<T>(params T[] entities) where T : class;

        Task<int> UpdateAsync<T>(T entity) where T : class;
        Task<int> UpdateAsync<T>(params T[] entities) where T : class;

        Task DeleteAsync<T>(T entity) where T : class;
        //Task DeleteAsync<T>(T[] entities) where T : class;
        Task<int> DeleteByIdAsync<T>(params int[] ids) where T : class;
        #endregion
    }
}
