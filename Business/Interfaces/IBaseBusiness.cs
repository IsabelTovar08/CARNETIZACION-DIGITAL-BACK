using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfases
{
    public interface IBaseBusiness<T, D>
    {
        Task<IEnumerable<D>> GetAll();

        Task<D?> GetById(int id);
        Task<D> Save(D entity);
        Task<bool> Update(D entity);
        Task<bool> Delete(int id);
        Task<bool> ToggleActiveAsync(int id);
    }
}
