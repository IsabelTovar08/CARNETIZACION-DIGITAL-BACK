using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.ModelSecurity.Response
{
    public class PersonRegistrerDto
    {
        public PersonDto Person { get; set; }
        public UserDTO User { get; set; }
    }
}
