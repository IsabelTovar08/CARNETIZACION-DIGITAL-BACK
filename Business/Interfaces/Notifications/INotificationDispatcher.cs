using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces.Notifications
{
    /// <summary>
    /// Contrato para enviar notificaciones en tiempo real.
    /// </summary>
    public interface INotificationDispatcher
    {
        Task SendToUserAsync(int userId, object notification);
        Task SendToAllAsync(object notification);
    }
}
