
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Security;
using Data.Classes.Specifics;
using Data.Interfases;
using Data.Interfases.Security;
using Entity.DTOs;
using Entity.DTOs.ModelSecurity.Request;
using Entity.DTOs.ModelSecurity.Response;
using Entity.Models;
using Entity.Models.ModelSecurity;
using Infrastructure.Notifications.Interfases;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Utilities.Exeptions;
using ValidationException = Utilities.Exeptions.ValidationException;

namespace Business.Classes
{
    public class PersonBusiness : BaseBusiness<Person, PersonDtoRequest, PersonDto>, IPersonBusiness
    {
        private readonly IPersonData _personData;
        private readonly INotify _notificationSender;
        private readonly IUserBusiness _userBusiness;
        public PersonBusiness(IPersonData personData, ILogger<Person> logger, IMapper mapper, INotify messageSender, IUserBusiness userBusiness) : base(personData, logger, mapper)
        {
            _notificationSender = messageSender;
            _userBusiness = userBusiness;
            _personData = personData;
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
                //Validate(personDTO);
                await EnsureIdentificationIsUnique(personDTO.DocumentNumber);

                var person = _mapper.Map<Person>(personDTO);

                var created = await _data.SaveAsync(person);
                await SendWelcomeNotifications(person.Email);

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

            var personEntity = _mapper.Map<Person>(personUser.Person);
            var userEntity = _mapper.Map<User>(personUser.User);

            var result = await _personData.SavePersonAndUser(personEntity, userEntity);

            // Enviar notificaciones una sola vez
            await SendWelcomeNotifications(result.Person.Phone);
            _logger.LogInformation("Persona y usuario registrados correctamente. PersonaID: {Id}", result.Person.Id);

            return new PersonRegistrerDto
            {
                Person = _mapper.Map<PersonDto>(result.Person),
                User = _mapper.Map<UserDTO>(result.User)
            };
        }

        // Método privado reutilizable
        private async Task SendWelcomeNotifications(string phoneNumber)
        {
            await _notificationSender.NotifyAsync(
                "whatsapp",
                +57 + phoneNumber,
                "¡Bienvenido!",
                "Tu cuenta ha sido creada con éxito en nuestra app."
            );
            await _notificationSender.NotifyAsync(
                "telegram",
                +57 + phoneNumber,
                "¡Bienvenido!",
                "Tu cuenta ha sido creada con éxito en nuestra app."
            );
        }

    }
}
