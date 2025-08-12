using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Classes.Base;
using Business.Interfases;
using Entity.DTOs.ModelSecurity.Request;
using Entity.DTOs.ModelSecurity.Response;
using Entity.Models;

namespace Business.Interfaces.Security
{
    public interface IUserRoleBusiness : IBaseBusiness<UserRoles, UserRoleDtoRequest, UserRolDto>
    {
        /// <summary>
        /// Obtiene una lista de roles para un usuario específico.
        /// </summary>
        Task<List<string>> GetRolesByUserIdAsync(int id);

        /// <summary>
        /// Guarda una lista de roles para un usuario específico.
        /// Si el rol ya está asignado al usuario, se omite para evitar duplicados.
        /// </summary>
        /// <param name="request">Objeto con el Id del usuario y lista de Ids de roles.</param>
        /// <returns>True si se guardaron correctamente, false si ocurrió un error.</returns>

        Task<bool> SaveUserRolesAsync(UserRolesRequest request);
    }
}
