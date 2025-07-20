
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Business.Classes.Base;
using Data.Classes.Specifics;
using Data.Interfases;
using Entity.DTOs;
using Entity.Models;
using Entity.Models.ModelSecurity;
using Infrastructure.Notifications.Interfases;
using Microsoft.Extensions.Logging;
using Utilities.Exeptions;
using ValidationException = Utilities.Exeptions.ValidationException;

namespace Business.Classes
{
    public class PersonBusiness : BaseBusiness<Person, PersonDto>
    {
        public readonly PersonData _personData;
        private readonly INotify _notificationSender;
        public PersonBusiness(ICrudBase<Person> data, ILogger<Person> logger, IMapper mapper, PersonData personData, INotify messageSender) : base(data, logger, mapper)
        {
            _personData = personData;
            _notificationSender = messageSender;
        }

        protected  void Validate(PersonDto person)
        {
            if (person == null)
                throw new ValidationException("la persona no puede ser nula.");

            if (string.IsNullOrWhiteSpace(person.FirstName))
                throw new ValidationException("El Primer Nombre de la persona es obligatorio.");
            if (string.IsNullOrWhiteSpace(person.LastName))
                throw new ValidationException("El Primer Apellido de la persona es obligatorio.");
            if (string.IsNullOrWhiteSpace(person.Identification))
                throw new ValidationException("El número de identificación de la persona es obligatorio.");
        }

        /// <summary>
        /// Crea una nueva entidad.
        /// </summary>
        public override async Task<PersonDto> Save(PersonDto personDTO)
        {
            try
            {
                //Validate(personDTO);
                await EnsureIdentificationIsUnique(personDTO.Identification);

                var person = _mapper.Map<Person>(personDTO);

                var created = await _data.SaveAsync(person);
                await _notificationSender.NotifyAsync(
                        "whatsapp",
                       + 57 +person.Phone, // debe incluir el código de país, ej. "+51987654321"
                       "¡Bienvenido!",
                       "Tu cuenta ha sido creada con éxito en nuestra app."
                   );
                await _notificationSender.NotifyAsync(
                        "telegram",
                       +57 + person.Phone, // debe incluir el código de país, ej. "+51987654321"
                       "¡Bienvenido!",
                       "Tu cuenta ha sido creada con éxito en nuestra app."
                   );

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


        private async Task EnsureIdentificationIsUnique(string email)
        {
            if (await _personData.FindByIdentification(email) != null)
            {
                throw new ValidationException("Ya existe una persona con este número de identificación.");
            }
        }

    }
}
