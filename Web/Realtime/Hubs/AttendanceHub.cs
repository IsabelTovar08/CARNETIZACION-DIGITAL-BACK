using Business.Interfaces.Auth;
using Microsoft.AspNetCore.SignalR;

namespace Web.Realtime.Hubs
{
    /// <summary>
    /// Envía notificaciones en tiempo real sobre asistencias.
    /// </summary>at

    public class AttendanceHub : Hub
    {
        private readonly ICurrentUser _currentUser;

        public AttendanceHub(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        public override Task OnConnectedAsync()
        {
            int userId = _currentUser.UserId;

            Console.WriteLine($"🔵 Conectado: conn={Context.ConnectionId} | user={userId}");

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"🔴 Desconectado: {Context.ConnectionId}");
            return base.OnDisconnectedAsync(exception);
        }
    }
}
