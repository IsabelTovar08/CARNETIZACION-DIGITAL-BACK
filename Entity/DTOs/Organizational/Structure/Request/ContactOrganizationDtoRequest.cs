using Entity.DTOs.Base;
using Entity.DTOs.ModelSecurity.Request;
using Entity.Models.Organizational.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Organizational.Structure.Request
{
    public class ContactOrganizationDtoRequest : BaseDtoRequest
    {
        [Required(ErrorMessage = "El nombre de la empresa es obligatorio.")]
        [StringLength(200, ErrorMessage = "El nombre de la empresa no puede exceder los 200 caracteres.")]
        public string CompanyName { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "El mensaje no puede exceder los 500 caracteres.")]
        public string? Message { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
        [StringLength(200, ErrorMessage = "El correo electrónico no puede exceder los 200 caracteres.")]
        public string CompanyEmail { get; set; }

        // 🔹 Datos del asesor / persona de contacto
        [Required(ErrorMessage = "El nombre del asesor es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre del asesor no puede exceder los 100 caracteres.")]
        public string AdvisorName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido del asesor es obligatorio.")]
        [StringLength(100, ErrorMessage = "El apellido del asesor no puede exceder los 100 caracteres.")]
        public string AdvisorLastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
        [StringLength(200, ErrorMessage = "El correo electrónico no puede exceder los 200 caracteres.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono de contacto es obligatorio.")]
        [RegularExpression(@"(^$|^[0-9\+]+$)", ErrorMessage = "El teléfono solo puede contener números y el signo +.")]
        [StringLength(20, ErrorMessage = "El teléfono no puede exceder los 20 caracteres.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [StringLength(150, ErrorMessage = "El cargo no puede exceder los 150 caracteres.")]
        public string? AdvisorRole { get; set; }

        // Campos mínimos para crear la persona (por compatibilidad con PersonDtoRequest)
        [Required(ErrorMessage = "El tipo de documento es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un tipo de documento válido.")]
        public int DocumentTypeId { get; set; }

        [Required(ErrorMessage = "El número de identificación de la persona es obligatorio.")]
        [StringLength(20, ErrorMessage = "El número de documento no puede exceder los 20 caracteres.")]
        public string DocumentNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [StringLength(300, ErrorMessage = "La dirección no puede exceder los 300 caracteres.")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "La ciudad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una ciudad válida.")]
        public int CityId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un tipo de sangre válido.")]
        public int? BloodTypeId { get; set; }

        public PersonDtoRequest Person { get; set; } = default!;
    }
}
