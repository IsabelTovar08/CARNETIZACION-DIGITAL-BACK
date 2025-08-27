using Entity.DTOs.Base;
using Entity.Models.Base;
using System;

namespace Entity.DTOs.Operational.Response
{
    public class AttendanceDtoResponse : BaseDTO
    {
        public int PersonId { get; set; }
        public string PersonFullName { get; set; }

        public DateTime TimeOfEntry { get; set; }
        public DateTime? TimeOfExit { get; set; }   // nullable

        // ➕ Formateados (para Swagger / UI)
        // Ejemplo: "26/08/2025 14:35" con cultura es-CO
        public string? TimeOfEntryStr { get; set; }
        public string? TimeOfExitStr { get; set; }

        // ahora nullable
        public int? AccessPointOfEntry { get; set; }
        public string? AccessPointOfEntryName { get; set; }

        public int? AccessPointOfExit { get; set; }
        public string? AccessPointOfExitName { get; set; }

        public string? EventName { get; set; }
    }
}
