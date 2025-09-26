using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.ModelSecurity.Request;

namespace Entity.DTOs.Organizational.Assigment.Request
{
    public class PersonWithDivissionAndProfile : PersonDtoRequest
    {
        public int ProfileId { get; set; }
        public int InternalDivissionId { get; set; }
    }
}
