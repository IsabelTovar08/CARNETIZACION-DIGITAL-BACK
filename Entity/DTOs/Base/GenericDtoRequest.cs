using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Base
{
    public class GenericDtoRequest: BaseDtoRequest
    {
        [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El Nombre debe tener entre 2 y 100 caracteres.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ0-9\s]+$", ErrorMessage = "El Nombre solo puede contener letras, números y espacios.")]
        public string Name { get; set; }
    }
}
