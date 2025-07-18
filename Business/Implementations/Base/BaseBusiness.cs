using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfases;
using Data.Interfases;
using Entity.DTOs.Base;
using Entity.Models.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Utilities.Exeptions;

namespace Business.Classes.Base
{
    public class BaseBusiness<T, D> : ABaseBusiness<T, D> where T : BaseModel where D  : BaseDTO
    {
        protected readonly ICrudBase<T> _data;
        protected readonly ILogger<T> _logger;
        protected readonly IMapper _mapper;
        public BaseBusiness(ICrudBase<T> data, ILogger<T> logger, IMapper mapper) 
        {
            _data = data;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Crea una nueva entidad.
        /// </summary>
        public override async Task<D> Save(D entity)
        {
            try
            {
                BaseModel newEntity = _mapper.Map<T>(entity);
                newEntity = await _data.SaveAsync((T)newEntity);
                return _mapper.Map<D>(newEntity);
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (DbUpdateException dbEx)
            {
                var mensaje = ParseUniqueConstraintError(dbEx);
                throw new ValidationException(mensaje);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al crear {typeof(T).Name}");
                throw new ExternalServiceException("Base de datos", $"Error al craer {typeof(T).Name}");
            }
        }

        /// <summary>
        /// Eliminar permanentemente una entidad.
        /// </summary>
        public override async Task<bool> Delete(int id)
        {
            if (id <= 0)
                throw new ValidationException("id", "El id debe ser mayor que cero.");
            try
            {
                var deleted = await _data.DeleteAsync(id);
                if (deleted == null)
                {
                    throw new ExternalServiceException("Base de datos", $"No se pudo eliminar {typeof(T).Name}");
                }
                return deleted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar {typeof(T).Name} con Id: {id}");
                throw new ExternalServiceException("Base de datos", $"Error al eliminar {typeof(T).Name} con ID: {id}");
            }
        }

        /// <summary>
        /// Obtiene todos los registros.
        /// </summary>
        /// 
        public override async Task<IEnumerable<D>> GetAll()
        {
            try
            {
                IEnumerable<T> list = await _data.GetAllAsync();
                IEnumerable<D> listDto = _mapper.Map<IEnumerable<D>>(list);
                return listDto.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener todos los {typeof(T).Name}");
                throw new ExternalServiceException("Base de datos", $"Error al obtener todos los {typeof(T).Name}", ex);
            }
            
        }

        /// <summary>
        /// Obtiene una entidad por ID.
        /// </summary>
        public override async Task<D?> GetById(int id)
        {
            if (id <= 0)
            {
                throw new ValidationException("Id", "El Id debe ser mayor que cero.");
            }
            try
            {
                T entity = await _data.GetByIdAsync(id);
                D dto = _mapper.Map<D>(entity);
                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener {typeof(T).Name} con Id: {id}");
                throw new ExternalServiceException("Base de Datos", $"Error al obtener {typeof(T).Name}", ex);
            }
        }

        /// <summary>
        /// Gestionar el eliminado lógico en una entidad.
        /// </summary>
        public override async Task<bool> ToggleActiveAsync(int id)
        {
            if (id <= 0)
                throw new ValidationException("id", "El id debe ser mayor que cero.");
            try
            {
                var deleted = await _data.ToggleActiveAsync(id);
                if (deleted == null)
                {
                    throw new ExternalServiceException("Base de datos", $"No se pudo actualizar el estado lógico {typeof(T).Name}");
                }
                return deleted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar el estado lógico {typeof(T).Name} con Id: {id}");
                throw new ExternalServiceException("Base de datos", $"Error al actualizar el estado lógico {typeof(T).Name} con ID: {id}");
            }
        }

        /// <summary>
        /// Actualiza una nueva entidad.
        /// </summary>
        public override async Task<bool> Update(D entity)
        {
            try
            {
                BaseModel newEntity = _mapper.Map<T>(entity);
                await _data.UpdateAsync((T)newEntity);
                return true;
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (DbUpdateException dbEx)
            {
                var mensaje = ParseUniqueConstraintError(dbEx);
                throw new ValidationException(mensaje);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar {typeof(T).Name}");
                throw new ExternalServiceException("Base de datos", $"Error al actualizar {typeof(T).Name}");
            }
        }


        private string ParseUniqueConstraintError(Exception ex)
        {
            var message = ex.InnerException?.Message ?? ex.Message;

            if (message.Contains("IX_Users_Email"))
                return "Este correo electrónico ya está registrado.";
            if (message.Contains("IX_ModuleForms_ModuleId_FormId"))
                return "Este formulario ya ha sido asignado al módulo.";
            if (message.Contains("IX_UserRoles_UserId_RoleId"))
                return "Este rol ya ha sido asignado al usuario.";
            if (message.Contains("IX_RolFormPermissions_RolId_FormId_PermissionId"))
                return "Ya se ha asignado este permiso para el rol y formulario seleccionados.";

            // Fallback general pero entendible
            return "Ya existe un registro con esta combinación de datos. Verifica la información.";
        }


    }
}
