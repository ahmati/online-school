using Autofac;
using Autofac.Extensions.DependencyInjection;
using ItalWebConsulting.Infrastructure.Composition;
using ItalWebConsulting.Infrastructure.Mvc.Caching;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace ItalWebConsulting.Infrastructure.Mvc.Composition
{
    public class ContainerManager: ContainerManagerBase
    {
        public ContainerManager(ContainerBuilder builder)
        :base(builder)
        {
            
        }
        //public static TService GetInstance<TService>() => Resolver.GetService<TService>();
        protected override void ConfigureInternal(CompositionRegisterInput input)
        {
            if (input.AssemblyMvcType == null)
                throw new ArgumentNullException(nameof(input.AssemblyMvcType), "Specificare un type contenuto nell'assembly MVC");

            ConfigureForMvc(input.AssemblyMvcType);
            RegisterAsSingleton(input.SingletonRegistration);

            RegisterAssemblies(input.AssemblyToRegister);

            RegisterDbContextAssembly(input.ConnStringKeyAndContext);
            RegisterCoreAssembly(input.AssemblyCoreType);
            RegisterRepositoryAssembly(input.AssemblyRepositoryType);
            RegisterFreeType(input.FreeRegistration);
        }

        protected override void OnBuilded(IContainer container)
        {
            //DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
        private void ConfigureForMvc(Type mvcType)
        {
            ////Microsoft.AspNetCore.Mvc.ControllerBase
            //builder.RegisterAssemblyTypes(mvcType.Assembly)
            //    .Where(t => t.IsSubclassOf(typeof(ControllerBase)))
            //    .PropertiesAutowired();

            //builder.RegisterGeneric(typeof(ControllerBase<>)).PropertiesAutowired();
            builder.RegisterAssemblyTypes(mvcType.Assembly)
                .Where(t => t.Name.EndsWith("Controller"))
               //.Where(t =>t.IsAssignableFrom(typeof(ControllerBase<>)))
               .PropertiesAutowired();
            
            //var sessionCache = new List<SingletonCompositionRegisterInput>();
            //sessionCache.Add(
            //    new SingletonCompositionRegisterInput
            //    {
            //        MappedInterface = typeof(IMvcInHttpSessionCache),
            //        ObjectType = typeof(MvcInHttpSessionCache)
            //    });
            //RegisterAsSingleton(sessionCache);

            var sessionCache = new List<FreeCompositionRegisterInput>();
            sessionCache.Add(
                new FreeCompositionRegisterInput
                {
                    MappedInterface = typeof(IMvcInHttpSessionCache),
                    ObjectType = typeof(MvcInHttpSessionCache),
                    RegistrationType = RegistrationType.ForRequest
                });

            RegisterFreeType(sessionCache);
        }


    }
}
