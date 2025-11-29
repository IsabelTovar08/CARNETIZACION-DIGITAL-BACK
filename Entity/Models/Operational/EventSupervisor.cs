using Entity.Models.Base;
using Entity.Models.Organizational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models.Operational
{
    public class EventSupervisor : BaseModel
    {
        public int EventId { get; set; }
        public Event Event { get; set; } = default!;

        public int UserId { get; set; }
        public User User { get; set; } = default!;
    }
}
