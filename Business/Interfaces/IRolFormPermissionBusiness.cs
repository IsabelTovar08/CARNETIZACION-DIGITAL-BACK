using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfases;
using Entity.DTOs.ModelSecurity;
using Entity.DTOs.ModelSecurity.Request;
using Entity.DTOs.ModelSecurity.Response;
using Entity.Models;

namespace Business.Interfaces
{
    public interface IRolFormPermissionBusiness : IBaseBusiness<RolFormPermission, RolFormPermissionDtoRequest, RolFormPermissionDto>
    {
        Task<List<RolFormPermissionsCompletedDto>> GetAllRolFormPermissionsAsync();
    }

}
