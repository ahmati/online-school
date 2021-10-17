using Autofac;
using AutoMapper;
using ItalWebConsulting.Infrastructure.Composition;
using ItalWebConsulting.Infrastructure.Comunication;
using ItalWebConsulting.Infrastructure.Logging;
//using InfoWeb.Infrastructure.Mvc.Composition;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Linq;
using System.Reflection;

namespace ItalWebConsulting.Infrastructure.Extension
{
    public static class ServiceCollectionExtension
    {
        public static void AddNLog(this IServiceCollection service)
        {
            LogConfigurator.Configure();
        }

        public static IContainer AddAutoFacForBatch(this IServiceCollection service, CompositionRegisterInput input)
        {
            //Mose change the logic with net core 3.0 everything is changed
            var cm = new ContainerManagerBatch(service);
            cm.Configure(input);
            //var container = cm.Configure(input);
            //return new AutofacServiceProvider(container);
            return null;
           
        }


        public static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(DependencyContext.Default);
        }

        public static void AddAutoMapper(this IServiceCollection services, DependencyContext dependencyContext)
        {
            services.AddAutoMapper(dependencyContext.RuntimeLibraries
                .SelectMany(lib => lib.GetDefaultAssemblyNames(dependencyContext).Select(Assembly.Load)));
        }
    }
}
