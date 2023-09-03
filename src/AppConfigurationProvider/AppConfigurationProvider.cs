using Azure.Data.AppConfiguration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AppConfigurationProvider
{
    /// <summary>
    /// Uses Azure App Configuration to provide configuration for an application.
    /// </summary>
    /// <remarks>
    /// See https://andrewlock.net/creating-a-custom-iconfigurationprovider-in-asp-net-core-to-parse-yaml/ for alternative implementation of a custom configuration provider.
    /// </remarks>
    public class AppConfigurationProvider : ConfigurationProvider, IDisposable
    {
        private readonly ConfigurationClient _ConfigurationClient;
        private readonly string _RootKey = "AppConfiguration";
        private readonly PeriodicTimer _periodicTimer = new (TimeSpan.FromSeconds(60));
        private string _RawJson = String.Empty;
        private bool _updateConfig = true;

        public AppConfigurationProvider(Func<ConfigurationClient> funcConfigurationClient)
        {
            _ConfigurationClient = funcConfigurationClient();

            Task.Run(async () =>
            {
                do
                {
                    await _periodicTimer.WaitForNextTickAsync();
                    await LoadFromAzureAppConfiguration();
                    OnReload();
                } while (_updateConfig);
            });

        }


        public static Dictionary<string, string?> JsonToDictionary(string jsonString)
        {
            var dict = new Dictionary<string, string?>();
            var jsonElement = JsonSerializer.Deserialize<JsonElement>(jsonString, new JsonSerializerOptions()
            {
                AllowTrailingCommas = true,
                ReadCommentHandling = JsonCommentHandling.Skip
            });
            AddProperties(jsonElement, "", dict);
            return dict;
        }

        private static void AddProperties(JsonElement jsonElement, string prefix, Dictionary<string, string?> dict)
        {
            foreach (var property in jsonElement.EnumerateObject())
            {
                var key = prefix + property.Name;
                if (property.Value.ValueKind == JsonValueKind.Object)
                {
                    AddProperties(property.Value, key + ":", dict);
                }
                else
                {
                    dict.Add(key, property.Value.ToString());
                }
            }
        }
        public override void Load()
        {
            LoadFromAzureAppConfiguration().Wait();
        }

        private async Task LoadFromAzureAppConfiguration()
        {
            var response = await _ConfigurationClient.GetConfigurationSettingAsync(_RootKey);
            _RawJson = response.Value.Value;
            Data = JsonToDictionary(_RawJson);
        }

        public string CurrentRawJson()
        {
            return _RawJson;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Cleanup
            _updateConfig = false;
            _periodicTimer.Dispose();
        }
    }
}
