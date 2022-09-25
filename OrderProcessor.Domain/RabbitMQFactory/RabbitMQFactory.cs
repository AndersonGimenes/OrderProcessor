using Microsoft.Extensions.Logging;
using OrderProcessor.Domain.Enum;
using OrderProcessor.Domain.Options;
using OrderProcessor.Domain.Service;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;

namespace OrderProcessor.Domain.RabbitMQFactory
{
    public static class RabbitMQFactory
    {
        public static void SendMessage<T>(T model, RabbitMqOptions options, IModel channel)
        {
            string message = JsonSerializer.Serialize(model);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: options.RoutingKey,
                                 basicProperties: null,
                                 body: body);
        }

        public static void CreateQueue(RabbitMqOptions options, IModel channel)
        {            
            channel.QueueDeclare(queue: options.Queue,
                                durable: false,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);
        }

        public static void ConsumeMessage<T>
        (
            RabbitMqOptions options, 
            ILogger logger, 
            IServiceBase<T> service, 
            IModel channel,
            Action<string> sendMessage
        )
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    logger.LogInformation("Received {0}", message);

                    T result = JsonSerializer.Deserialize<T>(message);

                    if (result != null)
                    {
                        logger.LogInformation("Executing service {0}", DateTimeOffset.Now);
                        await service.ExecuteAsync(result);
                    }

                    channel.BasicAck(ea.DeliveryTag, false);

                    sendMessage.Invoke(State.Done.ToString());
                }
                catch (Exception ex)
                {
                    logger.LogError("Error {0}", ex.Message);
                    channel.BasicNack(ea.DeliveryTag, false, true);
                    
                    sendMessage.Invoke(State.Error.ToString());
                }
            };

            channel.BasicConsume(queue: options.Queue,
                                 autoAck: false,
                                 consumer: consumer);
        }

        public static IModel GetConnection(RabbitMqOptions options)
        {
            var connection = new ConnectionFactory() { HostName = options.HostName };
            return connection.CreateConnection().CreateModel();
        }
    }
}
