using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItalWebConsulting.Infrastructure.Extension
{
    public static class MicrosoftConfigurationExtension
    {
        public static TSettingsConfig GetSection<TSettingsConfig>(this IConfiguration configuration, string key) where TSettingsConfig : new()
        {
            var tSettingsConfig = new TSettingsConfig();

            configuration.GetSection(key).Bind(tSettingsConfig);
            return tSettingsConfig;
        }

        public static IConfigurationSection GetSection<TSettingsConfig>(this IConfiguration configuration, string key, out TSettingsConfig tSettingsConfig) where TSettingsConfig : new()
        {
            //if (tSettingsConfig == null)
            tSettingsConfig = new TSettingsConfig();
            var cs = configuration.GetSection(key);
            cs.Bind(tSettingsConfig);
            return cs;
        }
    }
}
