using Entity.DTOs.Base;
using Entity.Models.Base;
using System;

namespace Entity.DTOs.Operational
{
    public class AttendanceDto : BaseModel
    {
        public int PersonId { get; set; }
        public string PersonFullName { get; set; }

        public DateTime TimeOfEntry { get; set; }
        public DateTime? TimeOfExit { get; set; }   // nullable

        // ahora nullable
        public int? AccessPointOfEntry { get; set; }
        public string? AccessPointOfEntryName { get; set; }

        public int? AccessPointOfExit { get; set; }
        public string? AccessPointOfExitName { get; set; }


        public int EventId { get; set; }
        public string? EventName { get; set; }

        // QR Base64
        public string? QrCode { get; set; }
    }
}
