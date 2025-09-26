using Microsoft.AspNetCore.SignalR;

namespace Web.Realtime.Hubs
{
    public class NotificationHub : Hub
    {
        // Enviar a un usuario específico
        public async Task SendNotificationToUser(string userId, object notification)
        {
            await Clients.User(userId).SendAsync("ReceiveNotification", notification);
        }

        // Enviar a todos
        public async Task SendNotificationToAll(object notification)
        {
            await Clients.All.SendAsync("ReceiveNotification", notification);
        }
    }
}
