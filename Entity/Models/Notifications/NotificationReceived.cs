using System;
using Entity.Models.Base;
using Entity.Models.Parameter;

namespace Entity.Models.Notifications
{
    public class NotificationReceived : BaseModel
    {
        public int StatusId { get; set; }
        public DateTime? SendDate { get; set; }
        public DateTime? ReadDate { get; set; }
        public DateTime? ExpirationDate { get; set; }

        // Relaciones
        public int NotificationId { get; set; }
        public Notification Notification { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public Status Status { get; set; }
    }
}
