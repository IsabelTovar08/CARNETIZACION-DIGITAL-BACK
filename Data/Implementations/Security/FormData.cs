using Data.Classes.Base;
using Entity;
using Entity.Context;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
namespace Data.Classes.Specifics
{
    public class FormData : BaseData<Form>
    {
        public FormData(ApplicationDbContext context, ILogger<Form> logger) : base(context, logger)
        {

        }

        public override async Task<IEnumerable<Form>> GetAllAsync()
        {
            return await _context.Set<Form>()
                .Include(f => f.Module)
                .ToListAsync();
        }

        public override async Task<Form?> GetByIdAsync(int id)
        {
            return await _context.Set<Form>()
                .Include(f => f.Module)
                .FirstOrDefaultAsync(f => f.Id == id);
        }
    }
}