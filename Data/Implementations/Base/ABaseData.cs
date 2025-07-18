using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
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
        public abstract Task<T?> GetByIdAsync(int id);
        public abstract Task<T> SaveAsync(T entity);
        public abstract Task<bool> UpdateAsync(T entity);
        public abstract Task<bool> DeleteAsync(int id);
        public abstract Task<bool> ToggleActiveAsync(int id);


    }
}
