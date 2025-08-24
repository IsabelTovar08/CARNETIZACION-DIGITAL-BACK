using Business.Interfaces.Notifications;
using Entity.DTOs.Notifications;
using Entity.Models.Notifications;
using Microsoft.Extensions.Logging;
using Web.Controllers.Base;

namespace Web.Controllers.Notifications
{
    public class NotificationReceivedController : GenericController<NotificationReceived, NotificationReceivedDto, NotificationReceivedDto>
    {
        private readonly INotificationReceivedBusiness _business;

        public NotificationReceivedController(INotificationReceivedBusiness business, ILogger<NotificationReceivedController> logger)
            : base(business, logger)
        {
            _business = business;
        }

        // Aquí puedes agregar métodos específicos para NotificationReceived si los necesitas
    }
}
