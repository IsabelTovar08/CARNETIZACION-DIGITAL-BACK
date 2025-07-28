using Entity.Models.Base;
using Entity.Models.Parameter;

namespace Entity.Models.Organizational
{
    public class EventTargetAudience : BaseModel
    {
        public int TypeId { get; set; }    
        public CustomType CustomType { get; set; }
        public int ReferenceId { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }
    }
}
