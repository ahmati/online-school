using ItalWebConsulting.Infrastructure.Caching;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ItalWebConsulting.Infrastructure.DataAccess.EntityFramework
{
    public abstract class EntityCache<TSource> where TSource : class 
                                                        
    {
        readonly ICacheManager _cacheManager;
        readonly DbContextBase _dbContextBase;
        int cacheDuration = (int)(new TimeSpan(1, 0, 0, 0)).TotalSeconds; //Durata di un giorno
        public ILogger<EntityCache<TSource>> Logger { get; set; }
        public IEnumerable<TSource> All
        {
            get
            {
                var key = GetChacheKey();
                if (!_cacheManager.Contains(key))
                    LoadCacheValues();

                return _cacheManager.Get<IEnumerable<TSource>>(key);
            }
        }
        //Expression<Func<TSource, bool>> defaultExpression = x => true;
        protected virtual Expression<Func<TSource, bool>> WherePredicate { get
            {
                
                return null;
            }
        }

        protected EntityCache(DbContextBase dbContextBase, ICacheManager cacheManager)
        {
            this._cacheManager = cacheManager;
            _dbContextBase = dbContextBase;
        }
        private string GetChacheKey()
        {
            var dbName = _dbContextBase.Database.GetDbConnection().Database;
            return string.Concat(this.GetType().Name, ".", dbName, ".", typeof(TSource).Name, ".All");
        }

        private void LoadCacheValues()
        {
            //if (All != null)
            //    return;

            try
            {
                IEnumerable<TSource> all;
                if (WherePredicate != null)
                    all = _dbContextBase.Set<TSource>().Where(WherePredicate).ToList();
                else
                    all = _dbContextBase.Set<TSource>().ToList();

                _cacheManager.Add(GetChacheKey(), all, cacheDuration);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Impossible to load Cache values from DataBase for: " + GetChacheKey());
                throw ex;
            }
        }

        public IEnumerable<TSource> GetValues(Expression<Func<TSource, bool>> filterPredicate)
        {
            var qry = All.AsQueryable();
            if(filterPredicate != null)
                qry = qry.Where(filterPredicate);

            return qry.ToArray();
        }

        public bool ReloadCache()
        {
            try
            {
                var key = GetChacheKey();
                _cacheManager.Remove(key);
                if(_cacheManager.Contains(key))
                {
                    Logger.LogWarning("Impossible to remove entry from cache. Entry key: " + key);
                    return false;
                }
                LoadCacheValues();
                if (All == null || !All.Any())
                {
                    Logger.LogWarning("Impossible to load entry in cache. Entry key: " + key);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Impossible to reload Cache values from DataBase for: " + GetChacheKey());
                return false;
            }

        }

    }
}
