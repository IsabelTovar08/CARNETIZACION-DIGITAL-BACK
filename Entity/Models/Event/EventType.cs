using System.Collections.Generic;
using Entity.Models.Base;

namespace Entity.Models.Event
{
    public class EventType : GenericModel
    {
        public string? Description { get; set; }

        public List<Event>? Events { get; set; }
    }
}
