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

namespace Business.Interfaces.Security
{
    public interface IRolFormPermissionBusiness : IBaseBusiness<RolFormPermission, RolFormPermissionDtoRequest, RolFormPermissionDto>
    {
        /// <summary>
        /// Obtiene una lista agrupada de todos los permisos asignados por rol y formulario.
        /// El resultado incluye la información del rol, el formulario y la lista de permisos únicos.
        /// </summary>
        /// <returns>Lista de RolFormPermissionsCompletedDto con permisos agrupados</returns>
        Task<List<RolFormPermissionsCompletedDto>> GetAllRolFormPermissionsAsync();

        /// <summary>
        /// Guarda una lista de permisos para un rol y formulario específicos.
        /// </summary>
        /// <param name="request">Objeto con RolId, FormId y lista de PermissionsIds</param>
        /// <returns>True si el guardado fue exitoso, false en caso de error</returns>
        Task<bool> SaveRoleFormPermissionsAsync(RoleFormPermissionsRequest request);
    }

}
