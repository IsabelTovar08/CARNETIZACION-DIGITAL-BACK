using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Operational.Response
{
    public class PersonSupervisorDtoResponse
    {
        public int PersonId { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
