using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Auth;
using Business.Interfaces.Security;
using Business.Interfaces.Storage;
using Business.Interfases.Storage;
using ClosedXML.Excel;
using Data.Classes.Specifics;
using Data.Interfases;
using Data.Interfases.Security;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using Entity.DTOs;
using Entity.DTOs.ModelSecurity.Request;
using Entity.DTOs.ModelSecurity.Response;
using Entity.DTOs.Specifics;
using Entity.Models;
using Entity.Models.ModelSecurity;
using Infrastructure.Notifications.Interfases;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utilities.Exeptions;
using Utilities.Notifications.Implementations.Templates.Email;
using ValidationException = Utilities.Exeptions.ValidationException;

namespace Business.Classes
{
    public class PersonBusiness : BaseBusiness<Person, PersonDtoRequest, PersonDto>, IPersonBusiness
    {
        private readonly IPersonData _personData;
        private readonly INotify _notificationSender;
        private readonly IUserRoleBusiness _userRolBusiness;
        private readonly IUserData _userData;
        private readonly IAssetUploader _assetUploader;
        private readonly ICurrentUser _currentUser;

        public PersonBusiness(IPersonData personData, ILogger<Person> logger, IMapper mapper, INotify messageSender, IUserRoleBusiness userRolBusiness,IUserData userData ,IAssetUploader assetUploader, ICurrentUser currentUser)
            : base(personData, logger, mapper)
        {
            _notificationSender = messageSender;
            _personData = personData;
            _userRolBusiness = userRolBusiness;
            _assetUploader = assetUploader;
            _currentUser = currentUser;
            _userData = userData;
        }

        public override async Task ValidateAsync(Person entity)
        {
            var errors = new List<(string Field, string Message)>();

            if (!string.IsNullOrWhiteSpace(entity.Phone))
            {
                if (await _data.ExistsByAsync(x => x.Phone, entity.Phone))
                    errors.Add(("Phone ", "El teléfono ya está registrado."));
            }
            if (!string.IsNullOrWhiteSpace(entity.Email))
            {
                if (await _data.ExistsByAsync(x => x.Email, entity.Email))
                    errors.Add(("Email ", "El Email ya está registrado."));
            }
            if (!string.IsNullOrWhiteSpace(entity.DocumentNumber))
            {
                if (await _data.ExistsByAsync(x => x.DocumentNumber, entity.DocumentNumber))
                    errors.Add(("DocumentNumber ", "El DocumentNumber ya está registrado."));
            }
            if ((int)entity.DocumentTypeId <= 0)
                errors.Add(("Tipo de documento", "Debe seleccionar un Tipo de documento válido."));
            if ((int)entity.BloodTypeId <= 0)
                errors.Add(("Tipo de sangre", "Debe seleccionar un Tipo de sangre válido."));
            if ((int)entity.CityId <= 0)
                errors.Add(("Ciudad", "Debe seleccionar una ciudad válida."));
            if (errors.Count > 0)
            {
                var combined = string.Join(" | ", errors.Select(e => $"{e.Field}: {e.Message}"));
                throw new ValidationException(errors[0].Field, combined);
            }
        }

        protected void Validate(PersonDtoRequest person)
        {
            if (person == null)
                throw new ValidationException("la persona no puede ser nula.");

            if (string.IsNullOrWhiteSpace(person.FirstName))
                throw new ValidationException("El Primer Nombre de la persona es obligatorio.");
            if (string.IsNullOrWhiteSpace(person.LastName))
                throw new ValidationException("El Primer Apellido de la persona es obligatorio.");
            if (string.IsNullOrWhiteSpace(person.DocumentNumber))
                throw new ValidationException("El número de identificación de la persona es obligatorio.");
        }

        /// <summary>
        /// Crea una nueva entidad.
        /// </summary>
        public override async Task<PersonDto> Save(PersonDtoRequest personDTO)
        {
            try
            {
                Validate(personDTO);

                await EnsureIdentificationIsUnique(personDTO.DocumentNumber);

                var person = _mapper.Map<Person>(personDTO);

                await ValidateAsync(person);

                var created = await _data.SaveAsync(person);
                await SendWelcomeNotifications(person, null);

                _logger.LogInformation("Persona registrada correctamente con ID {Id}", created.Id);

                return _mapper.Map<PersonDto>(created);
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el usuario.");
                throw new ExternalServiceException("Base de datos", "No se pudo crear el usuario.");
            }
        }

        private async Task EnsureIdentificationIsUnique(string identification)
        {
            if (await _personData.FindByIdentification(identification) != null)
            {
                throw new ValidationException("Ya existe una persona con este número de identificación.");
            }
        }

        public async Task<PersonRegistrerDto> SavePersonAndUser(PersonRegistrer personUser)
        {
            Validate(personUser.Person);
            await EnsureIdentificationIsUnique(personUser.Person.DocumentNumber);

            Person personEntity = _mapper.Map<Person>(personUser.Person);
            User userEntity = _mapper.Map<User>(personUser.User);
            (Person Person, User User) result = await _personData.SavePersonAndUser(personEntity, userEntity);

            // devuelve si se envió o no
            bool emailSent = _ = await SendWelcomeNotifications(result.Person, userEntity);
            _ = await AsignarRol(userEntity.Id);

            return (
                new PersonRegistrerDto
                {
                    Person = _mapper.Map<PersonDto>(result.Person),
                    User = _mapper.Map<UserDTO>(result.User),
                    EmailSent = emailSent
                }
            );
        }

        private async Task<bool> AsignarRol(int userId)
        {
            try
            {
                Console.WriteLine("Inicar a asignar rol.");
                UserRoleDtoRequest userRol = new UserRoleDtoRequest()
                {
                    Id = 0,
                    UserId = userId,
                    RolId = 6 // Rol estándar por defecto
                };
                await _userRolBusiness.Save(userRol);
                Console.WriteLine("Rol asignado.");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al asignar el rol por defecto para el nuevo usuario.");
                return false;
            }
        }

        // Método privado reutilizable
        private async Task<bool> SendWelcomeNotifications(Person person, User? user)
        {
            if (string.IsNullOrWhiteSpace(person?.Email))
                return false;

            try
            {
                var model = new Dictionary<string, object>
                {
                    ["user_name"] = $"{person.FirstName} {person.LastName}".Trim(),
                    ["email"] = person.Email,
                    ["CompanyName"] = "Sistema de Carnetización Digital",
                    ["Year"] = DateTime.Now.Year,
                    ["LoginUrl"] = "https://carnet.tuempresa.com",
                    ["ActionUrl"] = "https://carnet.tuempresa.com/login"
                };
                if (user != null)
                {
                    if (!string.IsNullOrEmpty(user.Password))
                    {
                        model["temp_password"] = user.Password;
                    }
                }

                var html = await EmailTemplates.RenderAsync("Welcome.html", model);

                await _notificationSender.NotifyAsync(
                    "email",
                    person.Email,
                    "¡Bienvenido!",
                    html
                );

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "No se pudo enviar email de bienvenida a {Email}", person.Email);
                return false; // no rompas el flujo de creación
            }
        }

        public async Task<PersonInfoDto?> GetPersonInfoAsync(int id)
        {
            Person? person = await _personData.GetPersonInfo(id);
            if (person is null) return null;

            PersonInfoDto dto = _mapper.Map<PersonInfoDto>(person);

            return dto;
        }

        public async Task<PersonOrganizationalInfoDto?> GetOrganizationalInfoAsync(int personId)
        {
            if (personId <= 0)
                throw new ArgumentOutOfRangeException(nameof(personId), "El id de persona debe ser mayor que cero.");

            try
            {
                var dto = await _personData.GetOrganizationalInfo(personId);

                //  Si no hay perfil seleccionado o no existe la persona, regresa null.
                if (dto == null)
                {
                    _logger.LogInformation("Person {PersonId} no encontrada o sin perfil seleccionado.", personId);
                    return null;
                }

                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo información organizacional de la persona {PersonId}", personId);
                throw;
            }
        }

        // Método público: upsert foto de persona 
        public async Task<(string PublicUrl, string StoragePath)> UpsertPersonPhotoAsync(
            int personId,
            Stream fileStream,
            string contentType,
            string fileName)
        {
            // 1) Cargar persona
            var person = await _personData.GetByIdAsync(personId)
                ?? throw new KeyNotFoundException($"La persona {personId} no fue encontrada");

            // 2) Calcular ruta (segmentos) para esta entidad
            var pathParts = await BuildPersonPhotoPathPartsAsync(personId);

            // 3) Subir y reemplazar, delegando en el servicio genérico
            var (publicUrl, storagePath) = await _assetUploader.UpsertAsync(
                pathParts,
                person.PhotoPath,             // previous path
                fileStream,
                contentType,
                fileName
            );

            // 4) Persistir cambios en entidad
            person.PhotoUrl = publicUrl;
            person.PhotoPath = storagePath;
            await _personData.UpdateAsync(person);

            return (publicUrl, storagePath);
        }

        // Helper para armar los segmentos de ruta
        private async Task<IReadOnlyList<string?>> BuildPersonPhotoPathPartsAsync(int personId)
        {
            // Pueden existir personas sin perfil org seleccionada:
            // usa los códigos si existen; si no, solo people/{personId}
            var info = await GetOrganizationalInfoAsync(personId);

            var parts = new List<string?>
            {
                "people",
                info?.OrganizationCode,
                info?.OrganizationUnitCode,
                info?.InternalDivissionCode,
                personId.ToString()
            };

            return parts;
        }

        public async Task<PersonDto?> GetMyPersonAsync()
        {
            var userIdStr = _currentUser.UserIdRaw;
            if (userIdStr == "unknown")
                throw new UnauthorizedAccessException("Usuario no identificado.");

            int userId = int.Parse(userIdStr);

            //  obtiene la persona asociada al User actual
            var person = await _personData.GetPersonByUserIdAsync(userId);
            if (person == null)
                throw new KeyNotFoundException("No se encontró la persona asociada al usuario actual.");

            return _mapper.Map<PersonDto>(person);
        }

        // 🔹 NUEVO MÉTODO: Filtro + paginación
        public async Task<(IList<PersonDto> Items, int Total)> QueryWithFiltersAsync(
            int? internalDivisionId,
            int? organizationalUnitId,
            int? profileId,
            int page,
            int pageSize,
            CancellationToken ct = default)
        {
            var (entities, total) = await _personData.QueryWithFiltersAsync(
                internalDivisionId, organizationalUnitId, profileId, page, pageSize, ct);

            var items = entities.Select(e => _mapper.Map<PersonDto>(e)).ToList();

            // 3. Agregar UserId (consulta aparte)
            foreach (var person in items)
            {
                person.UserId = await _userData.GetUserIdByPersonIdAsync(person.Id);
            }
            return (items, total);
        }
    
        public async Task<PersonDto?> GetCurrentPersonAsync()
        {
            try
            {
                // 1) Validar user autenticado
                if (_currentUser == null || _currentUser.UserId <= 0)
                {
                    _logger.LogDebug("Usuario no autenticado o claim inválido. UserIdRaw={UserIdRaw}", _currentUser?.UserIdRaw);
                    return null; // o lanzar UnauthorizedAccessException según convención de tu app
                }

                // 2) Buscar la persona asociada al User (la consulta al back queda en la capa Data)
                var person = await _personData.GetPersonByUserIdAsync(_currentUser.UserId);
                if (person == null)
                {
                    _logger.LogInformation("No se encontró persona asociada al usuario {UserId}", _currentUser.UserId);
                    return null;
                }

                // 3) Mapear a DTO usando AutoMapper (ya registrado)
                var dto = _mapper.Map<PersonDto>(person);
                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo persona actual para UserIdRaw={UserIdRaw}", _currentUser?.UserIdRaw);
                throw; // deja que el middleware lo maneje (o encapsula en una excepción controlada)
            }
        }
    }

}
