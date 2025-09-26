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

        public async Task SendToUserAsync(string userId, object notification)
        {
            await _hub.Clients.User(userId).SendAsync("ReceiveNotification", notification);
        }

        public async Task SendToAllAsync(object notification)
        {
            await _hub.Clients.All.SendAsync("ReceiveNotification", notification);
        }
    }
}
