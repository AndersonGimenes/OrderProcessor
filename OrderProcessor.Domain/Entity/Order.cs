using OrderProcessor.Domain.Enum;
using System;

namespace OrderProcessor.Domain.Entity
{
    public class Order
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public State State { get; set; }
    }
}
