using Entity.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Operational.Response
{
    public class EventAccessPointDto : BaseDTO
    {
        public int EventId { get; set; }
        public string EventName { get; set; } = default!; 
        public int AccessPointId { get; set; }
        public string AccessPointName { get; set; } = default!; // Nombre del access point

        //public DateTime CreationDate { get; set; }
        //public DateTime? UpdateDate { get; set; }
        public string? QrCodeKey { get; set; }
    }
}
