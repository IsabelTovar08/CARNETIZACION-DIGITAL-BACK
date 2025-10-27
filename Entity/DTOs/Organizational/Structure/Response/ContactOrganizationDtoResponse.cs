using Entity.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Organizational.Structure.Response
{
    public class ContactOrganizationDtoResponse : BaseDTO
    {
        // Datos de la empresa
        public string CompanyName { get; set; } = string.Empty;
        public string? Message { get; set; }
        public string? CompanyEmail { get; set; }

        //Datos del asesor
        public string AdvisorName { get; set; } = string.Empty;
        public string? AdvisorRole { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        //Datos asociados a la persona
        public int? PersonId { get; set; }
        public string? DocumentNumber { get; set; }
        public string? DocumentTypeName { get; set; }
        public string? BloodTypeName { get; set; }

        //Ubicación
        public string? Address { get; set; }
        public string? CityName { get; set; }

        //Estado de la solicitud
        public bool IsApproved { get; set; }
        public DateTime RequestDate { get; set; }

        //Información extra (solo lectura)
        //public bool IsDeleted { get; set; }
        public string StatusDescription => IsApproved ? "Aprobada" : "Pendiente de aprobación";
    }
}
