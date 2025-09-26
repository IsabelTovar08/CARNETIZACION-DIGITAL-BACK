using Entity.DTOs.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace Entity.DTOs.Operational.Response
{
    public class AttendanceDtoResponse : BaseDTO
    {
        [Display(Name = "ID")]
        public int PersonId { get; set; }

        [Display(Name = "Persona")]
        public string PersonFullName { get; set; }

        [Display(Name = "Entrada")]
        public DateTime TimeOfEntry { get; set; }

        [Display(Name = "Salida")]
        public DateTime? TimeOfExit { get; set; }

        [Display(Name = "Entrada")]
        public string? TimeOfEntryStr { get; set; }

        [Display(Name = "Salida")]
        public string? TimeOfExitStr { get; set; }

        [Display(Name = "Punto Entrada (ID)")]
        public int? AccessPointOfEntry { get; set; }

        [Display(Name = "Punto Entrada")]
        public string? AccessPointOfEntryName { get; set; }

        [Display(Name = "Punto Salida (ID)")]
        public int? AccessPointOfExit { get; set; }

        [Display(Name = "Punto Salida")]
        public string? AccessPointOfExitName { get; set; }

        [Display(Name = "Evento")]
        public string? EventName { get; set; }

        [Display(Name = "Éxito")]
        public bool Success { get; set; }

        [Display(Name = "Mensaje")]
        public string? Message { get; set; }
    }
}
