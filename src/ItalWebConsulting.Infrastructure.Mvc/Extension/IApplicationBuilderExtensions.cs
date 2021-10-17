using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ItalWebConsulting.Infrastructure.Mvc.Extension
{
    public static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder ConfigureITCulture(this IApplicationBuilder app)
        {
            return app.ConfigureCulture("it-IT");
        }
        //https://docs.telerik.com/aspnet-core/globalization/overview
        public static IApplicationBuilder ConfigureCulture(this IApplicationBuilder app, string culture)
        {
            var supportedCultures = new[] { new CultureInfo(culture) };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(culture),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
            return app;
        }
    }
}
