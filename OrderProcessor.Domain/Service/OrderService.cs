using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrderProcessor.Domain.Entity;
using OrderProcessor.Domain.Enum;
using Settings = OrderProcessor.Domain.RabbitMQFactory;
using System;
using System.Threading;
using System.Threading.Tasks;
using OrderProcessor.Domain.Options;

namespace OrderProcessor.Domain.Service
{
    public class OrderService : IOrderService
    {
        private readonly RabbitMqOptions _options;
        private readonly ILogger<OrderService> _logger;
        private int _count = 0;

        public OrderService(IOptions<RabbitMqOptions> options, ILogger<OrderService> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        public async Task ExecuteAsync(Order order)
        {
            await Task.Run(() =>
            {
                try
                {
                    Thread.Sleep(5000); // Simule some process
                    order.State = State.Done;

                    SimulateException();
                    _logger.LogInformation("Service executed with success - Thread -> {0}", Thread.GetCurrentProcessorId());
                }
                catch (Exception ex)
                {
                    order.State = State.Error;
                    _logger.LogError("Service didn't execute, error {1} - Thread -> {0} ", Thread.GetCurrentProcessorId(), ex.Message);
                    
                    throw;
                }
            });
        }

        public void Publish(Order order)
        {
            using var connection = Settings.RabbitMQFactory.GetConnection(_options);
            Settings.RabbitMQFactory.SendMessage(order, _options, connection);
        }

        private void SimulateException()
        {
            if (_count > 5)
            {
                _count = 0;
                throw new Exception("Simulate exception");
            }

            _count++;
        }
    }
}
