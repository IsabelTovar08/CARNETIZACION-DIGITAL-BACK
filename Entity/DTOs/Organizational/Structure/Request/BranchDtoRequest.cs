using System.ComponentModel.DataAnnotations;
using Entity.DTOs.Base;

namespace Entity.DTOs.Organizational.Structure.Request
{
    public class BranchDtoRequest : GenericDtoRequest
    {
        [Required(ErrorMessage = "El nombre de la sede es obligatorio.")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "El nombre de la sede debe tener entre 2 y 150 caracteres.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ0-9\s]+$", ErrorMessage = "El nombre de la sede solo puede contener letras, números y espacios.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "La ubicación no puede exceder los 200 caracteres.")]
        [RegularExpression(@"(^$|.*\S.*)", ErrorMessage = "La ubicación no puede estar compuesta únicamente por espacios en blanco.")]
        public string? Location { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [StringLength(20, ErrorMessage = "El teléfono no puede exceder los 20 caracteres.")]
        [RegularExpression(@"^[0-9\+]+$", ErrorMessage = "El teléfono solo puede contener números y el signo +.")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
        [StringLength(200, ErrorMessage = "El correo electrónico no puede exceder los 200 caracteres.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [StringLength(300, ErrorMessage = "La dirección no puede exceder los 300 caracteres.")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "El identificador de la ciudad es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador de la ciudad debe ser un número entero mayor que 0.")]
        public int CityId { get; set; }

        [Required(ErrorMessage = "El identificador de la organización es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador de la organización debe ser un número entero mayor que 0.")]
        public int OrganizationId { get; set; }
    }
}
