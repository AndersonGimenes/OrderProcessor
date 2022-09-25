using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrderProcessor.Domain.Options;
using OrderProcessor.Domain.RabbitMQFactory;
using OrderProcessor.Domain.Service;
using OrderProcessor.Worker.Options;
using OrderProcessor.Worker.WebSocketClient;
using System;
using System.Threading.Tasks;

namespace OrderProcessor.Worker.Handlers
{
    internal class OrderHandler : IOrderHandler
    {
        private readonly IOrderService _service;
        private readonly RabbitMqOptions _habbitOptions;
        private readonly ILogger<OrderHandler> _logger;
        private readonly WebSocketOptions _webSocketOptions;

        public OrderHandler
        (
            ILogger<OrderHandler> logger, 
            IOrderService service, 
            IOptions<RabbitMqOptions> habbitOptions,
            IOptions<WebSocketOptions> webSocketOptions
        )
        {
            _service = service;
            _habbitOptions = habbitOptions.Value;
            _logger = logger;
            _webSocketOptions = webSocketOptions.Value;
        }

        public async Task ExecuteAsync()
        {
            var connection = RabbitMQFactory.GetConnection(_habbitOptions);

            RabbitMQFactory.CreateQueue(_habbitOptions, connection);
            RabbitMQFactory.ConsumeMessage
            (
                _habbitOptions, 
                _logger, 
                _service, 
                connection, 
                async x => await WebSocketFactory.HubClientSend("SendMessage", x, _webSocketOptions)
            );            

            _logger.LogInformation("Stopping service {time}", DateTimeOffset.Now);
            await Task.CompletedTask;
        }
    }
}
