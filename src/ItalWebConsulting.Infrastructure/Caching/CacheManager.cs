using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItalWebConsulting.Infrastructure.Caching
{
    public abstract class CacheManager : ICacheManager
    {
        //public abstract object this[string key] { get ; set; }
        public ILogger<CacheManager> Logger { get; set; }
        public abstract void Add(string key, object value, int? expireSeconds);

        public abstract bool Contains(string key);

        public abstract int Count();

        public abstract T Get<T>(string key);

        public abstract IList<string> GetCacheKeys();

        public abstract IList<InfoCache> GetInfoCache();

        public abstract void Insert(string key, object value);

        public abstract void Remove(string key);

        public abstract void RemoveAll();
    }
}
