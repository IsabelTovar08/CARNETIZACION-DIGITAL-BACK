using Entity.Models.Base;
using Entity.Models.Organizational.Assignment;
using Entity.Models.Organizational.Structure;
using Entity.Models.Parameter;

namespace Entity.Models.Organizational
{
    public class EventTargetAudience : BaseModel
    {
        public int TypeId { get; set; }    
        public CustomType CustomType { get; set; }
        public int ReferenceId { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; } = default!;

        public int? ProfileId { get; set; }
        public Profiles? Profile { get; set; }

        public int? OrganizationalUnitId { get; set; }
        public OrganizationalUnit? OrganizationalUnit { get; set; }

        public int? InternalDivisionId { get; set; }
        public InternalDivision? InternalDivision { get; set; }
    }
}
