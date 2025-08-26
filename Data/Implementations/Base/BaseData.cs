using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Data.Interfases;
using Entity.Context;
using Entity.Models.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Classes.Base
{
    public class BaseData<T> : ABaseData<T> where T : BaseModel
    {
        protected readonly ApplicationDbContext _context;
        protected readonly ILogger<T> _logger;

        public BaseData(ApplicationDbContext context, ILogger<T> logger) 
        {
            _context = context;
            _logger = logger;
        }

        public BaseData(ApplicationDbContext context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<T>> GetAllAsync()
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

        public override async Task<IEnumerable<T>> GetActiveAsync()
        {
            try
            {
                return await _context.Set<T>().Where(x => !x.IsDeleted).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener todos los registros de la entidad {typeof(T).Name}");
                throw;
            }
        }


        public override async Task<T?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener el registro {typeof(T).Name} con ID: {id}");
                throw;
            }
        }

        public override async Task<T> SaveAsync(T entity)
        {
            try
            {
                await _context.Set<T>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al crear el registro para la entidad: {typeof(T).Name}");
                throw;
            }
        }
        public override async Task<T?> UpdateAsync(T entity)
        {

            try
            {
                // Busca la entidad existente en el contexto
                var existingEntity = await _context.Set<T>().FindAsync(entity.Id);

                if (existingEntity != null)
                {
                    // Actualiza los valores de la entidad existente con los de la nueva instancia
                    _context.Entry(existingEntity).CurrentValues.SetValues(entity);
                }
                else
                {
                    // Si la entidad no está en el contexto, adjúntala como nueva
                    _context.Attach(entity);
                    _context.Entry(entity).State = EntityState.Modified;
                }

                // Guarda los cambios
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar la entidad {typeof(T).Name} con ID: {entity.Id}");
                throw;
            }

        }
        public override async Task<bool> DeleteAsync(int id)
        {
            try
            {
                int rowsAffected = await _context.Set<T>().Where(e => e.Id == id).ExecuteDeleteAsync();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar la entidad {typeof(T).Name} con ID: {id}");
                throw;
            }
        }
        public override async Task<bool> ToggleActiveAsync(int id)
        {
            try
            {
                T? entity = await _context.Set<T>().FirstOrDefaultAsync(i => i.Id == id);
                entity.IsDeleted = !entity.IsDeleted;
                await UpdateAsync(entity);
                return entity.IsDeleted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar IsDeleted la entidad {typeof(T).Name} con ID: {id}");
                throw;
            }
        }

        public override async Task<bool> ExistsByAsync(Expression<Func<T, object>> fieldSelector,object? value)
        {
            if (fieldSelector == null)
                throw new ArgumentNullException(nameof(fieldSelector));

            // extraer nombre de la propiedad
            Expression body = fieldSelector.Body is UnaryExpression u && u.NodeType == ExpressionType.Convert
                ? u.Operand
                : fieldSelector.Body;

            if (body is not MemberExpression m)
                throw new ArgumentException("Selector must be a simple property access, e.g., x => x.Property.");

            string propName = m.Member.Name;
            string p = propName;
            object? v = value;

            return await _context.Set<T>()
                .AsNoTracking()
                .Where(e => EF.Property<object>(e, p) == v)
                .AnyAsync();
        }
    }
}
