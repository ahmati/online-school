using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data.SqlClient;
using System.Threading.Tasks;
using AutoMapper;

namespace ItalWebConsulting.Infrastructure.DataAccess.EntityFramework
{
    //Dapper.Extensions.NetCore
    public partial class RepositoryBase<TContext>
    {
        public IMapper Mapper { get; set; }
        public virtual async Task<T> GetByIdAsync<T>(int id) where T : class
        {
            var tableName = Context.GetTableName<T>();
            var key = Context.GetKey<T>();

            var sql = SqlQueryBuilderHelper.GetSqlSelectByIdColumn(tableName, key, id);

            //return await Context.Set<T>().FindAsync(id);
            return await Connection.QuerySingleOrDefaultAsync<T>(sql);
        }
        public async Task<IEnumerable<T>> GetByIdsAsync<T>(params int[] ids) where T : class
        {
            var tableName = Context.GetTableName<T>();
            var key = Context.GetKey<T>();
            var sql = SqlQueryBuilderHelper.GetSqlSelectByIdColumnInList(tableName, key, ids);

            return await Connection.QueryAsync<T>(sql);
        }

        public async Task<IEnumerable<T>> ListAllAsync<T>() where T : class
        {
            //var tableName = Context.GetTableName<T>();
            //var sql = SqlQueryBuilderHelper.GetSqlSelectAll(tableName);
            return await Context.Set<T>().ToListAsync();
            //return await Connection.QueryAsync<T>(sql);
        }
        public async Task<T> GetByColumnsAsync<T>(T entity) where T : class
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var tableName = Context.GetTableName<T>();
            var keys = Context.GetKeysValue<T>(entity);

            var sqlOutput = SqlQueryBuilderHelper.GetSqlSelectByColumnns(tableName, keys);
            
            return await Connection.QuerySingleOrDefaultAsync<T>(sqlOutput.SqlSelectWithWhere, sqlOutput.Parameters);
        }

        //public async Task<IReadOnlyList<T>> ListAsync<T>(ISpecification<T> spec) where T : class
        //{
        //    return await ApplySpecification(spec).ToListAsync();
        //}

        //public async Task<int> CountAsync<T>(ISpecification<T> spec) where T : class
        //{
        //    return await ApplySpecification(spec).CountAsync();
        //}
        public async Task<T> AddAsync<T>(T entity) where T : class
        {
            Context.Entry(entity).State = EntityState.Added;
            //var result = await (AddAsync(new[] { entity }));
            //var x = result.FirstOrDefault();
            //return x;
            await Context.Set<T>().AddAsync(entity);
            await Context.SaveChangesAsync();
            Context.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public async Task<T[]> AddAsync<T>(params T[] entities) where T : class
        {
            await Context.Set<T>().AddRangeAsync(entities);
            await Context.SaveChangesAsync();

            return entities;
        }

        public async Task<int> UpdateAsync<T>(T entity) where T : class
        {
            Context.Entry(entity).State = EntityState.Modified;
           // Context.Attach(entity);
            Context.Set<T>().Update(entity);
            return await Context.SaveChangesAsync();
        }
        public async Task<int> UpdateAsync<T>(params T[] entities) where T : class
        {
            entities.ToList().ForEach(entity => 
                Context.Entry(entity).State = EntityState.Modified
            );

            return await Context.SaveChangesAsync();
        }
        public async Task DeleteAsync<T>(T entity) where T : class
        {
            Context.Entry(entity).State = EntityState.Deleted;
            Context.Remove(entity);
           
            await Context.SaveChangesAsync();
        }

        //public async Task DeleteAsync<T>(T[] entities) where T : class
        //{
        //    //entities.ToList().ForEach(entity =>
        //    //    Context.Entry(entity).State = EntityState.Deleted
        //    //);
        //    Context.RemoveRange(entities);
           

        //    await Context.SaveChangesAsync();
        //}

        public async Task<int> DeleteByIdAsync<T>(params int[] ids) where T : class
        {
            var tableName = Context.GetTableName<T>();
            var key = Context.GetKey<T>();
            var sqlDelete = SqlQueryBuilderHelper.GetSqlDeleteFromWhere(tableName, key, ids);

            //entities.ToList().ForEach(entity =>
            //    Context.Entry(entity).State = EntityState.Deleted
            //);
            
            return await Connection.ExecuteAsync(sqlDelete);
        }

       
    }
}
