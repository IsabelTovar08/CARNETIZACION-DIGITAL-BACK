using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Data.Interfases;
using Entity.DTOs.Base;
using Entity.Models.Base;

namespace Data.Classes.Base
{
    public abstract class ABaseData<T> : ICrudBase<T> where T : BaseModel
    {
        public abstract Task<IEnumerable<T>> GetAllAsync();
        public abstract Task<IEnumerable<T>> GetActiveAsync();
        public abstract Task<T?> GetByIdAsync(int id);
        public abstract Task<T> SaveAsync(T entity);
        public abstract Task<T?> UpdateAsync(T entity);
        public abstract Task<bool> DeleteAsync(int id);
        public abstract Task<bool> ToggleActiveAsync(int id);
        public abstract Task<bool> ExistsByAsync(Expression<Func<T, object>> fieldSelector, object? value);


    }
}
