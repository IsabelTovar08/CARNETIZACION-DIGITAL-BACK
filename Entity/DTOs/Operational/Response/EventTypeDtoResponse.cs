using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Base;

namespace Entity.DTOs.Operational.Response
{
    public class EventTypeDtoResponse : GenericDto
    {
        public string? Description { get; set; }

    }
}
