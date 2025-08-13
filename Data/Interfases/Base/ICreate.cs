using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfases
{
    public interface ICreate<T> where T : class
    {
        Task<T> SaveAsync(T entity);
    }
}
