using Entity.DTOs.Base;
using Entity.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Organizational.Assigment.Request
{
    public class ProfileDtoRequest : GenericDto
    {
        public string? Description { get; set; }
    }
}
