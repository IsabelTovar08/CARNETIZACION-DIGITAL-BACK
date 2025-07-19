using Entity.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Event
{
    public class EventTargetAudience : GenericBaseDto
    {
        public Enum Type { get; set; }
        public  int ReferenceId { get; set; }
    }
}
