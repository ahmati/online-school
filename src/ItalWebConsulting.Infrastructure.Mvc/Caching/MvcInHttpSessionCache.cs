using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace ItalWebConsulting.Infrastructure.Mvc.Caching
{
    public class MvcInHttpSessionCache: IMvcInHttpSessionCache
    {
        ISession _cache = null;
        IHttpContextAccessor _httpContextAccessor;
        public MvcInHttpSessionCache(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            //_serviceProvider = serviceProvider;
        }
        //public MvcInHttpSessionCache(ISession cache)
        //{
        //    Cache = cache;
        //}
        ISession Cache
        {
            get
            {
                if (_cache == null)
                    InitilizeMvcInHttpSessionCache();
                return _cache;
            }
        }
        private void InitilizeMvcInHttpSessionCache()
        {
            //var sessionContext = (IHttpContextAccessor)_serviceProvider.GetService(typeof(IHttpContextAccessor));
            if (_httpContextAccessor.HttpContext != null)
                InitializeCache(_httpContextAccessor.HttpContext.Session);
        }
        public bool IsInitializedCache()
        {
            return Cache != null;
        }
        private void InitializeCache(ISession cache)
        {
            _cache = cache;
        }
        public string SessionId
        {
            get => _httpContextAccessor.HttpContext.Session.Id;
        }

        public void Add(string key, object value)
        {
            if (value == null)
            {
                Cache.Set(key, null);
                return;
            }

            if(value.GetType() == typeof(int))
            {
                Cache.SetInt32(key, (int)value);
            }
            else if (value.GetType() == typeof(string))
            {
                Cache.SetString(key, (string)value);
            }
            else
            {
                var serialisedValue = JsonConvert.SerializeObject(value);
                Cache.SetString(key, serialisedValue);
            }
        }

        public bool Contains(string key)
        {
            if(IsInitializedCache())
                foreach (var k in Cache.Keys)
                {
                    if (k.ToString() == key)
                        return true;
                }
            return false;
        }
        public int? Get(string key)
        {
            if (Contains(key))
            {
                return Cache.GetInt32(key);
            }

            return 0;
        }
        public T Get<T>(string key)
        {
            if (Contains(key))
            {
                var value = Cache.GetString(key);
                return JsonConvert.DeserializeObject<T>(value);
            }

            return default(T);
        }

        public void Remove(string key)
        {
            Cache.Remove(key);
        }
    }
}
