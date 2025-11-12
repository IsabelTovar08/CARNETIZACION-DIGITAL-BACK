using Entity.DTOs.Base;
using Entity.DTOs.Operational.Response;
using Entity.DTOs.Organizational.Structure.Response;
using Entity.Models.Organizational.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Operational
{
    public  class EventDtoResponse : GenericDto
    {
        public string Code { get; set; }
        public string? Description { get; set; }

        public DateTime? EventStart { get; set; }
        public DateTime? EventEnd{ get; set; }       
        public int? ScheduleId { get; set; }

        public ScheduleDto? Schedule { get; set; }

        public int EventTypeId { get; set; }
        public string? EventTypeName { get; set; }

        public int StatusId { get; set; }
        public string? StatusName { get; set; }

        public bool Ispublic { get; set; }
        public string? QrCodeBase64 { get; set; }

        public List<AccessPointDtoResponsee> AccessPoints { get; set; } = new();
    }
}
