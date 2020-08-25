using Microsoft.Extensions.Logging;

namespace Base.Logging
{
    public static class ApplicationLogManager
    {
        private static ILoggerFactory loggerFactory = null;

        public static void ConfigureLoggerFactory(ILoggerFactory factory)
        {
        }

        public static ILoggerFactory LoggerFactory
        {
            get
            {
                if (loggerFactory == null)
                {
                    loggerFactory = new LoggerFactory();
                    ConfigureLoggerFactory(loggerFactory);
                }
                return loggerFactory;
            }
            set { loggerFactory = value; }
        }

        public static ILogger CreateLogger(string categoryName)
            => LoggerFactory.CreateLogger(categoryName);
    }
}