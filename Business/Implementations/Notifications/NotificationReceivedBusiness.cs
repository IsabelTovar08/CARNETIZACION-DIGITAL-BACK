using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Notifications;
using Data.Interfases.Notifications;
using Entity.DTOs.Notifications;
using Entity.Models.Notifications;
using Microsoft.Extensions.Logging;
using Utilities.Enums.Specifics;

namespace Business.Implementations.Notifications
{
    /// <summary>
    /// Lógica de negocio para notificaciones recibidas (lectura y estados).
    /// </summary>
    public class NotificationReceivedBusiness: BaseBusiness<NotificationReceived, NotificationReceivedDto, NotificationReceivedDto>, INotificationReceivedBusiness
    {
        private readonly INotificationsReceivedData _notificationReceivedData;

        public NotificationReceivedBusiness(
            INotificationsReceivedData notificationReceivedData,
            ILogger<NotificationReceived> logger,
            IMapper mapper
        ) : base(notificationReceivedData, logger, mapper)
        {
            _notificationReceivedData = notificationReceivedData;
        }

        /// <summary>
        /// Obtiene todas las notificaciones activas de un usuario.
        /// </summary>
        public async Task<List<NotificationReceivedDto>> GetActiveNotificationsByUserAsync(int userId)
        {
            IEnumerable<NotificationReceived> list = await _notificationReceivedData.GetActiveByUserAsync(userId);
            return _mapper.Map<List<NotificationReceivedDto>>(list);
        }

        /// <summary>
        /// Marca una notificación como leída.
        /// </summary>
        public async Task MarkAsReadAsync(int notificationReceivedId)
        {
            NotificationReceived? entity = await _notificationReceivedData.GetByIdAsync(notificationReceivedId);
            if (entity != null && !entity.IsDeleted && entity.StatusId != NotificationStatus.Read)
            {
                entity.ReadDate = System.DateTime.UtcNow;
                entity.StatusId = NotificationStatus.Read;
                await _notificationReceivedData.UpdateAsync(entity);
            }
        }

        /// <summary>
        /// Marca todas las notificaciones de un usuario como leídas.
        /// </summary>
        public async Task MarkAllAsReadAsync(int userId)
        {
            IEnumerable<NotificationReceived> list = await _notificationReceivedData.GetByUserAsync(userId);

            foreach (var entity in list)
            {
                if (entity.StatusId != NotificationStatus.Read)
                {
                    entity.ReadDate = System.DateTime.UtcNow;
                    entity.StatusId = NotificationStatus.Read;
                    await _notificationReceivedData.UpdateAsync(entity);
                }
            }
        }
    }
}
