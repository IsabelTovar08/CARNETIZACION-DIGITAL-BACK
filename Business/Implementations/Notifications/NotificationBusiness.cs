using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Classes.Base;
using Business.Implementations.Notifications;
using Business.Interfaces.Auth;
using Business.Interfaces.Notifications;
using Business.Interfaces.Operational;
using Business.Services.CodeGenerator;
using Data.Interfases;
using Data.Interfases.Notifications;
using Data.Interfases.Operational;
using Entity.DTOs.Notifications;
using Entity.DTOs.Notifications.Request;
using Entity.DTOs.Operational;
using Entity.DTOs.Specifics;
using Entity.Models.Notifications;
using Microsoft.Extensions.Logging;
using Utilities.Enums.Specifics;

namespace Business.Implementations.Operational
{
    public class NotificationsBusiness : BaseBusiness<Notification, NotificationDtoRequest, NotificationDto>, INotificationBusiness
    {
        private readonly INotificationData _notificationData;
        private readonly INotificationReceivedBusiness _notificationReceivedBusiness;
        private readonly INotificationDispatcher _dispatcher;
        private readonly ICurrentUser _currentUser;

        public NotificationsBusiness(INotificationData data, ILogger<Notification> logger, IMapper mapper,
             INotificationData notificationData,
            INotificationReceivedBusiness notificationReceivedBusiness,
              INotificationDispatcher dispatcher,
              ICurrentUser currentUser
            ) : base(data, logger, mapper)
        {
            _notificationData = notificationData;
            _notificationReceivedBusiness = notificationReceivedBusiness;
            _dispatcher = dispatcher;
            _currentUser = currentUser;

        }

        /// <summary>
        /// Crea y envía una notificación a un usuario.
        /// </summary>
        public async Task<NotificationDto> CreateAndSendAsync(NotificationDtoRequest dto)
        {
            int userId = _currentUser.UserId;

            dto.NotificationTypeId = (int)NotificationType.Warning;
            // Guardar notificación
            NotificationDto notification = await Save(dto);

            // Crear registro de recepción
            var received = new NotificationReceivedDto
            {
                NotificationId = notification.Id,
                UserId = userId,
                StatusId = (int)NotificationStatus.Pending,
                SendDate = DateTime.UtcNow
            };
            await _notificationReceivedBusiness.Save(received);

            // Enviar por SignalR
            await _dispatcher.SendToUserAsync(userId.ToString(), new
            {
                notification.Id,
                notification.Title,
                notification.Message,
                notification.NotificationTypeName,
                received.SendDate
            });

            return _mapper.Map<NotificationDto>(notification);
        }

        /// <summary>
        /// Obtiene todas las notificaciones de un usuario.
        /// </summary>
        public async Task<IEnumerable<NotificationWithReceivedDto>> GetByUserAsync()
        {
            int userId = _currentUser.UserId;
            var notifications = await _notificationData.GetNotificationsByUserAsync(userId);
            return _mapper.Map<IEnumerable<NotificationWithReceivedDto>>(notifications);
        }
    }
}
