using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Base.Logging.Extensions;
using Base.Logging.NLogCustom;

namespace DVG.CK.OMS
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
            var webHost = Host.CreateDefaultBuilder(args)
                .ConfigureLogging((_, logging) =>
          {
              logging.ClearProviders();
              logging.SetMinimumLevel(LogLevel.Information);
              var configFilePath = configuration.GetSection("Logging:Providers:NLog:ConfigFilePath").Get<string>();
              logging.UseNLog(configFilePath, configuration.GetSection("Logging:KafkaTaget").Get<string>(), LogSourceTypeEnums.CloudKitchenWebClient);
          }).ConfigureWebHostDefaults(webBuilder =>
              {
                  webBuilder.UseStartup<Startup>();
              });
            return webHost;
        }
    }
}
