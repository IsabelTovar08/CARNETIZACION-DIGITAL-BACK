using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Classes.Base;
using Data.Interfases.Notifications;
using Entity.Context;
using Entity.Models.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Implementations.Notifications
{
    public class NotificationsReceivedData : BaseData<NotificationReceived>, INotificationsReceivedData
    {
        public NotificationsReceivedData(ApplicationDbContext context, ILogger<NotificationReceived> logger)
            : base(context, logger)
        {
        }

        /// <summary>
        /// Obtiene todas las notificaciones de un usuario.
        /// </summary>
        public async Task<IEnumerable<NotificationReceived>> GetByUserAsync(int userId)
        {
            return await _context.NotificationReceiveds
                .Include(nr => nr.Notification)
                .Where(nr => nr.UserId == userId && !nr.IsDeleted)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene todas las notificaciones activas de un usuario (no eliminadas ni expiradas).
        /// </summary>
        public async Task<IEnumerable<NotificationReceived>> GetActiveByUserAsync(int userId)
        {
            return await _context.NotificationReceiveds
                .Include(nr => nr.Notification)
                .Where(nr => nr.UserId == userId
                          && !nr.IsDeleted
                          && (nr.ExpirationDate == null || nr.ExpirationDate > DateTime.UtcNow))
                .ToListAsync();
        }
    }
}
