using System.ComponentModel.DataAnnotations;
using Entity.DTOs.Base;

namespace Entity.DTOs.ModelSecurity.Request
{
    public class MenuStructureRequest : BaseDtoRequest
    {
        // Puede ser nulo, pero si viene debe ser mayor que 0
        [Range(1, int.MaxValue, ErrorMessage = "El Id del menú padre debe ser un número mayor que 0.")]
        public int? ParentMenuId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El Id del módulo debe ser un número mayor que 0.")]
        public int? ModuleId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El Id del formulario debe ser un número mayor que 0.")]
        public int? FormId { get; set; }

        [StringLength(100, ErrorMessage = "El Icono no puede exceder los 100 caracteres.")]
        [RegularExpression(@"(^$|.*\S.*)", ErrorMessage = "El Icono no puede ser solo espacios en blanco.")]
        public string? Icon { get; set; }

        [Required(ErrorMessage = "El Título es obligatorio.")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "El Título debe tener entre 2 y 150 caracteres.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ0-9\s]+$", ErrorMessage = "El Título solo puede contener letras, números y espacios.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "El Tipo es obligatorio.")]
        [StringLength(50, ErrorMessage = "El Tipo no puede exceder los 50 caracteres.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ0-9\s]+$", ErrorMessage = "El Tipo solo puede contener letras, números y espacios.")]
        public string Type { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "El índice de orden debe ser un número mayor o igual que 0.")]
        public int OrderIndex { get; set; }
    }
}
