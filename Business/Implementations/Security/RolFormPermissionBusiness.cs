using System.Dynamic;
using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Security;
using Data.Interfases.Security;
using Entity.Context;
using Entity.DTOs;
using Entity.DTOs.ModelSecurity;
using Entity.DTOs.ModelSecurity.Request;
using Entity.DTOs.ModelSecurity.Response;
using Entity.Models;
using Microsoft.Extensions.Logging;
using Utilities.Exeptions;

namespace Business.Classes
{
    public class RolFormPermissionBusiness : BaseBusiness<RolFormPermission, RolFormPermissionDtoRequest, RolFormPermissionDto>, IRolFormPermissionBusiness
    {
        private readonly IRolFormPermissionData _rolFormPermissionData;
        public RolFormPermissionBusiness
            (IRolFormPermissionData rolFormPermissionData, ILogger<RolFormPermission> logger, IMapper mapper, ApplicationDbContext context) : base(rolFormPermissionData, logger, mapper)
        {
            _rolFormPermissionData = rolFormPermissionData;
        }

        /// <summary>
        /// Valida que los datos del permiso por formulario y rol sean correctos.
        /// </summary>
        /// <param name="rolFormPermissionDto">DTO con los datos de la asignación</param>
        /// <exception cref="ValidationException">Si algún dato requerido está vacío o es inválido</exception>
        protected void Validate(RolFormPermissionDtoRequest rolFormPermissionDto)
        {
            if (rolFormPermissionDto == null)
                throw new ValidationException("El permiso por formulario para el rol no puede ser nulo.");

            if (rolFormPermissionDto.RolId == null)
                throw new ValidationException("El Rol es obligatorio.");
            if (rolFormPermissionDto.FormId == null)
                throw new ValidationException("El Formulario es obligatorio.");
            if (rolFormPermissionDto.PermissionId == null)
                throw new ValidationException("El Permiso es obligatorio.");
        }

        /// <summary>
        /// Obtiene todos los permisos agrupados por rol y formulario.
        /// </summary>
        public async Task<List<RolFormPermissionsCompletedDto>> GetAllRolFormPermissionsAsync()
        {
            return await _rolFormPermissionData.GetAllRolFormPermissionsAsync();
        }

        /// <summary>
        /// Guarda los permisos para un rol y formulario específicos.
        /// Llama al método de la capa Data y maneja validaciones y errores.
        /// </summary>
        /// <param name="request">Objeto con el RolId, FormId y lista de PermissionIds</param>
        /// <returns>True si la operación fue exitosa, False en caso contrario</returns>
        /// <exception cref="ValidationException">Si el request es nulo o no tiene datos válidos</exception>
        public async Task<bool> SaveRoleFormPermissionsAsync(RoleFormPermissionsRequest request)
        {
            if (request == null)
                throw new ValidationException("El objeto no puede ser nulo.");

            if (request.RoleId <= 0)
                throw new ValidationException("Debe especificar un Rol válido.");

            if (request.FormId <= 0)
                throw new ValidationException("Debe especificar un Formulario válido.");

            if (request.PermissionsIds == null || !request.PermissionsIds.Any())
                throw new ValidationException("Debe especificar al menos un permiso.");

            try
            {
                return await _rolFormPermissionData.SaveRoleFormPermissions(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error en la lógica de negocio al guardar permisos para rol {RolId} y formulario {FormId}",
                    request.RoleId, request.FormId);

                throw new ExternalServiceException( "Ocurrió un error al guardar los permisos. Por favor, intente nuevamente.", ex.ToString());
            }
        }
    }

}
