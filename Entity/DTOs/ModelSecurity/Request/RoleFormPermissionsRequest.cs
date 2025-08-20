using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.ModelSecurity.Request
{
    public class RoleFormPermissionsRequest
    {
        [Required]
        public int RoleId { get; set; }
        [Required]
        public int FormId { get; set; }
        [MinLength(1)]
        public List<int> PermissionsIds { get; set; } = new List<int>();

    }
}
