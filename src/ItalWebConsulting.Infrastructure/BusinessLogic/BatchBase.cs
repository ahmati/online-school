using ItalWebConsulting.Infrastructure.Composition;
using ItalWebConsulting.Infrastructure.ServiceApp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ItalWebConsulting.Infrastructure.BusinessLogic
{
    public abstract class BatchBase: IServiceApp
    {
        public ConfigurationManager ConfigurationManager { get; private set; }

        private ServiceAppResult serviceAppResult = ServiceAppResult.Ok;
        public ILogger<CoreBase> Logger { get; set; }
        public IServiceProvider ServiceProvider { get; set; }
        //public IMapper Mapper { get; set; }
        //public Lazy<ICacheManager> CacheManager { get; set; }
        public T GetService<T>()
        {
            return (T)ServiceProvider.GetService(typeof(T));
        }
        public async Task<ServiceAppResult> RunAsync(string[] args)
        {
            try
            {
                await RunInternalAsync(args);
            }
            catch (Exception ex)
            {
                SetServiceAppResult(ServiceAppResult.Error);
                Logger.LogError(ex, "Errore Imprevisto durante l'esecuzione del processo");
            }

            return await Task.FromResult(serviceAppResult);
        }
        public void InitiliazeConfigurationManager(IConfiguration configurationRoot)
        {
            ConfigurationManager = new ConfigurationManager(configurationRoot);
        }

        protected abstract Task RunInternalAsync(string[] args);

        public abstract Task<bool> IsServiceEnable(string[] args);

        protected void SetServiceAppResult(ServiceAppResult srvAppResult, params string[] message)
        {
            if (srvAppResult < serviceAppResult) //Setto solo se l'errore è più grave
                ResetServiceAppResult(srvAppResult, message);
            
        }

        protected void ResetServiceAppResult(ServiceAppResult srvAppResult, params string[] message)
        {
            serviceAppResult = srvAppResult;
            if (message != null && message.Length > 0)
            {
                var msg = String.Concat(message);
                switch (srvAppResult)
                {
                    case ServiceAppResult.Ok:
                        Logger.LogDebug(msg);
                        break;
                    case ServiceAppResult.Warn:
                        Logger.LogWarning(msg);
                        break;
                    case ServiceAppResult.Error:
                        Logger.LogError(msg);
                        break;
                    default:
                        Logger.LogInformation(msg);
                        break;
                }
            }
        }
    }
}
