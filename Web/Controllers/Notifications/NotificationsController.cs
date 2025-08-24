using Business.Interfaces.Notifications;
using Business.Interfaces.Operational;
using Entity.DTOs.Notifications;
using Entity.DTOs.Operational;
using Entity.Models.Notifications;
using Microsoft.Extensions.Logging;
using Web.Controllers.Base;

namespace Web.Controllers.Operational
{
    public class NotificationsController : GenericController<Notification, NotificationDto, NotificationDto>
    {
        private readonly INotificationBusiness _business;

        public NotificationsController(INotificationBusiness business, ILogger<NotificationsController> logger)
            : base(business, logger)
        {
            _business = business;
        }

        // Aquí puedes agregar métodos específicos para Notifications si los necesitas
    }
}
