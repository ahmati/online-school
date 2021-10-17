using Autofac;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using OnlineSchool.Contract.Infrastructure;
using OnlineSchool.Contract.Identity.Validators;
using OnlineSchool.Core;
using OnlineSchool.Core.Students_.Repository;
using OnlineSchool.Domain.OnlineSchoolDB.EF.Context;
using OnlineSchool.Site.Models;
using OnlineSchool.Site.Quartz;
using Stripe;
using System;
using OnlineSchool.Core.Session_.Repository;

namespace OnlineSchool.Site
{
    //https://autofaccn.readthedocs.io/en/latest/integration/aspnetcore.html
    public partial class Startup
    {
        private IServiceCollection _services;
        public IConnectionStrings ConnectionString { get; set; }

        public Startup(IHostEnvironment env)
        {
            // In ASP.NET Core 3.0 `env` will be an IWebHostEnvironment, not IHostingEnvironment.
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            this.Configuration = builder.Build();
        }
        public IConfigurationRoot Configuration { get; private set; }

        //public ILifetimeScope AutofacContainer { get; private set; }

        // This is the default if you don't have an environment specific method.
        public void ConfigureServices(IServiceCollection services)
        {
            this._services = services;

            services.AddKendo();

            services.AddAutoMapper(typeof(Startup), typeof(MapperConfig));
            MapperConfig.InitializeAutoMapper();

            services.AddDistributedMemoryCache();

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
            });
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);//You can set Time   
            });

            // -------------------------------------------- DbContexts -----------------------------------------------
            ConnectionString = Configuration.GetSection("ConnectionStrings").Get<ConnectionStrings>();

            services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(ConnectionString.DefaultConnection));
            services.AddDbContext<OnlineSchoolDbContext>(opt => opt.UseSqlServer(ConnectionString.DefaultConnection));
            // -------------------------------------------------------------------------------------------------------

            services.AddAuthentication("esvlogin").AddCookie("esvlogin");

            services.AddHttpContextAccessor();
            services.AddHttpClient();
            AddLocalization(services);

            // Quartz
            services.AddQuartzService();

            services
                .AddMvc()
                .AddControllersAsServices()
                .AddRazorRuntimeCompilation();

            services.AddMvc()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<RegisterUtenteValidator>());


            AddAuthentication(services);
            services.Configure<Contract.Configuration.StripeSettingKeys>(Configuration.GetSection("Stripe"));
        }

        // This only gets called if your environment is Development. The
        // default ConfigureServices won't be automatically called if this
        // one is called.
        //public void ConfigureDevelopmentServices(IServiceCollection services)
        //{
        //    // Add things to the service collection that are only for the
        //    // development environment.
        //}

        // This is the default if you don't have an environment specific method.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Add things to the Autofac ContainerBuilder.
            //builder.RegisterType<TestService>().As<ITestService>();
            //builder.RegisterAssemblyTypes(this.GetType().Assembly).Where(t => t.Name.EndsWith("Controller")).PropertiesAutowired();
            LoadAutofac(builder);

        }

        // This only gets called if your environment is Production. The
        // default ConfigureContainer won't be automatically called if this
        // one is called.
        //public void ConfigureProductionContainer(ContainerBuilder builder)
        //{
        //    // Add things to the ContainerBuilder that are only for the
        //    // production environment.
        //}

        // This is the default if you don't have an environment specific method.
        //public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        //{
        //    // Set up the application.
        //}

        //// This only gets called if your environment is Staging. The
        //// default Configure won't be automatically called if this one is called.
        //public void ConfigureStaging(IApplicationBuilder app, ILoggerFactory loggerFactory)
        //{
        //    // Set up the application for staging.
        //}


        // This method gets called by the runtime. Use this method to add services to the container.
        //public IServiceProvider ConfigureServices(IServiceCollection services)
        //{

        //    LoadAutofac(services);
        //    return new AutofacServiceProvider(this.ApplicationContainer);
        //}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

        [Obsolete]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseSession();
            StripeConfiguration.SetApiKey(Configuration.GetSection("Stripe")["SecretKey"]);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //StripeConfiguration.SetApiKey(Configuration["Stripe:SecretKey"]);
            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);
            var culture = locOptions.Value.DefaultRequestCulture.Culture;
            app.UseRequestLocalization(app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
            app.UseHttpsRedirection();
            app.UseCookiePolicy();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
