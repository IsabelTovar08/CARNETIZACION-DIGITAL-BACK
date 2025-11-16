using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Base;
using Entity.Enums.Specifics;

namespace Entity.DTOs.Operational.Request
{
    public class ModificationRequestDto : BaseDtoRequest
    {
        public ModificationField Field { get; set; }      // Enum
        public ModificationReason Reason { get; set; }
        public string OldValue { get; set; } = "";
        public string NewValue { get; set; } = "";
        public string? Message { get; set; }
        public int UserId { get; set; }
    }
}
