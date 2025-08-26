using Entity.DTOs.Base;

namespace Entity.DTOs.Operational.Response
{
    public class EventTargetAudienceDtoResponse : BaseDtoRequest
    {
        public int TypeId { get; set; }
        public int ReferenceId { get; set; }
        public string ReferenceName { get; set; }

        public int EventId { get; set; }
        public string EventName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
