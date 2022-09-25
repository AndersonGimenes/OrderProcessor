using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderProcessor.Domain.Options;
using OrderProcessor.Domain.Service;
using OrderProcessor.Worker.Handlers;
using OrderProcessor.Worker.Options;

namespace OrderProcessor.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();

                    services.AddSingleton<IOrderHandler, OrderHandler>();
                    services.AddSingleton<IOrderService, OrderService>();
                    
                    services.Configure<RabbitMqOptions>(hostContext.Configuration.GetSection("HabbitMqOptions"));
                    services.Configure<WebSocketOptions>(hostContext.Configuration.GetSection("WebSocketOptions"));
                });
        }
    }
}
