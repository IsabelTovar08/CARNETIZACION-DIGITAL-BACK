using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        /// Devuelve los datos (DTO) de la persona asociada al token del usuario actual.
        /// Retorna null si no hay usuario autenticado o la persona no existe.
        /// </summary>
        Task<PersonDto?> GetCurrentPersonAsync();

        //  Evita que el documento se repita
        Task<PersonDto?> FindByDocumentAsync(string documentNumber);
    }
}
