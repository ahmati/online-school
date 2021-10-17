using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ItalWebConsulting.Infrastructure.ServiceApp
{
    public interface IServiceApp
    {
        void InitiliazeConfigurationManager(IConfiguration configurationRoot);
        Task<ServiceAppResult> RunAsync(string[] args);
        Task<bool> IsServiceEnable(string[] args);
      //  ServiceAppResult Run(string[] args);

       
    }
}
