using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.ModelSecurity.Request
{
    public class UserRolesRequest
    {
        public int UserId { get; set; }
        public List<int> RolesId { get; set; } = new List<int>();
    }
}
