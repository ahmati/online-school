using AutoMapper;
using ItalWebConsulting.Infrastructure.Caching;
using ItalWebConsulting.Infrastructure.Composition;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItalWebConsulting.Infrastructure.BusinessLogic
{
    public abstract class CoreBase
    {
        public ILogger<CoreBase> Logger { get; set; }
        public IServiceProvider ServiceProvider { get; set; }
        public IMapper Mapper { get; set; }
        public Lazy<ICacheManager> CacheManager { get; set; }
        public IConfigurationManager ConfigurationManager { get; set; }
        public T GetService<T>()
        {
            return (T)ServiceProvider.GetService(typeof(T));
        }
    }
}
