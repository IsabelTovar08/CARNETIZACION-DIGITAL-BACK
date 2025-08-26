using System.ComponentModel.DataAnnotations;
using Entity.DTOs.Base;

namespace Entity.DTOs.ModelSecurity.Request
{
    public class PersonDtoRequest : BaseDtoRequest
    {
        [Required(ErrorMessage = "El primer nombre es obligatorio.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El primer nombre debe tener entre 2 y 100 caracteres.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El primer nombre solo puede contener letras y espacios.")]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "El segundo nombre no puede exceder los 100 caracteres.")]
        [RegularExpression(@"(^$|^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$)", ErrorMessage = "El segundo nombre solo puede contener letras y espacios.")]
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "El primer apellido es obligatorio.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El primer apellido debe tener entre 2 y 100 caracteres.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El primer apellido solo puede contener letras y espacios.")]
        public string LastName { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "El segundo apellido no puede exceder los 100 caracteres.")]
        [RegularExpression(@"(^$|^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$)", ErrorMessage = "El segundo apellido solo puede contener letras y espacios.")]
        public string? SecondLastName { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El tipo de documento debe ser un número entero mayor que 0.")]
        public int? DocumentTypeId { get; set; }

        [StringLength(10, ErrorMessage = "El número de documento no puede exceder los 10 caracteres.")]
        [RegularExpression(@"(^$|^[0-9A-Za-z\-]+$)", ErrorMessage = "El número de documento solo puede contener letras, números y guiones.")]
        public string? DocumentNumber { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El tipo de sangre debe ser un número entero mayor que 0.")]
        public int? BloodTypeId { get; set; }

        [StringLength(20, ErrorMessage = "El teléfono no puede exceder los 20 caracteres.")]
        [RegularExpression(@"(^$|^[0-9\+]+$)", ErrorMessage = "El teléfono solo puede contener números y el signo +.")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
        [StringLength(200, ErrorMessage = "El correo electrónico no puede exceder los 200 caracteres.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [StringLength(300, ErrorMessage = "La dirección no puede exceder los 300 caracteres.")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "La ciudad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador de la ciudad debe ser un número entero mayor que 0.")]
        public int CityId { get; set; }
    }
}
