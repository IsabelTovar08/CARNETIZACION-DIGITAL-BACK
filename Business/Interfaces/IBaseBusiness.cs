using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfases
{
    public interface IBaseBusiness<T, DCreate, D>
    {
        Task<IEnumerable<D>> GetAll();

        Task<D?> GetById(int id);
        Task<D> Save(DCreate entity);
        Task<bool> Update(DCreate entity);
        Task<bool> Delete(int id);
        Task<bool> ToggleActiveAsync(int id);
    }
}
