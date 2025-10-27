using Data.Classes.Base;
using Data.Interfases.Security;
using Entity.Context;
using Entity.DTOs.ModelSecurity.Request;
using Entity.DTOs.ModelSecurity.Response;
using Entity.DTOs.Specifics;
using Entity.Models;
using Entity.Models.ModelSecurity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Data.Classes.Specifics
{
    public class PersonData : BaseData<Person>, IPersonData
    {
        private readonly IUserData _userData;

        public PersonData(ApplicationDbContext context, ILogger<Person> logger, IUserData userData)
            : base(context, logger)
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
            return await _context.Set<Person>()
                .Where(u => !u.IsDeleted)
                .FirstOrDefaultAsync(p => p.DocumentNumber == identification);
        }

        public async Task<Person?> GetPersonInfo(int id)
        {
            return await _context.Set<Person>()
                .Include(u => u.IssuedCard.Where(up => up.IsCurrentlySelected))
                    .ThenInclude(pdp => pdp.InternalDivision)
                        .ThenInclude(id => id.OrganizationalUnit)
                            .ThenInclude(ou => ou.OrganizationalUnitBranches)
                                .ThenInclude(oub => oub.Branch)
                                    .ThenInclude(b => b.Organization)
                .Where(u => !u.IsDeleted)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<(Person Person, User User)> SavePersonAndUser(Person person, User user)
        {
            var hasAmbientTx = _context.Database.CurrentTransaction != null;
            if (!hasAmbientTx)
                await _context.Database.BeginTransactionAsync();

            try
            {
                // Registrar persona
                Person personCreated = await SaveAsync(person);

                if (personCreated == null)
                    throw new Exception("Ocurrió un error al registrar la persona");

                // Registrar usuario vinculado a la persona
                user.PersonId = personCreated.Id;
                User userCreated = await _userData.SaveAsync(user);

                if (!hasAmbientTx)
                    await _context.Database.CommitTransactionAsync();

                return (person, user);
            }
            catch
            {
                // Revertir cambios si hay error
                if (!hasAmbientTx)
                    await _context.Database.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<PersonOrganizationalInfoDto?> GetOrganizationalInfo(int personId)
        {
            return await _context.Set<Person>()
                .AsNoTracking()
                .Where(p => p.Id == personId && !p.IsDeleted)
                .Select(p => new PersonOrganizationalInfoDto
                {
                    InternalDivissionCode = p.IssuedCard
                        .Where(x => x.IsCurrentlySelected)
                        .Select(x => x.InternalDivision.Code)
                        .FirstOrDefault(),

                    OrganizationUnitCode = p.IssuedCard
                        .Where(x => x.IsCurrentlySelected)
                        .Select(x => x.InternalDivision.OrganizationalUnit.Code)
                        .FirstOrDefault(),

                    OrganizationCode = p.IssuedCard
                        .Where(x => x.IsCurrentlySelected)
                        .SelectMany(x => x.InternalDivision.OrganizationalUnit.OrganizationalUnitBranches)
                        .Select(oub => oub.Branch.Organization.Code)
                        .FirstOrDefault()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Person?> GetPersonByUserIdAsync(int userId)
        {
            return await _context.Users
                .Where(u => u.Id == userId && !u.IsDeleted)
                .Include(u => u.Person)
                .Select(u => u.Person)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Filtro + paginación:
        /// - Solo personas no eliminadas
        /// - Con al menos un PersonDivisionProfile con un Card
        /// - Filtros opcionales por división interna, unidad organizativa y perfil
        /// </summary>
        public async Task<(IList<Person> Items, int Total)> QueryWithFiltersAsync(
            int? internalDivisionId,
            int? organizationalUnitId,
            int? profileId,
            int page,
            int pageSize,
            CancellationToken ct = default)
        {
            var q = _context.Set<Person>()
                .AsNoTracking()
                // Incluir relaciones relevantes
                .Include(p => p.IssuedCard)
                    .ThenInclude(pdp => pdp.InternalDivision)
                        .ThenInclude(id => id.OrganizationalUnit)
                .Include(p => p.IssuedCard)
                    .ThenInclude(pdp => pdp.Profile)
                .Include(p => p.IssuedCard)
                    .ThenInclude(pdp => pdp.Card) // carnet singular
                                                  // Reglas de negocio
                .Where(p => !p.IsDeleted)
                .Where(p => p.IssuedCard.Any(pdp => pdp.Card != null));

            // Filtros opcionales
            if (internalDivisionId.HasValue)
                q = q.Where(p => p.IssuedCard
                    .Any(pdp => pdp.InternalDivisionId == internalDivisionId.Value));

            if (organizationalUnitId.HasValue)
                q = q.Where(p => p.IssuedCard
                    .Any(pdp => pdp.InternalDivision.OrganizationalUnitId == organizationalUnitId.Value));

            if (profileId.HasValue)
                q = q.Where(p => p.IssuedCard
                    .Any(pdp => pdp.ProfileId == profileId.Value));

            // Total antes de paginar
            int total = await q.CountAsync(ct);

            // Paginación defensiva
            page = page <= 0 ? 1 : page;
            pageSize = pageSize <= 0 ? 20 : pageSize;
            int skip = (page - 1) * pageSize;

            // Orden básico (alfabético por nombre completo)
            var items = await q
                .OrderBy(p => p.FirstName)
                .ThenBy(p => p.LastName)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync(ct);

            return (items, total);
        }
        public async Task<Person?> FindByDocumentAsync(string documentNumber)
        {
            return await _context.People
                .FirstOrDefaultAsync(p => !p.IsDeleted && p.DocumentNumber == documentNumber);
        }

        public async Task<Person?> GetByDocumentAsync(string documentNumber)
        {
            return await _context.People
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.DocumentNumber == documentNumber && !p.IsDeleted);
        }
    }
}
