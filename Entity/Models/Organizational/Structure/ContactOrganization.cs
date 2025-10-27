using Entity.Models.Base;
using Entity.Models.ModelSecurity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models.Organizational.Structure
{
    public class ContactOrganization : BaseModel
    {
        public string AdvisorName { get; set; } = default!;   // Nombre del asesor interesado
        public string CompanyName { get; set; } = default!;   // Nombre de la empresa
        public string CompanyEmail { get; set; }              // Cprreo electronico de la empresa de la empresa
        public string Email { get; set; } = default!;         // Correo electrónico 
        public string PhoneNumber { get; set; } = default!;   // Número telefónico
        public string? AdvisorRole { get; set; }              // Cargo del asesor
        public string? Message { get; set; }                  // Comentarios

        public int? PersonId { get; set; }
        public Person Person { get; set; }

        public bool IsApproved { get; set; } = false;         // Estado de aprobación
        public DateTime RequestDate { get; set; } = DateTime.UtcNow; // Fecha de creación
    }
}
