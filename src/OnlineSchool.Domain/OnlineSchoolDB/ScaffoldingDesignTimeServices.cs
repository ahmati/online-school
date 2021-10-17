using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSchool.Domain.OnlineSchoolDB
{
    public class ScaffoldingDesignTimeServices : IDesignTimeServices
    {
        public void ConfigureDesignTimeServices(IServiceCollection services)
        {
            services.AddHandlebarsScaffolding(options =>
            {
                options.ReverseEngineerOptions = ReverseEngineerOptions.DbContextAndEntities;

                // Add custom template data
                options.TemplateData = new Dictionary<string, object>
                {
                    { "models-namespace", "OnlineSchool.Domain.OnlineSchool.EF" },
                    //{ "base-class", "xxx.EntityBase" }
                };

                // Exclude some tables
                //options.ExcludedTables = new List<string> { "xxxx", "dddd" };
            });
        }
    }
}
