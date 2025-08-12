using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfases
{
    public interface IUpdate<T> where T : class
    {
        Task<T?> UpdateAsync(T entity);
    }
}
