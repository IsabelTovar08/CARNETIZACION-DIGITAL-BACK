using System;
using System.Collections.Generic;
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
        Task<Person?> FindByIdentification(string identification);
        Task<(Person Person, User User)> SavePersonAndUser(Person person, User user);
        Task<Person?> GetPersonInfo(int id);

        Task<PersonOrganizationalInfoDto?> GetOrganizationalInfo(int personId);
        Task<Person?> GetPersonByUserIdAsync(int userId);

        /// <summary>
        /// Devuelve la entidad Person con las relaciones necesarias (DocumentType, BloodType, City, IssuedCard...).
        /// Útil para mapear a DTOs inmediatamente después de crear/actualizar.
        /// </summary>
        Task<Person?> GetByIdWithRelationsAsync(int id);

        /// <summary>
        /// Filtro + paginación:
        /// - Solo personas no eliminadas
        /// - Con al menos un PersonDivisionProfile que tenga al menos un Card (carnet)
        /// - Filtros opcionales por división interna, unidad organizativa y perfil
        /// </summary>
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