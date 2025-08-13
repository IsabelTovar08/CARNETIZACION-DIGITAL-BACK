using Data.Classes.Base;
using Data.Interfases.Security;
using Entity.Context;
using Entity.DTOs.ModelSecurity.Request;
using Entity.DTOs.ModelSecurity.Response;
using Entity.Models;
using Entity.Models.ModelSecurity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
namespace Data.Classes.Specifics
{
    public class PersonData : BaseData<Person>, IPersonData
    {
        private readonly IUserData _userData;
        public PersonData(ApplicationDbContext context, ILogger<Person> logger, IUserData userData) : base(context, logger)
        {
            _userData = userData;
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

        public async Task<(Person Person, User User)> SavePersonAndUser(Person person, User user)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Registrar persona
                Person personCreated = await SaveAsync(person);

                if (personCreated == null)
                    throw new Exception("Ocurrió un error al registrar la persona");

                // Registrar usuario vinculado a la persona
                user.PersonId = personCreated.Id;
                User userCreated = await _userData.SaveAsync(user);

                // Confirmar cambios en la BD
                await transaction.CommitAsync();

                return (person, user);
            }
            catch
            {
                // Revertir cambios si hay error
                await transaction.RollbackAsync();
                throw;
            }
        }

    }
}
