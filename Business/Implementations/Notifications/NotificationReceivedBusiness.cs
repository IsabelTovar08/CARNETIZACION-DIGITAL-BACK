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

namespace Business.Implementations.Notifications
{
    public class NotificationReceivedBusiness : BaseBusiness<NotificationReceived, NotificationReceivedDto, NotificationReceivedDto>, INotificationReceivedBusiness
    {
        protected readonly INotificationsReceivedData _notificationReceivedData;

        public NotificationReceivedBusiness(INotificationsReceivedData notificationReceivedData, ILogger<NotificationReceived> logger, IMapper mapper)
            : base(notificationReceivedData, logger, mapper)
        {
            _notificationReceivedData = notificationReceivedData;
        }

        public async Task<List<NotificationReceivedDto>> GetActiveNotificationsByUserAsync(int userId)
        {
            var list = await _notificationReceivedData.GetAllAsync();
            var filtered = list.Where(n => n.UserId == userId && !n.IsDeleted && (n.ExpirationDate == null || n.ExpirationDate > DateTime.Now)).ToList();
            return _mapper.Map<List<NotificationReceivedDto>>(filtered);
        }

        public async Task MarkAsReadAsync(int notificationReceivedId)
        {
            var entity = await _notificationReceivedData.GetByIdAsync(notificationReceivedId);
            if (entity != null && !entity.IsDeleted)
            {
                entity.ReadDate = DateTime.Now;
                entity.StatusId = 2;
                await _notificationReceivedData.UpdateAsync(entity);
            }
        }
    }
}
