using Autofac;
using ItalWebConsulting.Infrastructure.Composition;
using ItalWebConsulting.Infrastructure.Comunication;
using ItalWebConsulting.Infrastructure.Logging;
using ItalWebConsulting.Infrastructure.Mvc.Extension;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using OnlineSchool.Core.Identity_.Services;
using OnlineSchool.Core.Infrastructure.FileUpload;
using OnlineSchool.Core.Payment_.Service;
using OnlineSchool.Core.Test_.Repositories;
using OnlineSchool.Domain.OnlineSchoolDB.EF.Context;
using System.Collections.Generic;

namespace OnlineSchool.Site
{
    public partial class Startup
    {
        public IContainer ApplicationContainer { get; private set; }

        //private void LoadAutofacAuthentication(IServiceCollection services)
        //{
        //}

        private void LoadAutofac(ContainerBuilder builder)
        {
            //LoadAutofacAuthentication(services);

            var repository = new List<AssemblyCompositionRegisterInput>();
            repository.Add(new AssemblyCompositionRegisterInput
            {
                ObjectType = typeof(ITestRepository),
                RegistrationType = RegistrationType.ForRequest
            });

            var compositionBaseInput = new CompositionRegisterInput
            {
                AssemblyCoreType = new[] { typeof(IdentityService) },
                //BaseControllerType = typeof(ControllerBase),
                ConnStringKeyAndContext = GetConnStringAndContext(Configuration.GetConnectionString("DefaultConnection")),
                AssemblyMvcType = this.GetType(),
                AssemblyRepositoryType = repository,
                SingletonRegistration = new List<SingletonCompositionRegisterInput>(),
                FreeRegistration = new List<FreeCompositionRegisterInput>()
            };

            #region Singleton registration

            compositionBaseInput.SingletonRegistration.Add(new SingletonCompositionRegisterInput
            {
                MappedInterface = typeof(IEmailService),
                ObjectType = typeof(EmailService)
            });

            compositionBaseInput.SingletonRegistration.Add(new SingletonCompositionRegisterInput
            {
                ObjectType = typeof(LoggerManager)
            });
            
            compositionBaseInput.SingletonRegistration.Add(new SingletonCompositionRegisterInput
            {
                ObjectType = typeof(PurchasableItemFactory)
            });

            compositionBaseInput.SingletonRegistration.Add(new SingletonCompositionRegisterInput
            {
                MappedInterface = typeof(IFileService),
                ObjectType = typeof(FileService)
            });

            //compositionBaseInput.SingletonRegistration.Add(new SingletonCompositionRegisterInput
            //{
            //    MappedInterface = typeof(ICacheManager),
            //    ObjectType = typeof(IwMemoryCache)
            //});

            //compositionBaseInput.SingletonRegistration.Add(new SingletonCompositionRegisterInput
            //{
            //    MappedInterface = typeof(IStringLocalizerFactory),
            //    ObjectType = typeof(ResourceManagerStringLocalizerFactory)
            //});

            //compositionBaseInput.SingletonRegistration.Add(new SingletonCompositionRegisterInput
            //{
            //    MappedInterface = typeof(IResourceManager),
            //    ObjectType = typeof(ResourceManager)
            //});

            //compositionBaseInput.SingletonRegistration.Add(new SingletonCompositionRegisterInput
            //{
            //    MappedInterface = typeof(IResourceMvcManager),
            //    ObjectType = typeof(ResourceMvcManager)
            //});

            #endregion

            #region Free registration

            compositionBaseInput.FreeRegistration.Add(new FreeCompositionRegisterInput
            {
                ObjectType = typeof(IdentityService),
                MappedInterface = typeof(IIdentityService),
                RegistrationType = RegistrationType.ForRequest
            });

            compositionBaseInput.FreeRegistration.Add(new FreeCompositionRegisterInput
            {
                ObjectType = typeof(ConfigurationManager),
                MappedInterface = typeof(IConfigurationManager),
                RegistrationType = RegistrationType.ForRequest
            });

            compositionBaseInput.FreeRegistration.Add(new FreeCompositionRegisterInput
            {
                ObjectType = typeof(RoleManager<IdentityRole>),
                IsGeneric = true
            });

            #endregion

            _services.AddAutoFacForMvc(builder, compositionBaseInput);
        }

        private static IDictionary<string, AssemblyCompositionRegisterInput> GetConnStringAndContext(string connectionString)
        {
            IDictionary<string, AssemblyCompositionRegisterInput> connStringKeyAndContext = new Dictionary<string, AssemblyCompositionRegisterInput>();
            //var connServ = new ConfigurationService();
            connStringKeyAndContext.Add(
                connectionString,
                new AssemblyCompositionRegisterInput
                {
                    ObjectType = typeof(OnlineSchoolDbContext),
                    RegistrationType = RegistrationType.ForRequest
                }
                );
            return connStringKeyAndContext;
        }
    }
}
