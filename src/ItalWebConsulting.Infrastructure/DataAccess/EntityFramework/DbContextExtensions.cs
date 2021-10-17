using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace ItalWebConsulting.Infrastructure.DataAccess.EntityFramework
{
    public static class DbContextExtensions
    {
        public static string GetTableName<T>(this DbContext context) where T : class
        {

            var entityType = context.Model.FindEntityType(typeof(T));
            var schema = entityType.GetSchema();
            return entityType.GetTableName();
        }
        public static async Task<int> Delete<T>(this DbContext context, Expression<Func<T, bool>> filter) where T : class
        {
            var dbSet = context.Set<T>();
            if (filter != null)
                dbSet.RemoveRange(dbSet.Where(filter));
            else
                dbSet.RemoveRange(dbSet);

            return await context.SaveChangesAsync();
        }

        public static string GetKey<T>(this DbContext context) where T : class
        {
            return context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties
                .Select(x => x.Name).Single();

            //return (int)entity.GetType().GetProperty(keyName).GetValue(entity, null);
        }

        public static IDictionary<string,object> GetKeysValue<T>(this DbContext context, T entity) where T : class
        {
            var allKeys = context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.Select(x => x.Name).ToList();
            var keys = new Dictionary<string, object>();
            foreach (var keyName in allKeys)
            {
                var val = entity.GetType().GetProperty(keyName).GetValue(entity, null);
                keys.Add(keyName, val);
            }

            return keys;
        }

        public static void DetachEntities(this DbContext context, object[] entities) 
        {
            foreach (var en in entities)
                context.Entry(en).State = EntityState.Detached;
        }

        public async static Task<int> ExecuteNonQueryAsync(this DbContext context, string nonQuery)
        {
            return await context.Database.ExecuteSqlRawAsync(nonQuery);
            //var conn = context.Connection;
            //var cmd = conn.CreateCommand();
            //cmd.CommandText = nonQuery;
            //return cmd.ExecuteNonQuery();
        }

        public async static Task<int> ExecuteUpdateAsync<T>(this DbContext context, IDictionary<string, object> setColValue,params int[] idKey) where T : class
        {
            var tblName = context.GetTableName<T>();
            var key = context.GetKey<T>();
            var whereIn = SqlQueryBuilderHelper.GetSqlWhereByIdColumnInList(key, idKey);
            //var lsParams = new List<SqlParameter>();
            var lsSet = new StringBuilder();
            lsSet.Append(" SET ");
            var sep = "";
            foreach (var p in setColValue)
            {
                lsSet.Append(sep);
                //lsParams.Add(new SqlParameter
                //{
                //    ParameterName = "@"+p.Key,
                //    Value = p.Value
                //});
                lsSet.AppendFormat(p.Key);
                lsSet.AppendFormat("=?");

                sep = ",";
            }
            var update = string.Concat("UPDATE ", tblName, lsSet.ToString(), " WHERE ", whereIn);
            return await context.Database.ExecuteSqlRawAsync(update, setColValue.Values);
        }

        //public static string GetTableName<T>(this ObjectContext context) where T : class
        //{
        //    string sql = context.CreateObjectSet<T>().ToTraceString();
        //    var regex = new Regex("FROM (?<table>.*) AS");
        //    var match = regex.Match(sql);

        //    string table = match.Groups["table"].Value;
        //    return table;
        //}
    }
}
