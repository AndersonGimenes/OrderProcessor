using System.Threading.Tasks;

namespace OrderProcessor.Worker.Handlers
{
    internal interface IOrderHandler
    {
        Task ExecuteAsync();
    }
}
