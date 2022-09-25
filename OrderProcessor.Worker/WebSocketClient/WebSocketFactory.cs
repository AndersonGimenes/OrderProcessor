using Microsoft.AspNetCore.SignalR.Client;
using OrderProcessor.Worker.Options;
using System.Threading.Tasks;

namespace OrderProcessor.Worker.WebSocketClient
{
    internal static class WebSocketFactory
    {
        public static async Task HubClientSend(string method, string message, WebSocketOptions options)
        {
            await using var connection = new HubConnectionBuilder()
                .WithUrl(options.Url)
                .Build();

            await connection.StartAsync();
            await connection.SendAsync(method, message);
        }
    }
}
