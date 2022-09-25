using Microsoft.Extensions.DependencyInjection;
using OrderProcessor.Domain.Builder;
using OrderProcessor.Domain.Service;

namespace OrderProcessor.Domain.IoC
{
    public static class DependencyInjectionExtensions
    {
        public static void DomainDependencyInjection(this IServiceCollection service)
        {
            service.AddScoped<IOrderBuilder, OrderBuilder>();
            service.AddScoped<IOrderService, OrderService>();
        }
    }
}
