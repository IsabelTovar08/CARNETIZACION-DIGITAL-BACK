using System.ComponentModel.DataAnnotations;
using Entity.DTOs.Base;

namespace Entity.DTOs.Organizational.Structure.Request
{
    public class OrganizationalUnitBranchDtoRequest : BaseDtoRequest
    {
        [Required(ErrorMessage = "El identificador de la sede es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador de la sede debe ser un número entero mayor que 0.")]
        public int BranchId { get; set; }

        [Required(ErrorMessage = "El identificador de la unidad organizacional es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador de la unidad organizacional debe ser un número entero mayor que 0.")]
        public int OrganizationalUnitId { get; set; }
    }
}
