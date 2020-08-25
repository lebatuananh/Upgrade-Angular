using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DVG.WIS.Business;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DVG.CK.Services
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private OrderAgent orderAgent;

        public Worker(ILogger<Worker> logger)
        {
            Console.WriteLine("Hello Thank Dai Ca");
            _logger = logger;
            orderAgent = new OrderAgent();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    orderAgent.Run();

                    _logger.LogInformation($"Worker running at: { DateTimeOffset.Now}");
                    _logger.LogInformation("toannq test");
                    Console.WriteLine($"Worker running at: {DateTimeOffset.Now}");
                    await Task.Delay(6000, stoppingToken);
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
