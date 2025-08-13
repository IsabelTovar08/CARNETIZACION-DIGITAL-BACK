using Data.Classes.Base;
using Data.Interfases.Security;
using Entity.Context;
using Entity.DTOs.ModelSecurity;
using Entity.DTOs.ModelSecurity.Request;
using Entity.DTOs.ModelSecurity.Response;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Classes.Specifics
{
    public class RolFormPermissionData : BaseData<RolFormPermission>, IRolFormPermissionData
    {
        public RolFormPermissionData(ApplicationDbContext context, ILogger<RolFormPermission> logger) : base(context, logger)
        {
        }

        // <summary>
        /// Obtiene todos los registros de RolFormPermission incluyendo
        /// la información relacionada de Rol, Form y Permission.
        /// </summary>
        /// <returns>Lista completa de RolFormPermission con relaciones cargadas</returns>
        public override async Task<IEnumerable<RolFormPermission>> GetAllAsync()
        {
            try
            {
                return await _context.Set<RolFormPermission>()
                    .Include(rfp => rfp.Rol)
                    .Include(rfp => rfp.Form)
                    .Include(rfp => rfp.Permission)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving RolFormPermissions");
                throw;
            }
        }

        /// <summary>
        /// Obtiene un registro de RolFormPermission por su identificador,
        /// incluyendo Rol, Form y Permission asociados.
        /// </summary>
        /// <param name="id">Identificador del RolFormPermission</param>
        /// <returns>Instancia de RolFormPermission encontrada o null si no existe</returns>
        public override async Task<RolFormPermission> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<RolFormPermission>()
                    .Include(rfp => rfp.Rol)
                    .Include(rfp => rfp.Form)
                    .Include(rfp => rfp.Permission)
                    .FirstOrDefaultAsync(rfp => rfp.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving RolFormPermissions");
                throw;
            }
        }


        /// <summary>
        /// Obtiene una lista agrupada de todos los permisos asignados por rol y formulario.
        /// El resultado incluye la información del rol, el formulario y la lista de permisos únicos.
        /// </summary>
        /// <returns>Lista de RolFormPermissionsCompletedDto con permisos agrupados</returns>
        public async Task<List<RolFormPermissionsCompletedDto>> GetAllRolFormPermissionsAsync()
        {
            var result = await _context.Set<RolFormPermission>()
                .Include(rfp => rfp.Rol)
                .Include(rfp => rfp.Form)
                .Include(rfp => rfp.Permission)
                .ToListAsync();

            var agrupado = result
                .GroupBy(rfp => new
                {
                    rfp.RolId,
                    RolName = rfp.Rol.Name,
                    rfp.FormId,
                    FormName = rfp.Form.Name
                })
                .Select(g => new RolFormPermissionsCompletedDto
                {
                    RolId = g.Key.RolId,
                    RolName = g.Key.RolName,
                    FormId = g.Key.FormId,
                    FormName = g.Key.FormName,
                    Permissions = g
                        .Select(x => new PermissionDto
                        {
                            Id = x.Permission.Id,
                            Name = x.Permission.Name,
                            Description = x.Permission.Description
                        })
                        .DistinctBy(p => p.Id)
                        .ToList()
                })
                .ToList();

            return agrupado;
        }

        /// <summary>
        /// Guarda una lista de permisos para un rol y formulario específicos.
        /// Si un permiso ya está asignado, no se vuelve a insertar.
        /// El proceso se ejecuta dentro de una transacción para asegurar atomicidad.
        /// </summary>
        /// <param name="request">Objeto con RolId, FormId y lista de PermissionsIds</param>
        /// <returns>True si el guardado fue exitoso, false en caso de error</returns>
        /// <exception cref="ArgumentException">Si no se especifica al menos un permiso</exception>
        public async Task<bool> SaveRoleFormP(RoleFormPermissionsRequest request)
        {
            if (request == null || request.PermissionsIds == null || !request.PermissionsIds.Any())
                throw new ArgumentException("Debe especificar al menos un permiso.");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                foreach (var permissionId in request.PermissionsIds.Distinct())
                {
                    var exists = await _context.Set<RolFormPermission>()
                        .AnyAsync(p => p.RolId == request.RoleId &&
                                       p.FormId == request.FormId &&
                                       p.PermissionId == permissionId);

                    if (!exists)
                    {
                        var permisionSave = new RolFormPermission
                        {
                            RolId = request.RoleId,
                            FormId = request.FormId,
                            PermissionId = permissionId
                        };

                        await SaveAsync(permisionSave);
                    }
                }

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error guardando permisos para rol {RolId} y formulario {FormId}", request.RoleId, request.FormId);
                return false;
            }
        }

        /// <summary>
        /// Actualiza los permisos asociados a un rol y formulario eliminando los actuales
        /// y agregando los nuevos indicados. Opera dentro de una transacción para mantener la consistencia.
        /// </summary>
        /// <param name="request">Datos con RolId, FormId y lista de PermissionsIds.</param>
        /// <returns>True si la operación fue exitosa; false en caso contrario.</returns>
        /// <exception cref="ArgumentException">Si no se especifica al menos un permiso.</exception>
        public async Task<bool> SaveRoleFormPermissions(RoleFormPermissionsRequest request)
        {
            if (request == null || request.PermissionsIds == null || !request.PermissionsIds.Any())
                throw new ArgumentException("Debe especificar al menos un permiso.");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var newIds = request.PermissionsIds.Distinct().ToList();

                var repo = _context.Set<RolFormPermission>();

                // Obtener permisos actuales para el rol y formulario
                var existing = await repo
                    .Where(p => p.RolId == request.RoleId && p.FormId == request.FormId)
                    .ToListAsync();

                var existingIds = existing.Select(p => p.PermissionId).ToList();

                // IDs que hay que insertar
                var toInsert = newIds.Except(existingIds).ToList();
                if (toInsert.Any())
                {
                    var nuevosPermisos = toInsert.Select(pid => new RolFormPermission
                    {
                        RolId = request.RoleId,
                        FormId = request.FormId,
                        PermissionId = pid
                    }).ToList();

                    await repo.AddRangeAsync(nuevosPermisos);
                }

                // IDs que hay que eliminar
                var toDelete = existingIds.Except(newIds).ToList();
                if (toDelete.Any())
                {
                    await repo
                        .Where(p => p.RolId == request.RoleId &&
                                    p.FormId == request.FormId &&
                                    toDelete.Contains(p.PermissionId))
                        .ExecuteDeleteAsync();
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error actualizando permisos para rol {RolId} y formulario {FormId}", request.RoleId, request.FormId);
                return false;
            }
        }


    }
}

