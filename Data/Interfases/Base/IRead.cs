using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfases
{
    public interface IRead<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);

        Task<IEnumerable<T>> GetActiveAsync();
        //Task<T?> GetByIdActiveAsync(int id);

    }
}
