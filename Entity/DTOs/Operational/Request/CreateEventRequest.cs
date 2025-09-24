using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Operational.Request
{
    public class CreateEventRequest
    {
        public EventDtoRequest Event { get; set; } = default!;

        // Hijos directos
        public List<AccessPointDtoRequest> AccessPoints{ get; set; }

        // Audiencias
        public List<int>? ProfileIds { get; set; }
        public List<int>? OrganizationalUnitIds { get; set; }
        public List<int>? InternalDivisionIds { get; set; }
    }
}
