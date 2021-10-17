using ItalWebConsulting.Infrastructure.Caching;
using ItalWebConsulting.Infrastructure.Logging;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Web;

namespace ItalWebConsulting.Infrastructure.Mvc.Caching
{
    public class MvcInMemoryCache : IwMemoryCache
    {
        public MvcInMemoryCache(IMemoryCache cache)
            :base(cache)
        { }
    }
}
