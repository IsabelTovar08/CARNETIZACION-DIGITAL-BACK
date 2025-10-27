using Business.Interfases;
using Entity.DTOs.Notifications;
using Entity.Models.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces.Notifications
{
    public interface INotificationReceivedBusiness : IBaseBusiness<NotificationReceived, NotificationReceivedDto, NotificationReceivedDto>
    {
        /// <summary>
        /// Obtiene todas las notificaciones activas de un usuario.
        /// </summary>
        Task<List<NotificationReceivedDto>> GetActiveNotificationsByUserAsync(int userId);

        /// <summary>
        /// Marca una notificación como leída.
        /// </summary>
        Task MarkAsReadAsync(int notificationReceivedId);

        /// <summary>
        /// Marca todas las notificaciones de un usuario como leídas.
        /// </summary>
        Task MarkAllAsReadAsync(int userId);
    }
}
