using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.ModelSecurity.Request
{
    public class PersonRegistrer
    {
        public PersonDtoRequest Person { get; set; }
        public UserDtoRequest User { get; set; }
    }
}
