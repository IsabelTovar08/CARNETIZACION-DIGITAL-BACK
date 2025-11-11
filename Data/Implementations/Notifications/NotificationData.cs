using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Classes.Base;
using Data.Interfases.Notifications;
using Data.Interfases.Operational;
using Entity.Context;
using Entity.DTOs.Specifics;
using Entity.Enums.Extensions;
using Entity.Models.Notifications;
using Entity.Models.Organizational;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Utilities.Enums.Specifics;

namespace Data.Implementations.Notifications
{
    public class NotificationData : BaseData<Notification>, INotificationData
    {
        public NotificationData(ApplicationDbContext context, ILogger<Notification> logger)
            : base(context, logger)
        {
        }

        /// <summary>
        /// Obtiene todas las notificaciones enviadas a un usuario.
        /// </summary>
        public async Task<IEnumerable<NotificationWithReceivedDto>> GetNotificationsByUserAsync(int userId)
        {
            return await _context.NotificationReceiveds
                .Include(nr => nr.Notification)
                .Where(nr => nr.UserId == userId && !nr.IsDeleted)
                .Select(nr => new NotificationWithReceivedDto
                {
                    NotificationId = nr.Notification.Id,
                    Title = nr.Notification.Title,
                    Message = nr.Notification.Message,
                    NotificationTypeName = ((NotificationType)nr.Notification.NotificationType).GetDisplayName(),
                    StatusId = nr.Status,
                    SendDate = nr.SendDate,
                    ReadDate = nr.ReadDate
                })
                .ToListAsync();

        }
    }
}

