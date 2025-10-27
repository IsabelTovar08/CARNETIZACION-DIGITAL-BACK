using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Business.Interfases;
using Entity.DTOs.ModelSecurity.Request;
using Entity.DTOs.ModelSecurity.Response;
using Entity.DTOs.Specifics;
using Entity.Models.ModelSecurity;

namespace Business.Interfaces.Security
{
    public interface IPersonBusiness : IBaseBusiness<Person, PersonDtoRequest, PersonDto>
    {
        Task<PersonRegistrerDto> SavePersonAndUser(PersonRegistrer personUser);

        /// <summary>
        /// Upsert person's photo and persist URL/path on the entity.
        /// </summary>
        Task<(string PublicUrl, string StoragePath)> UpsertPersonPhotoAsync(
            int personId,
            Stream fileStream,
            string contentType,
            string originalFileName);

        Task<PersonInfoDto?> GetPersonInfoAsync(int id);

        // Devuelve los códigos orgánicos para la persona indicada, los cuales sirven para la organización de archivos
        Task<PersonOrganizationalInfoDto?> GetOrganizationalInfoAsync(int personId);
        Task<PersonDto?> GetMyPersonAsync();
        //Task<PersonDto?> GetPersonByUserIdAsync(int userId);

        /// <summary>
        /// Filtro + paginación de personas:
        /// - Solo personas no eliminadas
        /// - Con al menos un PersonDivisionProfile que tenga asociado un Card (carnet)
        /// - Filtros opcionales por división interna, unidad organizativa y perfil
        /// </summary>
        /// 

        
        Task<PersonDto?> GetCurrentPersonAsync();

        //  Evita que el documento se repita
        Task<PersonDto?> FindByDocumentAsync(string documentNumber);


        Task<(IList<PersonDto> Items, int Total)> QueryWithFiltersAsync(
            int? internalDivisionId,
            int? organizationalUnitId,
            int? profileId,
            int page,
            int pageSize,
            CancellationToken ct = default
        );
    }
}
