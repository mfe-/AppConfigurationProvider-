using Azure.Data.AppConfiguration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppConfigurationProvider
{
    public class AppConfigurationConfigurationSource : IConfigurationSource
    {
        private readonly Func<ConfigurationClient> _funcConfigurationClient;

        public AppConfigurationConfigurationSource(Func<ConfigurationClient> funcConfigurationClient)
        {
            this._funcConfigurationClient = funcConfigurationClient;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new AppConfigurationProvider(_funcConfigurationClient);
        }
    }
}
