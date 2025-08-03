using Data.Classes.Base;
using Entity.Context;
using Entity.DTOs.ModelSecurity.Response;
using Entity.Models;
using Entity.Models.ModelSecurity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
namespace Data.Classes.Specifics
{
    public class PersonData : BaseData<Person>
    {
        public PersonData(ApplicationDbContext context, ILogger<Person> logger) : base(context, logger)
        {
        }

        public override async Task<IEnumerable<Person>> GetAllAsync()
        {
            return await _context.Set<Person>()
                .Include(p => p.City)
                .Include(p => p.DocumentType)
                .Include(p => p.BloodType)

                .ToListAsync();
        }
        public async Task<Person?> FindByIdentification(string identification)
        {
            return await _context.Set<Person>().Where(u => !u.IsDeleted).FirstOrDefaultAsync(p => p.DocumentNumber == identification);
        }
    }
}
