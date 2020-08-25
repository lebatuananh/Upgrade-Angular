using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Base.Logging.NLogCustom;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Base.Logging.Extensions;
namespace DVG.CK.OMS.PublicApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
              .AddJsonFile("appsettings.json", true)
              .AddJsonFile($"appsettings.{environmentName}.json", true)
              .Build();
            var webhostBuilder = WebHost.CreateDefaultBuilder(args)
              .ConfigureLogging((_, logging) =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(LogLevel.Information);
                var configFilePath = configuration.GetSection("Logging:Providers:NLog:ConfigFilePath").Get<string>();
                logging.UseNLog(configFilePath, configuration.GetSection("Logging:KafkaTaget").Get<string>(), LogSourceTypeEnums.CloudKitchenWebApi);
            })
                .UseStartup<Startup>();
            return webhostBuilder;
        }
    }
}
