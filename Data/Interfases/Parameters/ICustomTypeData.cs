using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfases;
using Entity.Models.Parameter;

namespace Data.Interfases.Parameters
{
    public interface ICustomTypeData: ICrudBase<CustomType>
    {
       Task<IEnumerable<CustomType>> GetTypesByCategoryNameAsync(string categoryName);

    }
}
