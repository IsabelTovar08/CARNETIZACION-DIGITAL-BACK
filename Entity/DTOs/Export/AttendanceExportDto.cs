using System.ComponentModel.DataAnnotations;

namespace Entity.DTOs.Operational.Export
{
    public class AttendanceExportDto
    {
        [Display(Name = "Persona")]
        public string Person { get; set; }

        [Display(Name = "Evento")]
        public string Event { get; set; }

        [Display(Name = "Entrada")]
        public string Entry { get; set; }

        [Display(Name = "Salida")]
        public string Exit { get; set; }

        [Display(Name = "Punto Entrada")]
        public string EntryPoint { get; set; }

        [Display(Name = "Punto Salida")]
        public string ExitPoint { get; set; }
    }
}
