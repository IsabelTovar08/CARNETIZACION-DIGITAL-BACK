using Business.Interfaces.Notifications;
using Microsoft.AspNetCore.SignalR;
using Web.Realtime.Hubs;

namespace Web.Realtime.Dispatchers
{
    /// <summary>
    /// Implementación del dispatcher usando SignalR.
    /// </summary>
    public class SignalRNotificationDispatcher : INotificationDispatcher
    {
        private readonly IHubContext<NotificationHub> _hub;

        public SignalRNotificationDispatcher(IHubContext<NotificationHub> hub)
        {
            _hub = hub;
        }

        public async Task SendToUserAsync(int userId, object notification)
        {
            var conn = NotificationHub.GetConnection(userId);

            if (string.IsNullOrEmpty(conn))
                return;

            await _hub.Clients.Client(conn)
                .SendAsync("ReceiveNotification", notification);
        }


        public async Task SendToAllAsync(object notification)
        {
            await _hub.Clients.All.SendAsync("ReceiveNotification", notification);
        }
    }
}
