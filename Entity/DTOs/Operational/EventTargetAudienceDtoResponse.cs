using Entity.DTOs.Base;

namespace Entity.DTOs.Operational
{
    public class EventTargetAudienceDtoResponse : BaseDTO
    {
        public int TypeId { get; set; }
        public int ReferenceId { get; set; }
        public int EventId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
