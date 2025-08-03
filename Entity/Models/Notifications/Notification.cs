using System;
using System.Collections.Generic;
using Entity.Models.Base;
using Entity.Models.Parameter;

namespace Entity.Models.Notifications
{
    public class Notification : BaseModel
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime? CreateDate { get; set; }

        // Relaciones
        public int NotificationTypeId { get; set; }
        public CustomType NotificationType { get; set; }
        public List<NotificationReceived> NotificationReceiveds { get; set; }
    }
}
