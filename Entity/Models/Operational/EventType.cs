using System.Collections.Generic;
using Entity.Models.Base;

namespace Entity.Models.Organizational
{
    public class EventType : GenericModel
    {
        public string? Description { get; set; }

        public List<Event>? Events { get; set; }
    }
}
