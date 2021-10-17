using System;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using ItalWebConsulting.Infrastructure.Logging;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace OnlineSchool.Site
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Always call this beforehand
            LogConfigurator.Configure();
            var logger = new LoggerManager();

            try
            {
                logger.Debug("Starting host builder");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
            .UseNLog();
        //  .UseNLog();  // NLog: Setup NLog for Dependency injection

        //public static void Main(string[] args)
        //{ 
        //   var configuration = new ConfigurationBuilder()
        //   .AddJsonFile("appsettings.json")
        //   .Build();
        //     Serilog.Log.Logger = new LoggerConfiguration()
        //      .ReadFrom.Configuration(configuration)

        //      //.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        //      .CreateLogger();

        //     BuildWebHost(args).Run();

        //    //CreateHostBuilder(args).Build().Run();
        //}

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        });

        //public static IWebHost BuildWebHost(string[] args) =>
        //WebHost.CreateDefaultBuilder(args)
        //.UseKestrel()
        //.UseContentRoot(Directory.GetCurrentDirectory())
        //.UseWebRoot("wwwroot")
        //.UseStartup<Startup>()
        //.UseIIS()
        //.UseIISIntegration()
        //.UseSerilog()
        //.Build();
    }
}
