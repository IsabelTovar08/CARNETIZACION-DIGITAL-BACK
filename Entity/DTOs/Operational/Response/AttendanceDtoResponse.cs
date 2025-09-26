using Entity.DTOs.Base;
using System;

namespace Entity.DTOs.Operational.Response
{
    public class AttendanceDtoResponse : BaseDTO
    {
        public int PersonId { get; set; }
        public string PersonFullName { get; set; }

        public DateTime TimeOfEntry { get; set; }
        public DateTime? TimeOfExit { get; set; }

        public string? TimeOfEntryStr { get; set; }
        public string? TimeOfExitStr { get; set; }

        public int? AccessPointOfEntry { get; set; }
        public string? AccessPointOfEntryName { get; set; }

        public int? AccessPointOfExit { get; set; }
        public string? AccessPointOfExitName { get; set; }

        public int? EventId { get; set; }

        public string? EventName { get; set; }

        /// <summary>
        /// Resultado de la operación
        /// </summary>
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}
