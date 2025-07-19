using Entity.Models.Base;

namespace Entity.Models.Event
{
    public class EventTargetAudience : GenericModel
    {
        public string Type { get; set; }
        public string ReferenceId { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }
    }
}
