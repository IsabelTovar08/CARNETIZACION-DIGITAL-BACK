
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfases;
using Data.Classes.Base;
using Entity.DTOs.Base;
using Entity.Models.Base;

namespace Business.Classes.Base
{
    public abstract class ABaseBusiness<T, D> : IBaseBusiness<T, D> where T : BaseModel where D : BaseDTO
    {
        public abstract Task<D> Save(D entity);
        public abstract Task<bool> Delete(int id);
        public abstract Task<IEnumerable<D>> GetAll();
        public abstract Task<D?> GetById(int id);
        public abstract Task<bool> ToggleActiveAsync(int id);
        public abstract Task<bool> Update(D entity);

    }
   
}
