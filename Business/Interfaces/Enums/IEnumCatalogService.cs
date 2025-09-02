using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Enums;

namespace Business.Interfaces.Enums
{
    public interface IEnumCatalogService
    {
        Task<IEnumerable<EnumOptionDto>> GetByTypeAsync(string enumType);
    }
}
