using Entity.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Operational
{
    public class EventTypeDto : GenericDto
    {
        public string? Description { get; set; }
    }
}
