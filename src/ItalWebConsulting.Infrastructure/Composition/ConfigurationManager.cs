using ItalWebConsulting.Infrastructure.Common;
using ItalWebConsulting.Infrastructure.Comunication;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItalWebConsulting.Infrastructure.Composition
{
    public class ConfigurationManager: IConfigurationManager
    {
        public IConfiguration configuration;
        public ConfigurationManager(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public AppDomainConfig AppDomainConfig { get => GetSection<AppDomainConfig>(); }
        public SmtpSettings SmtpSettings { get => GetSection<SmtpSettings>(); }

        public TSettingsConfig GetSection<TSettingsConfig>() where TSettingsConfig : new()
        {
            var key = typeof(TSettingsConfig).Name;
            return GetSection<TSettingsConfig>(key);
        }

        public TSettingsConfig GetSection<TSettingsConfig>(string key) where TSettingsConfig : new()
        {
            var tSettingsConfig = new TSettingsConfig();
            
            configuration.GetSection(key).Bind(tSettingsConfig);
            return tSettingsConfig;
        }

        //public AppDomainConfig GetAppDomainConfigSection()
        //{
        //    return GetSection<AppDomainConfig>();
        //}
    }

    public interface IConfigurationManager
    {
        AppDomainConfig AppDomainConfig { get; }
        SmtpSettings SmtpSettings { get; }

        TSettingsConfig GetSection<TSettingsConfig>(string key) where TSettingsConfig : new();
        TSettingsConfig GetSection<TSettingsConfig>() where TSettingsConfig : new();
        
    }
}
