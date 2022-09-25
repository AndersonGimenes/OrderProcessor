using System.Threading.Tasks;

namespace OrderProcessor.Domain.Service
{
    public interface IServiceBase<T>
    {
        Task ExecuteAsync(T model);
    }
}
