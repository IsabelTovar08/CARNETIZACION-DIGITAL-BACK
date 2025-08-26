using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfases
{
    public interface ICrudBase<T> : IRead<T>, ICreate<T>, IUpdate<T>, IDelete, ISoftDelete where T : class 
    {
        Task<bool> ExistsByAsync(Expression<Func<T, object>> fieldSelector, object? value);
    }
}
