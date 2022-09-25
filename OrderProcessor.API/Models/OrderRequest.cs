namespace OrderProcessor.API.Models
{
    public class OrderRequest
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
