using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using System.Data.SqlClient;
using Microsoft.Extensions.Logging;
using AutoMapper;

namespace ItalWebConsulting.Infrastructure.DataAccess.EntityFramework
{
    public abstract partial class RepositoryBase<TContext> where TContext: DbContext
    {
       
        public ILogger<RepositoryBase<TContext>> Logger { get; set; }
        public TContext Context { get; set; }
        public IServiceProvider ServiceProvider { get; set; }
        public T GetService<T>()
        {
            return (T)ServiceProvider.GetService(typeof(T));
        }

        public IDbConnection Connection
        {
            get
            {
                var conn = Context.Database.GetDbConnection();

                if (conn.State != ConnectionState.Open)
                    conn.Open();
                return conn;
            }
        }
        public virtual T GetById<T>(int id) where T : class
        {
            var tableName = Context.GetTableName<T>();
            var key = Context.GetKey<T>();

            var sql = SqlQueryBuilderHelper.GetSqlSelectByIdColumn(tableName, key, id);
            //Connection.
            //return Context.Set<T>().Find(id);
            return Connection.QuerySingleOrDefault<T>(sql);
        }

        /*NUOVI METODI*/
        public T GetByColumns<T>(T entity) where T : class
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var tableName = Context.GetTableName<T>();
            var keys = Context.GetKeysValue<T>(entity);

            var sqlOutput = SqlQueryBuilderHelper.GetSqlSelectByColumnns(tableName, keys);

            return Connection.QuerySingleOrDefault<T>(sqlOutput.SqlSelectWithWhere, sqlOutput.Parameters);
        }

        /**/

        public IEnumerable<T> ListAll<T>() where T : class
        {
            var tableName = Context.GetTableName<T>();
            var sql = SqlQueryBuilderHelper.GetSqlSelectAll(tableName);
            //return  Context.Set<T>().ToList();
            return Connection.Query<T>(sql);
        }

        //public IEnumerable<T> List<T>(ISpecification<T> spec) where T : class
        //{
        //    //return Connection.GetList<T>(ApplySpecification(spec).Expression);
        //    return ApplySpecification(spec).ToList();
        //}

        //public int Count<T>(ISpecification<T> spec) where T : class
        //{
        //    return ApplySpecification(spec).Count();
        //}
        //private IQueryable<T> ApplySpecification<T>(ISpecification<T> spec) where T : class
        //{
        //    return SpecificationEvaluator<T>.GetQuery(Context.Set<T>().AsQueryable(), spec);
        //}

        public int Count<T>() where T : class
        {
            var tableName = Context.GetTableName<T>();
            var sql = SqlQueryBuilderHelper.GetSqlSelectAllCount(tableName);
            //return  Context.Set<T>().ToList();
            return Connection.ExecuteScalar<int>(sql);
        }

        public T Add<T>(T entity) where T : class
        {
            Context.Set<T>().Add(entity);
            Context.SaveChanges();

            return entity;
        }

        public void Update<T>(T entity) where T : class
        {
            Context.Entry(entity).State = EntityState.Modified;
            Context.Set<T>().Attach(entity);
            Context.SaveChanges();
        }

        public void Delete<T>(T entity) where T : class
        {
            Context.Set<T>().Remove(entity);
            Context.SaveChanges();
        }

        
    }
}
