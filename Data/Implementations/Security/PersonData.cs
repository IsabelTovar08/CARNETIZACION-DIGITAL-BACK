using Data.Classes.Base;
using Entity.Context;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
namespace Data.Classes.Specifics
{
    public class PersonData : BaseData<Person>
    {

        public PersonData(ApplicationDbContext context, ILogger<Person> logger) : base(context, logger)
        {
        }

        public async Task<Person?> FindByIdentification(string identification)
        {
            return await _context.Set<Person>().Where(u => !u.IsDeleted).FirstOrDefaultAsync(p => p.Identification == identification);
        }
    }
}
