using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Classes.Base;
using Entity.Context;
using Entity.Models.Parameter;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Implementations.Parameters
{
    public class CustomTypeData : BaseData<CustomType>
    {
        public CustomTypeData(ApplicationDbContext context, ILogger<CustomType> logger) : base(context, logger)
        {
        }

        public override async Task<IEnumerable<CustomType>> GetAllAsync()
        {
            return await _context.Set<CustomType>()
                .Include(ct => ct.TypeCategory)
                .ToListAsync();
        }

        public override async Task<CustomType?> GetByIdAsync(int id)
        {
            return await _context.Set<CustomType>()
                .Include(ct => ct.TypeCategory)
                .FirstOrDefaultAsync(ct => ct.Id == id);
        }  

    }
}
