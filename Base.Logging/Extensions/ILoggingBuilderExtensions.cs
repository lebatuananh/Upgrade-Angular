using Base.Logging.NLogCustom;
using Base.Logging.StaticConfig;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;
using Serilog;
using System.Collections.Generic;

namespace Base.Logging.Extensions
{
    public static class ILoggingBuilderExtensions
    {
        public static void UseSerilog(this ILoggingBuilder builder, IConfiguration configuration)
        {
            builder.AddSerilog();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration, "Logging:Providers:Serilog")
                .CreateLogger();

            //Log.Logger = new LoggerConfiguration()
            //    .WriteTo.Console()
            //    .WriteTo.Logger(configuration =>
            //    {
            //        configuration.WriteTo.File("Logs/error.log");
            //        configuration.Filter.ByIncludingOnly(ev => ev.)
            //    })
            //    .CreateLogger();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder">Using ILogging builder</param>
        /// <param name="configFile">nlog.config file</param>
        /// <param name="kafkaTaget">Ip and port of Kafka cluster</param>
        /// <param name="applicationStore">Application name and Application Id</param>
        public static void UseNLog(this ILoggingBuilder builder, string configFile, string kafkaTaget, LogSourceTypeEnums applicationStore)
        {
            NLogTargetCustom.RegisterTarget();
            StaticConfiguration.KafkaServer = kafkaTaget;
            StaticConfiguration.ApplicationStore = new Dictionary<int, string> { { (int)applicationStore, (EnumConvert.GetEnumDescription(applicationStore)) } };
            builder.AddNLog(configFile);
        }
    }
}