using Autofac;
using AutoMapper;
using ItalWebConsulting.Infrastructure.Composition;
using ItalWebConsulting.Infrastructure.Mvc.Composition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;

namespace ItalWebConsulting.Infrastructure.Mvc.Extension
{
    public static class ServiceCollectionExtension
    {
        
        //public static IContainer AddAutoFacForMvc(this IServiceCollection service, ContainerBuilder builder, CompositionRegisterInput input)
        //{
        //    var cm = new ContainerManager(service, builder);
        //    var container = cm.Configure(input);
        //    //return new AutofacServiceProvider(container);
        //    return container;
        //}

        public static void AddAutoFacForMvc(this IServiceCollection service, ContainerBuilder builder, CompositionRegisterInput input)
        {
            var cm = new ContainerManager(builder);
            cm.Configure(input);
            //return new AutofacServiceProvider(container);
        }

        //public static TSettingsConfig GetSection<TSettingsConfig>(this IServiceCollection service,  IConfiguration configuration, string key) where TSettingsConfig : new()
        //{

        //    var confValue = configuration.GetSection(key);
        //    service.Configure<TSettingsConfig>(confValue);
        //    return confValue.Get<TSettingsConfig>();
        //}
    }
}
