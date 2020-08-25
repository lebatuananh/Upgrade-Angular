using Base.Logging;
using Base.Logging.Extensions;
using Base.Logging.NLogCustom;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace DVG.CK.OMSApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var host = CreateHostBuilder(args).Build();

                var loggerFactory = (ILoggerFactory)host.Services.GetService(typeof(ILoggerFactory));
                ApplicationLogManager.LoggerFactory = loggerFactory;

                host.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
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
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    //logging.AddConsole();
                    //logging.UseSerilog(configuration);

                    var configFilePath = configuration.GetSection("Logging:Providers:NLog:ConfigFilePath").Get<string>();
                    logging.UseNLog(configFilePath, configuration.GetSection("Logging:KafkaTaget").Get<string>(), LogSourceTypeEnums.CloudKitchenOMSApi);
                }).ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

            return hostBuilder;
        }
    }
}