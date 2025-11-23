using Business.Implementations.Operational;
using Business.Interfaces.Notifications;
using Business.Interfaces.Operational;
using Entity.DTOs.Notifications;
using Entity.DTOs.Notifications.Request;
using Entity.DTOs.Operational;
using Entity.DTOs.Specifics;
using Entity.Enums.Specifics;
using Entity.Models.Notifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.Controllers.Base;

namespace Web.Controllers.Operational
{
    public class NotificationsController : GenericController<Notification, NotificationDtoRequest, NotificationDto>
    {
        private readonly INotificationBusiness _business;

        public NotificationsController(INotificationBusiness business, ILogger<NotificationsController> logger)
            : base(business, logger)
        {
            _business = business;
        }

        /// <summary>
        /// Envía una notificación a un usuario específico.
        /// </summary>
        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] NotificationDtoRequest dto)
        {
            var result = await _business.CreateAndSendAsync(dto);
            return Ok(new
            {
                status = true,
                message = "Notificación enviada correctamente",
                data = result
            });
        }

        /// <summary>
        /// Obtiene las notificaciones de un usuario.
        /// </summary>
        [HttpGet("user")]
        public async Task<IActionResult> GetNotificationsByUser()
        {
            var result = await _business.GetByUserAsync();
            return Ok(new
            {
                status = true,
                message = "Listado de notificaciones",
                data = result
            });
        }


        /// <summary>
        /// Envía una notificación utilizando una plantilla predefinida.
        /// </summary>
        [HttpPost("template")]
        public async Task<ActionResult<NotificationDto>> SendTemplate([FromQuery] NotificationTemplateType type, [FromBody] object[] args)
        {
            var result = await _business.SendTemplateAsync(type, args);
            return Ok(result);
        }


        /// <summary>
        /// Obtiene la cantidad de notificaciones del usuario autenticado.
        /// </summary>
        [HttpGet("count")]
        public async Task<IActionResult> GetCountAsync()
        {

            int total = await _business.GetUserNotificationCountAsync();

            return Ok(total);
        }
    }
}
