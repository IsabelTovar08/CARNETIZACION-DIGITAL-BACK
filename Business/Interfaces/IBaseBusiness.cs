using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;

namespace Business.Interfases
{
    public interface IBaseBusiness<T, DCreate, D>
    {
        Task<IEnumerable<D>> GetAll();
        Task<IEnumerable<D>> GetActive();

        Task<D?> GetById(int id);
        Task<D> Save(DCreate entity);
        Task<D?> Update(DCreate entity);
        Task<bool> Delete(int id);
        Task<bool> ToggleActiveAsync(int id);

        Task ValidateAsync(T newEntity);
        void Validate(DCreate create);

    }
}
