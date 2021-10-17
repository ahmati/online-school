using Autofac;
using Autofac.Extensions.DependencyInjection;
using ItalWebConsulting.Infrastructure.Comunication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItalWebConsulting.Infrastructure.Composition
{
    public class ContainerManagerBatch : ContainerManagerBase
    {
        public ContainerManagerBatch(IServiceCollection service)
        {
            //builder.Populate(service);
        }

        //public static TService GetInstance<TService>() => Resolver.GetService<TService>();
        protected override void ConfigureInternal(CompositionRegisterInput input)
        {
            //builder.RegisterModule<Autofac.Extras.NLog.NLogModule>();
            CustomRegistration();

            RegisterAsSingleton(input.SingletonRegistration);

            RegisterAssemblies(input.AssemblyToRegister);

            RegisterDbContextAssembly(input.ConnStringKeyAndContext);
            RegisterCoreAssembly(input.AssemblyCoreType);
            RegisterRepositoryAssembly(input.AssemblyRepositoryType);
            RegisterFreeType(input.FreeRegistration);
        }

        private void CustomRegistration()
        {
            builder.RegisterType<LoggerFactory>()
                .As<ILoggerFactory>()
                .SingleInstance();
            builder.RegisterGeneric(typeof(Logger<>))
                .As(typeof(ILogger<>))
                .SingleInstance();

            builder.RegisterType<ItalWebConsulting.Infrastructure.Comunication.EmailService>()
               .As<IEmailService>()
               .SingleInstance();
        }

        protected override void OnBuilded(IContainer container)
        {
            //DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
       
    }
}
