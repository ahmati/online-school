using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Linq;
using System.IO;
using ItalWebConsulting.Infrastructure.Extension;
using ItalWebConsulting.Infrastructure.Comunication;
using Autofac;


using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using NLog.Extensions.Logging;

namespace ItalWebConsulting.Infrastructure.ServiceApp
{
    public abstract class ServiceAppBase<TProgram> where TProgram:class
    {
        //private static NLog.ILogger logger = LogManager.GetCurrentClassLogger();

        public static IContainer ApplicationContainer { get; protected set; }
        public static ServiceCollection Services { get; protected set; }
        
        public static ILogger<TProgram> Logger { get; protected set; }

        public static IConfigurationRoot ConfigurationRoot { get; protected set; }
        public static ConfigurationBuilder Builder { get; protected set; }

        protected static async Task<ServiceAppResult> StartBatchExecution(ConfigAppInput input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            if (input.AssemblyContainerBatch == null)
                throw new ArgumentNullException(nameof(input.AssemblyContainerBatch),"Inserire uno o più assembly in cui si trovano le classi batch da avviare");

            if (input.ConfigureStandardAutofact == null)
                throw new ArgumentNullException(nameof(input.ConfigureStandardAutofact));

            //if (string.IsNullOrWhiteSpace(input.BatchName))
            //    throw new ArgumentNullException(nameof(input.BatchName));
            try
            {
                Console.WriteLine(DateTime.Now.ToString() + " - Inizio avvio applicazione...");
                ConfigureAppSettingsRoot();
                LoadServiceCollection(input.AddSmtpSettingsConfiguration, input.AddNLogConfiguration);

                var sau = new ServiceAppUtils(input.AssemblyContainerBatch.ToArray());
                var service = sau.GetRunnableService(input.BatchArgs);

                ApplicationContainer = input.ConfigureStandardAutofact(service);
                
                //if(input.AddNLogConfiguration)
                //    StartNLog();

                Logger = ApplicationContainer.Resolve<ILogger<TProgram>>();
                if (Logger != null)
                    Logger.LogInformation("Start applicazione");

                //Logger.LogDebug("DEBUG");
                //Logger.LogTrace("TRACE");

                return await StartBatch(input);
            }
            catch (Exception ex)
            {
                if (Logger != null)
                    Logger.LogError(ex, "Errore non gestito durante l'esecuzione del servizio");
                else
                    Console.WriteLine("Errore non gestito durante l'esecuzione del servizio (Logger non inizializzato). Testo eccezione: " + ex.ToString());

                return ServiceAppResult.Error;
            }
            finally
            {
                StopApp();
            }
        }

        protected static void StopApp()
        {
            
            NLog.LogManager.Shutdown();
            if (ApplicationContainer != null)
                ApplicationContainer.Dispose();
            Console.WriteLine(DateTime.Now.ToString() + " - Fine esecuzione");
        }

        protected static void ConfigureAppSettingsRoot()
        {
            var builder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              //.AddUserSecrets<Program>()
              .AddEnvironmentVariables();

            ConfigurationRoot = builder.Build();
        }

        protected static void LoadServiceCollection(bool addSmtpSettingsConfiguration, bool addNLogConfiguration)
        {
            Services = new ServiceCollection();
            if (addNLogConfiguration)
            {
                Services.AddLogging(loggingBuilder =>
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                    loggingBuilder.AddNLog();
                });
                //var nLogConfiguration = ConfigurationRoot.GetSection<NLogConfiguration>("NLogConfiguration");
                //Services.AddNLog(nLogConfiguration);
            }
            Services.AddMemoryCache();
            

            if (addSmtpSettingsConfiguration)
            {
                var smtpSettings = new SmtpSettings();
                var confSmtp = ConfigurationRoot.GetSection("SmtpSettings", out smtpSettings);

                Services.Configure<SmtpSettings>(confSmtp);
            }
            
        }

        //protected static void StartNLog()
        //{
        //    //var loggerFactory = ApplicationContainer.Resolve<ILoggerFactory>();
        //    //loggerFactory.AddNLog();
        //}
        
        private static async Task<ServiceAppResult> StartBatch(ConfigAppInput input)
        {
            //var sau = new ServiceAppUtils(input.AssemblyContainerBatch.ToArray());
            //var service = sau.GetRunnableService(input.BatchArgs);
            var args = input.BatchArgs.Length > 1 ? input.BatchArgs.Skip(1).ToArray() : (new List<string>()).ToArray();
            var service = ApplicationContainer.Resolve<IServiceApp>();
            if (service == null)
                throw new ArgumentException("Autofact non ha caricato alcuno batch eseguibile per l'interfaccia  IServiceApp. Verificare il Batch con chiave: " + input.BatchArgs[0]);
            var isEnable = await service.IsServiceEnable(input.BatchArgs);
            if (isEnable)
            {
                service.InitiliazeConfigurationManager(ConfigurationRoot);
                return await service.RunAsync(args);
            }
            else
            {
                Logger.LogWarning("Servizio " + input.BatchArgs[0] + " è disabilitato, nessuna esecuzione. Abilitare il servizio per poterlo eseguire");
                return await Task.FromResult(ServiceAppResult.ServiceDisable);
            }
        }

    }
}
