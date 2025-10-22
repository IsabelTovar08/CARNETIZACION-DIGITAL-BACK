using System;
using System.Collections.Generic;
using Entity.Models.Base;
using Entity.Models.Parameter;
using Utilities.Enums.Specifics;

namespace Entity.Models.Notifications
{
    public class Notification : BaseModel
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// URL o ruta a donde se redirigirá el usuario al abrir la notificación.
        /// </summary>
        public string? RedirectUrl { get; set; }

        // Relaciones
        public NotificationType NotificationType { get; set; }
        public List<NotificationReceived> NotificationReceiveds { get; set; }
    }
}
