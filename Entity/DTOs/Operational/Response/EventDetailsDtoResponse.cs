using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Operational.Response
{
    public class EventDetailsDtoResponse : EventDtoResponse
    {
        public List<AccessPointDtoResponsee>? AccessPoints { get; set; }
        public List<EventTargetAudienceViewDtoResponse> Audiences { get; set; } = new();
    }
}
