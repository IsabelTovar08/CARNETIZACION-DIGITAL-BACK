using System;
using Entity.Models.Base;
using Entity.Models.ModelSecurity;

namespace Entity.Models.Organizational
{
    public class Attendance : BaseModel
    {
        public DateTime TimeOfEntry { get; set; }
        public DateTime? TimeOfExit { get; set; }

        public int? AccessPointOfEntry { get; set; }
        public int? AccessPointOfExit { get; set; }

        public AccessPoint? AccessPointEntry { get; set; }
        public AccessPoint? AccessPointExit { get; set; }

        public int PersonId { get; set; }
        public Person Person { get; set; }

        //  vínculo al evento
        public int? EventId { get; set; }
        public Event? Event { get; set; }

        // aquí vive el Base64 del PNG del QR
        public string? QrCode { get; set; }
    }
}
