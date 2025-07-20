using Entity.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Event
{
    public class EventTypeDto : GenericBaseDto
    {
        public string? Description { get; set; }
    }
}
