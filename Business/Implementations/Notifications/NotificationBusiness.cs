using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Business.Classes.Base;
using Business.Implementations.Notifications;
using Business.Interfaces.Auth;
using Business.Interfaces.Notifications;
using Business.Services.Notifications;
using Data.Interfases;
using Data.Interfases.Notifications;
using Data.Interfases.Transaction;
using Entity.DTOs.Notifications;
using Entity.DTOs.Notifications.Request;
using Entity.DTOs.Specifics;
using Entity.Enums.Specifics;
using Entity.Models.Notifications;
using Microsoft.Extensions.Logging;
using Utilities.Enums.Specifics;

namespace Business.Implementations.Operational
{
    /// <summary>
    /// Lógica de negocio para la gestión de notificaciones.
    /// </summary>
    public class NotificationsBusiness
        : BaseBusiness<Notification, NotificationDtoRequest, NotificationDto>, INotificationBusiness
    {
        private readonly INotificationData _notificationData;
        private readonly INotificationReceivedBusiness _notificationReceivedBusiness;
        private readonly INotificationDispatcher _dispatcher;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;

        public NotificationsBusiness(
            INotificationData data,
            ILogger<Notification> logger,
            IMapper mapper,
            INotificationData notificationData,
            INotificationReceivedBusiness notificationReceivedBusiness,
            INotificationDispatcher dispatcher,
            ICurrentUser currentUser,
            IUnitOfWork unitOfWork
        ) : base(data, logger, mapper)
        {
            _notificationData = notificationData;
            _notificationReceivedBusiness = notificationReceivedBusiness;
            _dispatcher = dispatcher;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Crea y envía una notificación a un usuario a partir de un DTO.
        /// Siempre genera un registro en NotificationReceived con estado "Enviado".
        /// </summary>
        public async Task<NotificationDto> CreateAndSendAsync(NotificationDtoRequest dto)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                int userId = _currentUser.UserId;

                // Guardar notificación
                NotificationDto notification = await Save(dto);

                // Registrar recepción en estado "Enviado"
                var received = new NotificationReceivedDto
                {
                    NotificationId = notification.Id,
                    UserId = userId,
                    Status = NotificationStatus.Sent,
                    SendDate = DateTime.UtcNow
                };
                await _notificationReceivedBusiness.Save(received);

                await _unitOfWork.CommitAsync();

                // Enviar al usuario vía SignalR (u otro dispatcher)
                await _dispatcher.SendToUserAsync(userId, new
                {
                    notification.Id,
                    notification.Title,
                    notification.Message,
                    notification.NotificationTypeName,
                    received.SendDate
                });

                return notification;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new Exception($"Error al crear y enviar notificación: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Envía una notificación utilizando una plantilla predefinida.
        /// </summary>
        public async Task<NotificationDto> SendTemplateAsync(NotificationTemplateType type, params object[] args)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var dto = NotificationFactory.Create(type, args);
                var notification = await CreateAndSendAsync(dto);
                await _unitOfWork.CommitAsync();
                return notification;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new Exception($"Error al enviar notificación mediante plantilla: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtiene todas las notificaciones del usuario actual (con estado incluido).
        /// </summary>
        public async Task<IEnumerable<NotificationWithReceivedDto>> GetByUserAsync()
        {
            int userId = _currentUser.UserId;
            var notifications = await _notificationData.GetNotificationsByUserAsync(userId);
            return _mapper.Map<IEnumerable<NotificationWithReceivedDto>>(notifications);
        }
    }
}
