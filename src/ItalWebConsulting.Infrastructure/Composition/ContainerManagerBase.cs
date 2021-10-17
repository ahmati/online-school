using Autofac;
using ItalWebConsulting.Infrastructure.BusinessLogic;
using ItalWebConsulting.Infrastructure.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ItalWebConsulting.Infrastructure.Composition
{
    public abstract class ContainerManagerBase
    {
        protected readonly ContainerBuilder builder;

        protected IContainer container = null;

        protected ContainerManagerBase(ContainerBuilder builder)
        {
            this.builder = builder;
        }

        //Try to understand if this works with batch app
        protected ContainerManagerBase()
        {
            this.builder = new ContainerBuilder();
        }
        //{
        //    return Resolver.GetService<TService>();
        //}
        public void Configure(CompositionRegisterInput input)
        {
            ConfigureInternal(input);
            //container = builder.Build();

            //OnBuilded(container);

            //return container;
        }

        protected abstract void ConfigureInternal(CompositionRegisterInput input);

        protected abstract void OnBuilded(IContainer container);

        protected void RegisterAssemblies(IEnumerable<Type> assemblyToRegister)
        {
            if (assemblyToRegister != null)
                foreach (var a in assemblyToRegister)
                {
                    builder.RegisterAssemblyTypes(a.Assembly);
                }
        }

        protected void RegisterRepositoryAssembly(IEnumerable<AssemblyCompositionRegisterInput> assemblyRepositoryType)
        {
            if (assemblyRepositoryType != null)
                foreach (var arType in assemblyRepositoryType)
                {
                    var a = arType.ObjectType;
                    //Here I'm registering all that inherit from an Interface
                    if (arType.RegistrationType == RegistrationType.ForRequest)
                        builder.RegisterAssemblyTypes(a.Assembly)
                            .Where(t => t.Name.EndsWith("Repository") && t.GetInterfaces().Any())
                            .As(t => t.GetInterfaces()[0]).PropertiesAutowired().InstancePerLifetimeScope();
                    else
                        builder.RegisterAssemblyTypes(a.Assembly)
                       .Where(t => t.Name.EndsWith("Repository") && t.GetInterfaces().Any())
                       .As(t => t.GetInterfaces()[0]).PropertiesAutowired();

                    //Here I'm registering all that Doesn't inherit from an Interface
                    if (arType.RegistrationType == RegistrationType.ForRequest)
                        builder.RegisterAssemblyTypes(a.Assembly)
                        .Where(t => t.Name.EndsWith("Repository") && !t.GetInterfaces().Any())
                        .PropertiesAutowired().InstancePerLifetimeScope();
                    else
                        builder.RegisterAssemblyTypes(a.Assembly)
                        .Where(t => t.Name.EndsWith("Repository") && !t.GetInterfaces().Any())
                        .PropertiesAutowired();

                }
        }

        protected void RegisterDbContextAssembly(IDictionary<string, AssemblyCompositionRegisterInput> connStringKeyAndContext)
        {
            if (connStringKeyAndContext != null)
                foreach (var kv in connStringKeyAndContext)
                {
                    if (kv.Value.RegistrationType == RegistrationType.ForRequest)
                        builder.RegisterType(kv.Value.ObjectType)
                            .AsSelf().As<DbContextBase>()
                        .WithParameter("connectionString", kv.Key)
                        .InstancePerLifetimeScope();
                    else
                        builder.RegisterType(kv.Value.ObjectType)
                        .WithParameter("connectionString", kv.Key);

                }

        }

        protected void RegisterCoreAssembly(IEnumerable<Type> assemblyCoreType)
        {
            if (assemblyCoreType != null)
                foreach (var a in assemblyCoreType)
                {
                    //Here I'm registering all that inherit from an Interface
                    builder.RegisterAssemblyTypes(a.Assembly)
                        .Where(t => t.IsSubclassOf(typeof(CoreBase)) && t.GetInterfaces().Any())
                        .As(t => t.GetInterfaces()[0]).PropertiesAutowired();

                    //Here I'm registering all that Doesn't inherit from an Interface
                    builder.RegisterAssemblyTypes(a.Assembly)
                        .Where(t => t.IsSubclassOf(typeof(CoreBase)) && !t.GetInterfaces().Any()).PropertiesAutowired();
                }
        }

        protected void RegisterFreeType(IEnumerable<FreeCompositionRegisterInput> freeRegistration)
        {
            if (freeRegistration != null)
                foreach (var f in freeRegistration)
                {
                    var obj = f.ObjectType;
                    var inter = f.MappedInterface;
                    var regType = f.RegistrationType;
                    if (inter != null && !inter.IsInterface)
                        throw new ArgumentException("Il tipo MappedInterface in input: " + inter.GetType().Name + " deve essere una interfaccia");

                    if (inter != null)
                    {
                        if (regType == RegistrationType.ForRequest)
                            builder.RegisterType(obj).As(inter).InstancePerLifetimeScope();
                        else
                            builder.RegisterType(obj).As(inter);
                    }
                    else
                    {
                        if (regType == RegistrationType.ForRequest)
                            builder.RegisterType(obj).InstancePerLifetimeScope();
                        else
                            builder.RegisterType(obj);
                    }
                }

        }


        protected void RegisterAsSingleton(IEnumerable<SingletonCompositionRegisterInput> singletonRegistration)
        {

            if (singletonRegistration != null)
                foreach (var f in singletonRegistration)
                {
                    if (f.IsGeneric)
                        RegisterAsGenericSingleton(f);
                    else
                    {
                        var obj = f.ObjectType;
                        var inter = f.MappedInterface;
                        var par = f.CtorParam;

                        if (inter != null && par != null)
                            builder.RegisterType(obj)
                                .As(inter).WithParameter(par.Code, par.Value).PropertiesAutowired().SingleInstance();
                        else if (par != null)
                            builder.RegisterType(obj).WithParameter(par.Code, par.Value).PropertiesAutowired().SingleInstance();
                        else if (inter != null && par == null)
                            builder.RegisterType(obj).As(inter).PropertiesAutowired().SingleInstance();
                        else
                            builder.RegisterType(obj).PropertiesAutowired().SingleInstance();
                    }
                   
                }
        }

        private void RegisterAsGenericSingleton(SingletonCompositionRegisterInput singletonRegistration)
        {
            var obj = singletonRegistration.ObjectType;
            var inter = singletonRegistration.MappedInterface;
            var par = singletonRegistration.CtorParam;
            //builder.RegisterAssemblyTypes(obj.Assembly).AsClosedTypesOf(inter);

            if (inter != null && par != null)
                builder.RegisterAssemblyTypes(obj.Assembly).AsClosedTypesOf(inter)
                    .WithParameter(par.Code, par.Value).PropertiesAutowired().SingleInstance();
            else if (par != null)
                builder.RegisterAssemblyTypes(obj.Assembly).AsClosedTypesOf(inter).WithParameter(par.Code, par.Value).PropertiesAutowired().SingleInstance();
            else if (inter != null && par == null)
                builder.RegisterAssemblyTypes(obj.Assembly).AsClosedTypesOf(inter).PropertiesAutowired().SingleInstance();
            else
                builder.RegisterAssemblyTypes(obj.Assembly).AsClosedTypesOf(inter).PropertiesAutowired().SingleInstance();
        }


    }
}
