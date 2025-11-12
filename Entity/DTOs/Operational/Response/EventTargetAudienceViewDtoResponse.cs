using Entity.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Operational.Response
{
    public class EventTargetAudienceViewDtoResponse : BaseDtoRequest
    {
        public int TypeId { get; set; }
        public int ReferenceId { get; set; }
        public string? ReferenceName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
