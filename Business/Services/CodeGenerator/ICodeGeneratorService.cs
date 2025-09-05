using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Base;
using Entity.Models.Base;

namespace Business.Services.CodeGenerator
{
    public interface ICodeGeneratorService<T> where T : BaseModel
    {
        // Genera Code solo si Name tiene valor y Code está vacío.
        Task<string> EnsureCodeAsync(T entity, Func<string, int, Task<bool>> codeExists);
    }
}
