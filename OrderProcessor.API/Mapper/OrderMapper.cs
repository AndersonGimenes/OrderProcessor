using OrderProcessor.API.Models;
using OrderProcessor.Domain.Builder;
using OrderProcessor.Domain.Entity;

namespace OrderProcessor.API.Mapper
{
    public static class OrderMapper
    {
        public static Order OrderMapperRequest(OrderRequest request, IOrderBuilder builder)
        {
            return builder
                        .Reset()
                        .AddProductName(request.ProductName)
                        .AddPrice(request.Price)
                        .AddQuantity(request.Quantity)
                        .Build();
        }
    }
}
