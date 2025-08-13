using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfases
{
    public interface ICrudBase<T> : IRead<T>, ICreate<T>, IUpdate<T>, IDelete, ISoftDelete where T : class 
    {
    }
}
