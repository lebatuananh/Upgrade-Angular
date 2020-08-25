using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Base.Logging.NLogCustom;
using DVG.WIS.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Base.Logging.Extensions;
using NLog.Web;

namespace DVG.CK.Services
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
              .AddJsonFile("appsettings.json", true)
              .AddJsonFile($"appsettings.{environmentName}.json", true)
              .Build();
            var hostBuilder = Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(config =>
                {
                    config.AddConfiguration(configuration);
                    AppSettings.Instance.SetConfiguration(configuration);
                }).ConfigureLogging((_, logging) =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Information);
                    var configFilePath = configuration.GetSection("Logging:Providers:NLog:ConfigFilePath").Get<string>();
                    logging.UseNLog(configFilePath, configuration.GetSection("Logging:KafkaTaget").Get<string>(), LogSourceTypeEnums.CloudKitchenWorker);
                }).UseSystemd()
               .ConfigureServices((hostContext, services) =>
               {

                   services.AddHostedService<Worker>();
               });
            return hostBuilder;
        }
    }
}
