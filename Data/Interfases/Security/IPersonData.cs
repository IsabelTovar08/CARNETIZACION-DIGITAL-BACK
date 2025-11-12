using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Entity.DTOs.ModelSecurity.Request;
using Entity.DTOs.Specifics;
using Entity.Models;
using Entity.Models.ModelSecurity;

namespace Data.Interfases.Security
{
    public interface IPersonData : ICrudBase<Person>
    {
        /// <summary>
        /// Devuelve un IQueryable para consultas personalizadas (por ejemplo desde AttendanceBusiness)
        /// </summary>
        IQueryable<Person> GetQueryable();

        Task<Person?> FindByIdentification(string identification);
        Task<(Person Person, User User)> SavePersonAndUser(Person person, User user);
        Task<Person?> GetPersonInfo(int id);
        Task<PersonOrganizationalInfoDto?> GetOrganizationalInfo(int personId);
        Task<Person?> GetPersonByUserIdAsync(int userId);
        Task<(IList<Person> Items, int Total)> QueryWithFiltersAsync(
            int? internalDivisionId,
            int? organizationalUnitId,
            int? profileId,
            int page,
            int pageSize,
            CancellationToken ct = default
        );
    }
}
