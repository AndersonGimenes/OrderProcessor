namespace OrderProcessor.Domain.Options
{
    public class RabbitMqOptions
    {
        public string HostName { get; set; }
        public string RoutingKey { get; set; }
        public string Queue { get; set; }
    }
}
