using Entity.DTOs.Base;
using Entity.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Organizational.Structure.Request
{
    public class OrganizationUpdateDtoRequest : GenericDtoRequest
    {

        [StringLength(300)]
        public string? Description { get; set; }

        //[Base64String(ErrorMessage = "El logo debe estar en Base64 válido.")]
        public string? Logo { get; set; }
        [Required(ErrorMessage = "El identificador del tipo de organización es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador del tipo de organización debe ser un número entero mayor que 0.")]
        public int? TypeId { get; set; }
    }
}
