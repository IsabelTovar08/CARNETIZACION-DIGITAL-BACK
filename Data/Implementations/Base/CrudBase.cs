using System.Dynamic;
using Data.Interfases;
using Entity.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Classes.Base
{
    public class CrudBase<T> : ICrudBase<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly ILogger<T> _logger;
        public CrudBase(ApplicationDbContext context, ILogger<T> logger)
        {
            _context = context;
            _logger = logger;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await _context.Set<T>().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener todos los registros de la entidad {typeof(T).Name}");
                throw;
            }
        }
        public virtual async Task<IEnumerable<T>> GetAllActiveAsync()
        {
            try
            {
                ValidateIsDeleted();

                return await _context.Set<T>()
                    .Where(e => EF.Property<bool>(e, "IsDeleted"))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener entidades activas de tipo {EntityType}", typeof(T).Name);
                throw;
            }
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _context.Set<T>().FindAsync(id);
                if (entity == null) return null;
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener el registro {typeof(T).Name} con ID: {id}");
                throw;
            }
        }
        public virtual async Task<T?> GetByIdActiveAsync(int id)
        {
            try
            {
                ValidateIsDeleted();
                var entity = await _context.Set<T>().FindAsync(id);
                if (entity == null) return null;

                var status = (bool)typeof(T).GetProperty("IsDeleted")!.GetValue(entity)!;
                return status ? entity : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener el registro {typeof(T).Name} con ID: {id}");
                throw;
            }
        }



        public virtual async Task<T> SaveAsync(T entity)
        {
            try
            {
                await _context.Set<T>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex,$"Error al crear el registro para la entidad: {typeof(T).Name}");
                throw;
            }
        }

        public virtual async Task<bool> UpdateAsync(T entity)
        {
            int id = (int)(entity.GetType().GetProperty("Id")?.GetValue(entity)
                ?? throw new Exception("La entidad no tiene un Id válido."));

            try
            {
                var entityExisting = await GetByIdAsync(id);
                if (entityExisting == null) return false;

                _context.Entry(entityExisting).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar la entidad {typeof(T).Name} con ID: {id}");
                throw;
            }
        }


        public virtual async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await GetByIdAsync(id);
                if (entity == null) return false;

                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar la entidad {typeof(T).Name} con ID: {id}");
                throw;
            }
        }


        public virtual async Task<bool> ToggleActiveAsync(int id)
        {
            try
            {
                var entityExisting = await GetByIdAsync(id);
                if (entityExisting == null) return false;

                ValidateIsDeleted();

                var propIsDeleted = typeof(T).GetProperty("IsDeleted")!;
                var currentValue = (bool)propIsDeleted.GetValue(entityExisting)!;
                propIsDeleted.SetValue(entityExisting, !currentValue);

                _context.Entry(entityExisting).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar IsDeleted la entidad {typeof(T).Name} con ID: {id}");
                throw;
            }
        }


        private void ValidateIsDeleted()
        {
            var IsDeleted = typeof(T).GetProperty("IsDeleted");
            if (IsDeleted == null || IsDeleted.PropertyType != typeof(bool))
            {
                throw new InvalidOperationException($"La entidad {typeof(T).Name} no contiene un campo IsDeleted del tipo booleano válido.");
            }
        }

        public Task<List<ExpandoObject>> GetAllDynamicAsync()
        {
            throw new NotImplementedException();
        }
    }
}
