
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Security;
using ClosedXML.Excel;
using Data.Classes.Specifics;
using Data.Interfases;
using Data.Interfases.Security;
using DocumentFormat.OpenXml.Spreadsheet;
using Entity.DTOs;
using Entity.DTOs.ModelSecurity.Request;
using Entity.DTOs.ModelSecurity.Response;
using Entity.Models;
using Entity.Models.ModelSecurity;
using Infrastructure.Notifications.Interfases;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Utilities.Exeptions;
using Utilities.Notifications.Implementations.Templates.Email;
using ValidationException = Utilities.Exeptions.ValidationException;

namespace Business.Classes
{
    public class PersonBusiness : BaseBusiness<Person, PersonDtoRequest, PersonDto>, IPersonBusiness
    {
        private readonly IPersonData _personData;
        private readonly INotify _notificationSender;
        private readonly IUserBusiness _userBusiness;
        private readonly IUserRoleBusiness _userRolBusiness;

        public PersonBusiness(IPersonData personData, ILogger<Person> logger, IMapper mapper, INotify messageSender, IUserBusiness userBusiness, IUserRoleBusiness userRolBusiness) : base(personData, logger, mapper)
        {
            _notificationSender = messageSender;
            _userBusiness = userBusiness;
            _personData = personData;
            _userRolBusiness = userRolBusiness;
        }

        public override async Task ValidateAsync(Person entity)
        {
            var errors = new List<(string Field, string Message)>();

            if (!string.IsNullOrWhiteSpace(entity.Phone))
            {
                if (await _data.ExistsByAsync(x => x.Phone , entity.Phone ))
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
        protected  void Validate(PersonDtoRequest person)
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


        public async Task<(PersonRegistrerDto, bool?)> SavePersonAndUser(PersonRegistrer personUser)
        {
            Validate(personUser.Person);
            await EnsureIdentificationIsUnique(personUser.Person.DocumentNumber);

            var personEntity = _mapper.Map<Person>(personUser.Person);
            var userEntity = _mapper.Map<User>(personUser.User);
            var result = await _personData.SavePersonAndUser(personEntity, userEntity);

            // devuelve si se envió o no
            var emailSent = _=await SendWelcomeNotifications(result.Person, userEntity);
            _ = await AsignarRol(userEntity.Id);

            return (
                new PersonRegistrerDto
                {
                    Person = _mapper.Map<PersonDto>(result.Person),
                    User = _mapper.Map<UserDTO>(result.User)
                },
                emailSent
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
                if(user != null)
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

    }
}
