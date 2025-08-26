using System.Collections.Generic;
using Entity.Models.Base;
using Entity.Models.Organizational;
using Entity.Models.Parameter;

namespace Entity.Models.Organizational
{
    public class AccessPoint : BaseModel
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }

        // FKs
        public int EventId { get; set; }
        public int TypeId { get; set; }

        // QR en Base64 (png)
        public string? QrCode { get; set; }

        // Navegaciones: NULLABLE y SIN inicializar (para que EF NO inserte Event/CustomType en blanco)
        public Event? Event { get; set; }
        public CustomType? AccessPointType { get; set; }

        // Si AttendanceConfiguration usa WithMany(ap => ap.AttendancesEntry/Exit)
        public ICollection<Attendance> AttendancesEntry { get; set; } = new List<Attendance>();
        public ICollection<Attendance> AttendancesExit { get; set; } = new List<Attendance>();
    }
}
