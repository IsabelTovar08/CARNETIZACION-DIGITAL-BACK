using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfases;
using Entity.DTOs.Specifics;
using Entity.Models.Notifications;

namespace Data.Interfases.Notifications
{
    public interface INotificationData : ICrudBase<Notification>
    {
        Task<IEnumerable<NotificationWithReceivedDto>> GetNotificationsByUserAsync(int userId);
    }
}