using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.ModelSecurity.Response;

namespace Entity.DTOs.ModelSecurity
{
    public class RolFormPermissionsCompletedDto
    {
        public int RolId { get; set; }

        public string RolName { get; set; }

        public int FormId { get; set; }
        public string FormName { get; set; }

        public List<PermissionDto> Permissions { get; set; }
    }
}
