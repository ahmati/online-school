using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItalWebConsulting.Infrastructure.Mvc.Caching
{
    public interface IMvcInHttpSessionCache
    {
        string SessionId { get; }
        void Add(string key, object value);
        bool Contains(string key);
        T Get<T>(string key);
        void Remove(string key);
        int? Get(string key);
        //void InitializeCache(ISession cache);
        bool IsInitializedCache();
    }
}
