using Entity.Models.Base;

namespace Entity.Models.Event
{
    public class EventTargetAudience : BaseModel
    {
        public string Type { get; set; }
        public int ReferenceId { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }
    }
}
