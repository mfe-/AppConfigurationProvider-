using Azure.Data.AppConfiguration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppConfigurationProvider
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddAzureAppConfiguration(this IConfigurationBuilder builder, Func<ConfigurationClient> funcConfigurationClient)
        {
            //var tempConfig = builder.Build();
            //var connectionString = tempConfig.GetConnectionString("WidgetConnectionString");

            return builder.Add(new AppConfigurationConfigurationSource(funcConfigurationClient));
        }
    }
}
