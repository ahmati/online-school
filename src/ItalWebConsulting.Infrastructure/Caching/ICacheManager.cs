using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ItalWebConsulting.Infrastructure.Caching
{
    public interface ICacheManager
    {
        void Add(string key, object value, int? expireSeconds);
        bool Contains(string key);
        int Count();
        void Insert(string key, object value);
        T Get<T>(string key);
        IList<string> GetCacheKeys();
        IList<InfoCache> GetInfoCache();
        
        void Remove(string key);
        void RemoveAll();
        //object this[string key]
        //{
        //    get;
        //    set;
        //}
    }
}
