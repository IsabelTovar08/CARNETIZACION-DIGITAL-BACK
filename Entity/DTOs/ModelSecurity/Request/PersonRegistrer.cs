using System.ComponentModel.DataAnnotations;

namespace Entity.DTOs.ModelSecurity.Request
{
    public class PersonRegistrer
    {
        [Required(ErrorMessage = "La información de la persona es obligatoria.")]
        public PersonDtoRequest Person { get; set; } = null!;

        [Required(ErrorMessage = "La información del usuario es obligatoria.")]
        public UserDtoRequest User { get; set; } = null!;
    }
}
