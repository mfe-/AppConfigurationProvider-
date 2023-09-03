using Azure.Data.AppConfiguration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Reflection.Emit;

namespace AppConfigurationProvider.ConsoleApp3
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            var builder = new HostBuilder();
            builder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                //config.AddAzureAppConfiguration();
                //config.AddJsonFile(appSettingsFile);

                //config.Sources.Clear();
                config.AddAzureAppConfiguration(() => 
                    new ConfigurationClient("Endpoint="));
                config.AddCommandLine(args);
            }).ConfigureServices((hostContext, services) =>
            {
                services.Configure<AppConfiguration>(hostContext.Configuration.GetSection("EventProducerServiceConfiguration"));


                services.AddOptions();
                services.AddLogging();
                services.AddSingleton<App>();
            }).ConfigureLogging((hostingContext, logging) =>
            {

                logging.ClearProviders();
                logging.AddConsole();
                logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
            }).UseConsoleLifetime();



            var host = builder.Build();

            var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));
            do
            {
                var x = host.Services.GetRequiredService<IOptionsMonitor<AppConfiguration>>().CurrentValue;
                Console.WriteLine($"NumberOfWorkers {x.NumberOfWorkers} WorkerBatchSize {x.WorkerBatchSize}");

                await timer.WaitForNextTickAsync();
            } while (true);
   
            host.Run();
        }
    }
}