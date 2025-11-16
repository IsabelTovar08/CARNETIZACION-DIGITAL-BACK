using Business.Implementations.Operational;
using Business.Interfaces.Notifications;
using Entity.DTOs.Notifications;
using Entity.Models.Notifications;
using Microsoft.AspNetCore.Mvc;
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
        /// <summary>
        /// Marca una notificación como leída.
        /// </summary>
        [HttpPut("mark-as-read/{id:int}")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            await _business.MarkAsReadAsync(id);
            return Ok(new { message = "Notificación marcada como leída" });
        }


    }
}
