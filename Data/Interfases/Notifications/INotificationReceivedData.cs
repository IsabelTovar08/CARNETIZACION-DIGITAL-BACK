using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfases;
using Entity.Models.Notifications;

namespace Data.Interfases.Notifications
{
    public interface INotificationsReceivedData : ICrudBase<NotificationReceived>
    {
        /// <summary>
        /// Obtiene todas las notificaciones de un usuario.
        /// </summary>
        Task<IEnumerable<NotificationReceived>> GetByUserAsync(int userId);

        /// <summary>
        /// Obtiene todas las notificaciones activas de un usuario (no eliminadas ni expiradas).
        /// </summary>
        Task<IEnumerable<NotificationReceived>> GetActiveByUserAsync(int userId);
    }
}
