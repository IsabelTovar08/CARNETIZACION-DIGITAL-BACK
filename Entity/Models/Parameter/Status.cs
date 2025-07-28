using Entity.Models.Base;
using Entity.Models.Notifications;
using Entity.Models.Organizational;
using Entity.Models.Organizational.Assignment;

namespace Entity.Models.Parameter
{
    public class Status : GenericModel
    {
        public List<NotificationReceived> NotificatiosReceived { get; set; }
        public List<Card> cards { get; set; }
        public List<Event> Events { get; set; }
    }
}
