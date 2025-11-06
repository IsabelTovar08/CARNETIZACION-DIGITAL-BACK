using Business.Classes.Base;
using Business.Interfases;
using Entity.DTOs.Notifications;
using Entity.DTOs.Notifications.Request;
using Entity.DTOs.Operational;
using Entity.DTOs.Specifics;
using Entity.Enums.Specifics;
using Entity.Models.Notifications;

namespace Business.Interfaces.Notifications
{
    public interface INotificationBusiness : IBaseBusiness<Notification, NotificationDtoRequest, NotificationDto>
    {
        Task<NotificationDto> CreateAndSendAsync(NotificationDtoRequest dto);
        Task<IEnumerable<NotificationWithReceivedDto>> GetByUserAsync();

        /// <summary>
        /// Envía una notificación utilizando una plantilla predefinida.
        /// </summary>
        Task<NotificationDto> SendTemplateAsync(NotificationTemplateType type, params object[] args);
    }
}
