using OrderProcessor.Domain.Entity;

namespace OrderProcessor.Domain.Service
{
    public interface IOrderService : IServiceBase<Order>
    {
        void Publish(Order order);
    }
}
