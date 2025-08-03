using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Base;

namespace Entity.DTOs.ModelSecurity.Request
{
    public class FormDtoRequest : GenericBaseDto
    {
        public string? Description { get; set; }
    }
}
