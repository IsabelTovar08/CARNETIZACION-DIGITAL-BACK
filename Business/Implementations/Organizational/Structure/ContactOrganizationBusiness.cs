using AutoMapper;
using Business.Classes.Base;
using Business.Implementations.Notifications;
using Business.Interfaces.Auth;
using Business.Interfaces.Notifications;
using Business.Interfaces.Organizational.Structure;
using Business.Interfaces.Security;
using ClosedXML.Excel;
using Data.Implementations.Organizational.Structure;
using Data.Interfases;
using Data.Interfases.Organizational.Structure;
using Data.Interfases.Security;
using Entity.DTOs.ModelSecurity.Request;
using Entity.DTOs.ModelSecurity.Response;
using Entity.DTOs.Notifications;
using Entity.DTOs.Notifications.Request;
using Entity.DTOs.Organizational.Structure.Request;
using Entity.DTOs.Organizational.Structure.Response;
using Entity.Models.Organizational.Structure;
using Infrastructure.Notifications.Interfases;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Enums.Specifics;
using Utilities.Exeptions;
using Utilities.Notifications.Implementations;
using Utilities.Notifications.Implementations.Templates.Email;

namespace Business.Implementations.Organizational.Structure
{
    public class ContactOrganizationBusiness
        : BaseBusiness<ContactOrganization, ContactOrganizationDtoRequest, ContactOrganizationDtoResponse>,
          IContactOrganizationBusiness
    {
        private readonly INotificationBusiness _notificationBusiness;
        private readonly IPersonBusiness _personBusiness;
        private readonly INotify _notifier;
        private readonly IOrganizationBusiness _organizationBusiness;
        private readonly IPersonData _personData;

        public ContactOrganizationBusiness(
            IContactOrganizationData data,
            ILogger<ContactOrganization> logger,
            IMapper mapper,
            INotificationBusiness notificationBusiness,
            IPersonBusiness personBusiness,
            IOrganizationBusiness organizationBusiness,
            INotify notifier,
            IPersonData personData
        ) : base(data, logger, mapper)
        {
            _notificationBusiness = notificationBusiness;
            _personBusiness = personBusiness;
            _organizationBusiness = organizationBusiness;
            _notifier = notifier;
            _personData = personData;
        }

        /// <summary>
        /// Crea una nueva solicitud de contacto de organización y notifica al administrador.
        /// </summary>
        public async Task<ContactOrganizationDtoResponse> CreateContactRequest(ContactOrganizationDtoRequest dto)
        {
            try
            {
                Validate(dto);

                // 1️⃣ Buscar si la persona ya existe
                var existingPerson = await _personData.GetByDocumentAsync(dto.DocumentNumber);
                PersonDto personCreated;

                if (existingPerson != null)
                {
                    // Ya existe, solo mapear a DTO
                    personCreated = _mapper.Map<PersonDto>(existingPerson);
                }
                else
                {
                    // Crear nueva persona normalmente
                    var personDto = _mapper.Map<PersonDtoRequest>(dto.Person);
                    personCreated = await _personBusiness.Save(personDto);
                }

                // 2️⃣ Crear el contacto
                var contact = _mapper.Map<ContactOrganization>(dto);
                contact.PersonId = personCreated.Id;
                contact.Person = null; // evitar reinsertar persona
                contact.IsApproved = false;
                contact.RequestDate = DateTime.UtcNow;

                await _data.SaveAsync(contact);

                // 3️⃣ Enviar notificación (si tienes método ya implementado)
                await SendOrganizationNotification(contact, personCreated);

                return _mapper.Map<ContactOrganizationDtoResponse>(contact);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear solicitud de organización.");
                throw;
            }
        }

        private async Task SendOrganizationNotification(ContactOrganization contact, PersonDto person)
        {
            try
            {
                var model = new Dictionary<string, object>
                {
                    ["welcome_title"] = "Tu solicitud fue recibida",
                    ["company_name"] = contact.CompanyName,
                    ["user_name"] = $"{person.FirstName} {person.LastName}",
                    ["email"] = contact.Email,
                    ["welcome_message"] = "Tu solicitud está en proceso de revisión. Te notificaremos pronto."
                };

                string html = await EmailTemplates.RenderAsync("RequestOrganization.html", model);
                await _notifier.NotifyAsync("email", contact.CompanyEmail ?? contact.Email, "Solicitud recibida", html);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "No se pudo enviar la notificación de solicitud.");
            }
        }




        /// <summary>
        /// Aprueba o rechaza una solicitud de contacto.
        /// </summary>
        public async Task ApproveContactAsync(int id, bool approved)
        {
            //Buscar la solicitud
            var contact = await _data.GetByIdAsync(id) 
                ?? throw new Exception("Solicitud no encontrada");

            contact.IsApproved = approved;
            await _data.UpdateAsync(contact);

            //Validar la persona asociada
            if (contact.PersonId == null)
                throw new Exception("No hay persona asociada.");

            var person = await _personBusiness.GetById(contact.PersonId.Value)
                          ?? throw new Exception("Persona no encontrada.");

            person.IsDeleted = !approved;
            await _personBusiness.Update(_mapper.Map<PersonDtoRequest>(person));

            if (approved)
            {
                var newOrg = new Organization
                {
                    Name = contact.CompanyName,
                    //Email = contact.CompanyEmail ?? contact.Email, // si no tiene companyEmail, usa el del asesor
                    IsDeleted = false,
                    CreateAt = DateTime.UtcNow,
                    UpdateAt = DateTime.UtcNow
                };

                await _organizationBusiness.Save(new OrganizationDtoRequest
                {
                    Name = newOrg.Name,
                    
                });
            }

            //Construir el modelo para la plantilla
            var model = new Dictionary<string, object>
            {
                ["welcome_title"] = approved ? "Tu empresa ha sido aprobada" : "Tu solicitud fue rechazada",
                ["welcome_message"] = approved
                    ? "Ya puedes ingresar al sistema para continuar con el proceso."
                    : "Comunícate con soporte si consideras que hubo un error.",
                ["company_name"] = contact.CompanyName ?? "Empresa desconocida",
                ["email"] = contact.Email ?? "N/A",
                ["user_name"] = contact.AdvisorName ?? "Usuario",
                ["advisor_role"] = contact.AdvisorRole ?? "",
                ["year"] = DateTime.Now.Year
            };

            //Renderizar plantilla HTML
            string subject = approved ? "✅ Solicitud aprobada" : "❌ Solicitud rechazada";
            string htmlTemplate = await EmailTemplates.RenderAsync("RequestOrganization.html", model);

            await _notifier.NotifyAsync("email", contact.Email, subject, htmlTemplate);

            if (!string.IsNullOrEmpty(contact.CompanyEmail))
                await _notifier.NotifyAsync("email", contact.CompanyEmail, subject, htmlTemplate);
        }
    }
}

