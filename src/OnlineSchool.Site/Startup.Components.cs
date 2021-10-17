using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using OnlineSchool.Contract.Globalization;
using OnlineSchool.Contract.Identity.Models;
using OnlineSchool.Site.Models;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace OnlineSchool.Site
{
    public partial class Startup
    {
        private void AddLocalization(IServiceCollection services)
        {
            var resourcesPath = "Resources";
            services.AddLocalization(options => options.ResourcesPath = resourcesPath);

            services.AddMvc()
                .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix,
                opts => { opts.ResourcesPath = resourcesPath; })
                .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.SubFolder)
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                      factory.Create(typeof(Contract.Resources.SharedDataAnnotationResourcesKey));
                });

            services.Configure<RequestLocalizationOptions>(
                options => {
                    var supportedCultures = new[]
                    {
                        new CultureInfo(CultureNaming.CultureInfoIt),
                        new CultureInfo(CultureNaming.CultureInfoEn),
                    };
                    options.DefaultRequestCulture = new RequestCulture(culture: CultureNaming.CultureInfoIt, uiCulture: CultureNaming.CultureInfoIt);
                    options.SupportedCultures = supportedCultures;
                    options.SupportedUICultures = supportedCultures;
                    options.RequestCultureProviders = new List<IRequestCultureProvider>
                    {
                        new QueryStringRequestCultureProvider(),
                        new CookieRequestCultureProvider()
                    };
                }
            );
        }

        private void AddAuthentication(IServiceCollection services)
        {
            services
                .AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                   options.SignIn.RequireConfirmedAccount = true;
                })
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                //options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);

                //options.LoginPath = "/Identity/Account/Login";
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            services.Configure<SecurityStampValidatorOptions>(options =>
                options.ValidationInterval = TimeSpan.FromMinutes(0));
        }
    }
}
