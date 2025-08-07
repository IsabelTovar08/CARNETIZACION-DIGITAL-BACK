using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.ModelSecurity;
using Entity.Models;

namespace Data.Interfases
{
    public interface IRolFormPermissionData : ICrudBase<RolFormPermission>
    {
        Task<List<RolFormPermissionsCompletedDto>> GetAllRolFormPermissionsAsync();
    }

}
