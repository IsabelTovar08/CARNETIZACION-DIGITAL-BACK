using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Operational.Response
{
    public class EventTypeCountDtoResponse
    {
        public int EventTypeId { get; set; }
        public string EventTypeName { get; set; } = default!;
        public int TotalEvents { get; set; }
    }
}
