using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrderProcessor.Worker.Handlers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrderProcessor.Worker
{
    internal class Worker : BackgroundService
    {
        private readonly IOrderHandler _handler;
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger, IOrderHandler handler)
        {
            _handler = handler;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _handler.ExecuteAsync();

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Running {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
