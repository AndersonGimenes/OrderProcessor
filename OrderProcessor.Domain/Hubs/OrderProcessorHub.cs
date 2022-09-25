using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace OrderProcessor.Domain.Hubs
{
    public class OrderProcessorHub : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ShowMessage", message);
        }
    }
}
