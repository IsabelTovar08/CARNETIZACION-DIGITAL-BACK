using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Base;
using Entity.Enums.Specifics;

namespace Entity.DTOs.Operational.Response
{
    public class ModificationRequestResponseDto : BaseDTO
    {
        public int Id { get; set; }
        public DateTime RequestDate { get; set; }
        public ModificationRequestStatus Status { get; set; }
        public string? StatusName { get; set; }


        // Datos del usuario solicitante
        public int UserId { get; set; }
        public string UserName { get; set; } = "";
        public string UserEmail { get; set; } = "";
        public string UserIdentification { get; set; } = "";

        // Detalle de la solicitud
        public int FieldId { get; set; }
        public string FieldName { get; set; } = "";
        public int ReasonId { get; set; }
        public string ReasonName { get; set; } = "";
        public string OldValue { get; set; } = string.Empty;
        public string? OldValueName { get; set; } 

        public string NewValue { get; set; } = string.Empty;
        public string? NewValueName { get; set; } 
        public string? Message { get; set; }
        public int? UpdatedById { get; set; }
    }
}
