using System.Collections.Generic;
using Entity.Models.Base;

namespace Entity.Models.Notifications
{
    public class NotificationType : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }

        // Relaciones
        public List<Notification> Notifications { get; set; }
    }
}
