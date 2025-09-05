using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Specifics
{
    public class PersonCreatedResult
    {
        public int PersonId { get; set; }
        public int? UserId { get; set; }
        public bool? EmailSent { get; set; }
    }
}
