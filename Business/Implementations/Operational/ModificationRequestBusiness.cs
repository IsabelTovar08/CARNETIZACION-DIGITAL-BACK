using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Auth;
using Business.Interfaces.Notifications;
using Business.Interfaces.Operational;
using Business.Interfaces.Security;
using Business.Services.CodeGenerator;
using Business.Services.Notifications;
using Data.Interfases;
using Data.Interfases.Operational;
using Data.Interfases.Transaction;
using Entity.DTOs.ModelSecurity.Request;
using Entity.DTOs.ModelSecurity.Response;
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.Enums.Extensions;
using Entity.Enums.Specifics;
using Entity.Models.Operational;
using Microsoft.Extensions.Logging;

namespace Business.Implementations.Operational
{
    /// <summary>
    /// Lógica de negocio para la gestión de solicitudes de modificación de datos.
    /// </summary>
    public class ModificationRequestBusiness
        : BaseBusiness<ModificationRequest, ModificationRequestDto, ModificationRequestResponseDto>, IModificationRequestBusiness
    {
        protected new readonly IModificationRequestData _data;
        protected readonly ICurrentUser _currentUser;
        protected readonly IUserBusiness _userBusiness;
        protected readonly IPersonBusiness _personBusiness;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationBusiness _notificationBusiness;

        public ModificationRequestBusiness(
            IModificationRequestData data,
            ILogger<ModificationRequest> logger,
            IMapper mapper,
            ICurrentUser currentUser,
            IUserBusiness userBusiness,
            IPersonBusiness personBusiness,
            IUnitOfWork unitOfWork,
            INotificationBusiness notificationBusiness,
            ICodeGeneratorService<ModificationRequest>? codeService = null)
            : base(data, logger, mapper, codeService)
        {
            _data = data;
            _currentUser = currentUser;
            _userBusiness = userBusiness;
            _personBusiness = personBusiness;
            _unitOfWork = unitOfWork;
            _notificationBusiness = notificationBusiness;
        }

        /// <summary>
        /// Guarda una nueva solicitud y notifica a los administradores.
        /// </summary>
        public override async Task<ModificationRequestResponseDto> Save(ModificationRequestDto entity)
        {
            int userId = _currentUser.UserId;
            entity.UserId = userId;

            // Guardar solicitud
            ModificationRequestResponseDto saved = await base.Save(entity);

            // Notificar a los administradores
            await NotifyAdminsOnCreationAsync(saved);

            return saved;
        }

        /// <summary>
        /// Obtiene todas las solicitudes realizadas por el usuario autenticado.
        /// </summary>
        public async Task<IEnumerable<ModificationRequestResponseDto>> GetByCurrentUserAsync()
        {
            int userId = _currentUser.UserId;
            var list = await _data.GetByUserIdAsync(userId);
            return list.Select(r => _mapper.Map<ModificationRequestResponseDto>(r));
        }

        /// <inheritdoc/>
        public async Task<bool> ApproveRequestAsync(int requestId, string? approvalMessage = null)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // 1️⃣ Obtener la solicitud
                var request = await _data.GetByIdAsync(requestId);
                if (request == null)
                    throw new InvalidOperationException("Solicitud no encontrada.");

                if (request.Status != ModificationRequestStatus.Pending)
                    throw new InvalidOperationException("La solicitud ya fue procesada.");

                if (string.IsNullOrWhiteSpace(request.NewValue))
                    throw new InvalidOperationException("El nuevo valor no puede estar vacío.");

                // 2️⃣ Obtener el usuario y su persona asociada
                var user = await _userBusiness.GetById(request.UserId);
                if (user == null)
                    throw new InvalidOperationException("No se encontró el usuario asociado a la solicitud.");

                var person = await _personBusiness.GetById(user.PersonId);
                if (person == null)
                    throw new InvalidOperationException("No se encontró la persona asociada al usuario.");

                // 3️⃣ Crear un nuevo DTO de actualización
                var personUpdateDto = new PersonDtoRequest
                {
                    Id = person.Id,
                    FirstName = person.FirstName,
                    MiddleName = person.MiddleName,
                    LastName = person.LastName,
                    SecondLastName = person.SecondLastName,
                    DocumentTypeId = person.DocumentTypeId,
                    DocumentNumber = person.DocumentNumber,
                    BloodTypeId = person.BloodTypeId,
                    Phone = person.Phone,
                    Email = person.Email,
                    Address = person.Address,
                    CityId = person.CityId > 0 ? person.CityId : 1
                };

                // 4️⃣ Aplicar cambio según campo
                switch (request.Field)
                {
                    case ModificationField.FirstName: personUpdateDto.FirstName = request.NewValue; break;
                    case ModificationField.MiddleName: personUpdateDto.MiddleName = request.NewValue; break;
                    case ModificationField.LastName: personUpdateDto.LastName = request.NewValue; break;
                    case ModificationField.SecondLastName: personUpdateDto.SecondLastName = request.NewValue; break;
                    case ModificationField.DocumentNumber: personUpdateDto.DocumentNumber = request.NewValue; break;
                    case ModificationField.Email:
                        if (!request.NewValue.Contains("@"))
                            throw new InvalidOperationException("El correo electrónico no es válido.");
                        personUpdateDto.Email = request.NewValue;
                        break;
                    case ModificationField.Phone:
                        if (request.NewValue.Length < 7)
                            throw new InvalidOperationException("Número de teléfono inválido.");
                        personUpdateDto.Phone = request.NewValue;
                        break;
                    case ModificationField.Address: personUpdateDto.Address = request.NewValue; break;
                    case ModificationField.DocumentTypeId:
                        if (int.TryParse(request.NewValue, out var docTypeId))
                            personUpdateDto.DocumentTypeId = docTypeId;
                        break;
                    case ModificationField.BloodTypeId:
                        if (int.TryParse(request.NewValue, out var bloodTypeId))
                            personUpdateDto.BloodTypeId = bloodTypeId;
                        break;
                    case ModificationField.CityId:
                        if (int.TryParse(request.NewValue, out var cityId))
                            personUpdateDto.CityId = cityId;
                        break;
                    case ModificationField.PhotoUrl:
                        break;
                    default:
                        throw new InvalidOperationException("Campo de modificación no válido.");
                }

                // 5️⃣ Actualizar persona
                await _personBusiness.Update(personUpdateDto);

                // 6️⃣ Actualizar solicitud
                request.Status = ModificationRequestStatus.Approved;
                request.UpdatedById = _currentUser.UserId;
                request.UpdateAt = DateTime.UtcNow;
                request.Message = approvalMessage;

                await _data.UpdateAsync(request);

                // 7️⃣ Notificar al usuario
                await NotifyUserOnResultAsync(request, NotificationTemplateType.ModificationApproved);

                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogError(ex, "Error al aprobar la solicitud de modificación (Id: {Id})", requestId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> RejectRequestAsync(int requestId, string? rejectionMessage = null)
        {
            var request = await _data.GetByIdAsync(requestId);
            if (request == null)
                throw new InvalidOperationException("Solicitud no encontrada.");

            if (request.Status != ModificationRequestStatus.Pending)
                throw new InvalidOperationException("La solicitud ya fue procesada.");

            request.Status = ModificationRequestStatus.Rejected;
            request.Message = rejectionMessage ?? "Solicitud rechazada.";
            request.UpdatedById = _currentUser.UserId;
            request.UpdateAt = DateTime.UtcNow;

            await _data.UpdateAsync(request);

            // 🔹 Notificar al usuario solicitante
            await NotifyUserOnResultAsync(request, NotificationTemplateType.ModificationRejected);

            return true;
        }

        /// <summary>
        /// 🔹 Notifica a los administradores cuando un usuario crea una solicitud.
        /// </summary>
        private async Task NotifyAdminsOnCreationAsync(ModificationRequestResponseDto request)
        {
            try
            {
                var user = await _userBusiness.GetById(request.UserId);
                var person =  await _personBusiness.GetById(user.PersonId);

                var fullName = person != null
                    ? $"{person.FirstName} {person.LastName}".Trim()
                    : $"Usuario #{user.Id}";

                var admins = await _userBusiness.GetUsersByRoleAsync("OrgAdmin");

                foreach (var admin in admins)
                {
                    var dto = NotificationFactory.Create(
                        NotificationTemplateType.ModificationRequest,
                        fullName,
                        request.FieldName
                    );

                    await _notificationBusiness.CreateAndSendAsync(dto);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error notificando a administradores sobre la solicitud {Id}", request.Id);
            }
        }

        /// <summary>
        /// 🔹 Notifica al usuario solicitante sobre el resultado (aprobado o rechazado).
        /// </summary>
        private async Task NotifyUserOnResultAsync(ModificationRequest request, NotificationTemplateType type)
        {
            try
            {
                var user = await _userBusiness.GetById(request.UserId);
                if (user == null)
                    return;

                await _notificationBusiness.SendTemplateAsync(
                    type,
                    request.Field.GetDisplayName(),
                    request.Message ?? ""
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error notificando resultado de solicitud {Id}", request.Id);
            }
        }
    }
}
