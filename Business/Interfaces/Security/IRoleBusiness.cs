using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfases;
using Entity.DTOs.ModelSecurity.Request;
using Entity.DTOs.ModelSecurity.Response;
using Entity.Models;

namespace Business.Interfaces.Security
{
    public interface IRoleBusiness: IBaseBusiness<Role, RoleDtoRequest, RolDto>
    {
        Task<bool> UserIsSuperAdminAsync(List<string> roleIds);
    }
}
