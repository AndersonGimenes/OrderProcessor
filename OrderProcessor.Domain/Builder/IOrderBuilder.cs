using OrderProcessor.Domain.Entity;

namespace OrderProcessor.Domain.Builder
{
    public interface IOrderBuilder
    {
        IOrderBuilder Reset();
        IOrderBuilder AddProductName(string name);
        IOrderBuilder AddPrice(decimal price);
        IOrderBuilder AddQuantity(int quantity);
        Order Build();
    }
}