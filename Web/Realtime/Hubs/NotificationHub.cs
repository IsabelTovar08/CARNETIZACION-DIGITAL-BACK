using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;

namespace Web.Realtime.Hubs
{
    public class NotificationHub : Hub
    {
        private static readonly ConcurrentDictionary<int, string> _connections
                = new ConcurrentDictionary<int, string>();

        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"🔌 Cliente conectado: {Context.ConnectionId}");
            return base.OnConnectedAsync();
        }

        public Task RegisterConnection(int userId)
        {
            _connections[userId] = Context.ConnectionId;
            Console.WriteLine($"🔗 Usuario {userId} registrado con conexión {Context.ConnectionId}");
            return Task.CompletedTask;
        }

        public static string? GetConnection(int userId)
            => _connections.TryGetValue(userId, out var conn) ? conn : null;
    }
}
