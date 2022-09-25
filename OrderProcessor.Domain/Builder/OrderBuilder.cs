using OrderProcessor.Domain.Entity;
using OrderProcessor.Domain.Enum;
using System;

namespace OrderProcessor.Domain.Builder
{
    internal class OrderBuilder : IOrderBuilder
    {
        private Order _order;

        public IOrderBuilder Reset()
        {
            _order = new()
            {
                State = State.InProgress, 
                Id = Guid.NewGuid()
            };

            return this;
        }

        public IOrderBuilder AddProductName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Invalid Product Name");

            _order.ProductName = name;

            return this;
        }

        public IOrderBuilder AddPrice(decimal price)
        {
            if (price <= 0)
                throw new ArgumentException("Invalid Price");

            _order.Price = price;
            return this;
        }

        public IOrderBuilder AddQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Invalid Quantity");

            _order.Quantity = quantity;
            return this;
        }

        public Order Build() => _order;
    }
}
