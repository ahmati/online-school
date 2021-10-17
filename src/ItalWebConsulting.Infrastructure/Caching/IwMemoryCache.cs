using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ItalWebConsulting.Infrastructure.Caching
{
    public class IwMemoryCache : CacheManager
    {
        private IMemoryCache _cache;
        public IwMemoryCache(IMemoryCache cache)
        {
            this._cache = cache;
        }

        #region ICacheManager Members
        /// <summary>
        /// Insert a new key-value in chache
        /// </summary>
        /// <param name="key">The unique key name formatted following the rule: [caller].[KeyName]</param>
        /// <param name="value">The value you want to cache</param>
        /// /// <param name="expireSeconds">Optional, the duration in seconds of the value in cache</param>
        public override void Add(string key, object value, int? expireSeconds)
        {
            CheckKeyNamingConvection(key);
            CheckExistWithDifferentType(key, value);

            if (!expireSeconds.HasValue)
                _cache.Set(key, GetCacheContainer(value));
            else
                _cache.Set(key, GetCacheContainer(value), TimeSpan.FromSeconds(expireSeconds.Value));
        }

        public override bool Contains(string key)
        {
            CacheContainer cc;
            return _cache.TryGetValue(key, out cc);
        }

        public override int Count()
        {
            return GetAllItemsInCache().Count();
        }
        /// <summary>
        /// Insert a new key-value in chache
        /// </summary>
        /// <param name="key">The unique key name formatted following the rule: [caller].[KeyName]</param>
        /// <param name="value">The value you want to cache</param>
        public override void Insert(string key, object value)
        {
            CheckKeyNamingConvection(key);
            CheckExistWithDifferentType(key, value);

            _cache.Set(key, GetCacheContainer(value));
        }

        public override T Get<T>(string key)
        {
            CacheContainer cc;
            if (_cache.TryGetValue(key, out cc))
                return (T)cc.Value;

            return default(T);
        }

        public override IList<string> GetCacheKeys()
        {
            return GetAllItemsInCache().Keys.ToList();
        }
        public override IList<InfoCache> GetInfoCache()
        {
            var ls = new List<InfoCache>();
            var allItamesInCache = GetAllItemsInCache();
            foreach (var item in allItamesInCache)
            {
                var key = item.Key;
                var obj = item.Value;
                var cc = obj as CacheContainer;
                if (cc != null)
                {
                    ls.Add(new InfoCache
                    {
                        CachedDate = cc.CachedDate,
                        Key = key,
                        ValueType = cc.ValueType
                    });
                }
                else if (obj != null)
                {
                    ls.Add(new InfoCache
                    {
                        CachedDate = null,
                        Key = key,
                        ValueType = obj.GetType()
                    });
                }

            }
            return ls;
        }

        public override void Remove(string key)
        {
            _cache.Remove(key);
        }

        public override void RemoveAll()
        {
            var keys = GetCacheKeys();
            foreach (string key in keys)
                Remove(key);
        }

        //public override object this[string key]
        //{
        //    get
        //    {
        //        return Cache[key];
        //    }
        //    set
        //    {
        //        Cache[key] = value;
        //    }
        //}


        #endregion
        //private IMemoryCache Cache
        //{
        //    get
        //    {
        //        return cache;
        //    }
        //}
        private void CheckExistWithDifferentType(string key, object value)
        {
            if (value != null && Contains(key))
            {
                CacheContainer cc;
                if (_cache.TryGetValue(key, out cc))
                {
                    if (cc.ValueType != value.GetType())
                        throw new ArgumentException("Impossibile inserire l'oggetto con chiave: " + key + " di tipo " + value.GetType().Name + " poiché in cache esiste già un oggetto con la stessa chiave ma con tipo differente: " + cc.GetType().Name);
                }
            }
        }

        private void CheckKeyNamingConvection(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            var lenKey = key.Length;
            var minKeyLength = 12;
            var error = "The name of the key to avoid ambiguity with other keys must be at least " + minKeyLength + " characters long and must contain the [name of the caller] +. + [key name]. Example a name key Id registered by the LoginController controller the key must be of the type: LoginController.Id";
            if (lenKey < minKeyLength)
                throw new ArgumentException(error);
            if (!key.Contains("."))
                throw new ArgumentException(error);
        }

        private CacheContainer GetCacheContainer(object value)
        {
            return new CacheContainer
            {
                CachedDate = DateTime.Now,
                Value = value,
            };
        }

        private IDictionary<string, object> GetAllItemsInCache()
        {
            var field = typeof(MemoryCache).GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);
            var collection = field.GetValue(_cache) as System.Collections.ICollection;

            var items = new Dictionary<string, object>();
            if (collection != null)
                foreach (var item in collection)
                {
                    var methodInfo = item.GetType().GetProperty("Key");
                    var val = methodInfo.GetValue(item);
                    items.Add(methodInfo.Name, val);
                }

            return items;
        }

    }
}
