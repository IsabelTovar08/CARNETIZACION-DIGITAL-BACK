using Data.Classes.Base;
using Data.Interfases.Security;
using Entity.Context;
using Entity.DTOs.ModelSecurity.Request;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Classes.Specifics
{
    public class UserRolesData : BaseData<UserRoles>, IUserRoleData
    {
        public UserRolesData(ApplicationDbContext context, ILogger<UserRoles> logger) : base(context, logger)
        {
        }


        /// <summary>
        /// Obtiene todos los registros de roles asignados a usuarios.
        /// Incluye la información del usuario y el rol.
        /// </summary>
        /// <returns>Lista de objetos UserRoles.</returns>
        public override async Task<IEnumerable<UserRoles>> GetAllAsync()
        {
            try
            {
                return await _context.Set<UserRoles>()
                    .Include(ur => ur.User)
                    .Include(ur => ur.Rol)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving UserRols");
                throw;
            }
        }

        /// <summary>
        /// Obtiene un registro específico de UserRoles por su identificador.
        /// Incluye la información del usuario y el rol.
        /// </summary>
        /// <param name="id">Id del registro UserRoles.</param>
        /// <returns>Objeto UserRoles si existe, null si no.</returns>
        public override async Task<UserRoles?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<UserRoles>()
                    .Include(ur => ur.User)
                    .Include(ur => ur.Rol)
                    .FirstOrDefaultAsync(ur => ur.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving UserRols");
                throw;
            }
        }

        /// <summary>
        /// Obtiene los nombres de todos los roles asociados a un usuario por su Id.
        /// </summary>
        /// <param name="userId">Id del usuario.</param>
        /// <returns>Lista de nombres de roles.</returns>
        public async Task<List<string>> GetRolesByUserId(int userId)
        {
            try
            {
                return await _context.Set<UserRoles>()
                    .Include(ur => ur.Rol)
                    .Where(ur => ur.User.Id == userId)
                    .Select(ur => ur.Rol.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving roles for user with ID {userId}");
                throw;
            }
        }

        /// <summary>
        /// Guarda una lista de roles para un usuario específico.
        /// Si el rol ya está asignado al usuario, se omite para evitar duplicados.
        /// </summary>
        /// <param name="request">Objeto con el Id del usuario y lista de Ids de roles.</param>
        /// <returns>True si se guardaron correctamente, false si ocurrió un error.</returns>

        public async Task<bool> SaveUserRoles(UserRolesRequest request)
        {
            if (request == null || request.RolesId == null || !request.RolesId.Any())
                throw new ArgumentException("Debe especificar al menos un rol.");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Obtener roles actuales del usuario
                var currentRoles = await _context.Set<UserRoles>()
                    .Where(r => r.UserId == request.UserId)
                    .ToListAsync();

                var rolesToAdd = request.RolesId.Except(currentRoles.Select(r => r.RolId)).Distinct();
                var rolesToRemove = currentRoles.Select(r => r.RolId).Except(request.RolesId).ToList();

                // Agregar los roles que faltan
                foreach (var roleId in rolesToAdd)
                {
                    await SaveAsync(new UserRoles
                    {
                        UserId = request.UserId,
                        RolId = roleId
                    });
                }

                // Eliminar los roles que ya no están
                foreach (var roleId in rolesToRemove)
                {
                    var userRole = currentRoles.First(r => r.RolId == roleId);
                    _context.Set<UserRoles>().Remove(userRole);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error guardando roles para el usuario {UserId}", request.UserId);
                return false;
            }
        }

    }
}

