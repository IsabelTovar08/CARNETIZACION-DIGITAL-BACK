using System.ComponentModel.DataAnnotations;
using Entity.DTOs.Base;

namespace Entity.DTOs.ModelSecurity.Request
{
    public class PermissionDtoRequest : GenericDtoRequest
    {
        [StringLength(300, ErrorMessage = "La descripción no puede exceder los 300 caracteres.")]
        [RegularExpression(@"(^$|.*\S.*)", ErrorMessage = "La descripción no puede estar compuesta únicamente por espacios en blanco.")]
        public string? Description { get; set; }
    }
}
