using Entity.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Event
{
    public class AccessPoint : GenericBaseDto
    {
        public string Type { get; set; }
        public string? Description { get; set; }

        public int EventId { get; set; }
        public string? EventName { get; set; }

    }
}
