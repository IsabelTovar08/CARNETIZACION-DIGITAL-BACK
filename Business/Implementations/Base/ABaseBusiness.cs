
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Business.Interfases;
using Data.Classes.Base;
using Data.Interfases;
using Entity.DTOs.Base;
using Entity.DTOs.ModelSecurity.Request;
using Entity.Models.Base;

namespace Business.Classes.Base
{
    public abstract class ABaseBusiness<T, DCreate, D> : IBaseBusiness<T, DCreate, D> where T : BaseModel where D : class
    {
        public abstract Task<D> Save(DCreate entity);
        public abstract Task<bool> Delete(int id);
        public abstract Task<IEnumerable<D>> GetAll();
        public abstract Task<IEnumerable<D>> GetActive();
        public abstract Task<D?> GetById(int id);
        public abstract Task<bool> ToggleActiveAsync(int id);
        public abstract Task<D?> Update(DCreate entity);

        public abstract Task ValidateAsync(T entity);
        public abstract void Validate(DCreate d);
    }
   
}
