using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Auth;
using Business.Interfaces.Notifications;
using Business.Interfaces.Operational;
using Business.Interfaces.Security;
using Business.Interfases.Organizational.Location;
using Business.Services.CodeGenerator;
using Business.Services.Notifications;
using Data.Interfases.Operational;
using Data.Interfases.Transaction;
using Entity.DTOs.ModelSecurity.Request;
using Entity.DTOs.ModelSecurity.Response;
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.Enums.Extensions;
using Entity.Enums.Specifics;
using Entity.Models;
using Entity.Models.Operational;
using Infrastructure.Notifications.Interfases;
using Microsoft.Extensions.Logging;
using Utilities.Enums.Specifics;
using Utilities.Notifications.Implementations.Templates.Email;

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
        private readonly ICityBusiness _cityBusiness;
        private readonly INotify _notificationSender;


        public ModificationRequestBusiness(
            IModificationRequestData data,
            ILogger<ModificationRequest> logger,
            IMapper mapper,
            ICurrentUser currentUser,
            IUserBusiness userBusiness,
            IPersonBusiness personBusiness,
            IUnitOfWork unitOfWork,
            INotificationBusiness notificationBusiness,
            ICityBusiness cityBusiness,
            INotify notificationSender,
            ICodeGeneratorService<ModificationRequest>? codeService = null)
            : base(data, logger, mapper, codeService)
        {
            _data = data;
            _currentUser = currentUser;
            _userBusiness = userBusiness;
            _personBusiness = personBusiness;
            _unitOfWork = unitOfWork;
            _notificationBusiness = notificationBusiness;
            _cityBusiness = cityBusiness;
            _notificationSender = notificationSender;
        }

        /// <summary>
        /// Guarda una nueva solicitud y notifica a los administradores.
        /// </summary>
        public override async Task<ModificationRequestResponseDto> Save(ModificationRequestDto entity)
        {
            entity.UserId = _currentUser.UserId;

            // Obtener info del usuario y su persona
            UserDTO user = await _userBusiness.GetById(entity.UserId);
            PersonDto person = await _personBusiness.GetById(user.PersonId);

            // Convertir el valor enviado (int) al enum
            ModificationField field = (ModificationField)entity.Field;

            // Obtener OldValue usando ENUM tipado
            entity.OldValue = field switch
            {
                ModificationField.FirstName => person.FirstName,
                ModificationField.MiddleName => person.MiddleName,
                ModificationField.LastName => person.LastName,
                ModificationField.SecondLastName => person.SecondLastName,
                ModificationField.DocumentNumber => person.DocumentNumber,
                ModificationField.Email => person.Email,
                ModificationField.Phone => person.Phone,
                ModificationField.Address => person.Address,

                ModificationField.DocumentTypeId => person.DocumentTypeId?.ToString(),
                ModificationField.BloodTypeId => person.BloodTypeId?.ToString(),
                ModificationField.CityId => person.CityId.ToString(),

                ModificationField.PhotoUrl => person.PhotoUrl,

                _ => ""
            } ?? "";

            // Guardar solicitud
            ModificationRequestResponseDto saved = await base.Save(entity);

            // Notificar a los administradores
            await NotifyAdminsOnCreationAsync(saved);

            ModificationRequest requestEntity = _mapper.Map<ModificationRequest>(entity);

            // Notificar al usuario
            await NotifyUserOnResultAsync( requestEntity, NotificationTemplateType.ModificationSent);
            await SendModificationRequestEmail(requestEntity, "Pending");

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

                PersonDto? person = await _personBusiness.GetById(user.PersonId);
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
                    DocumentTypeId = (DocumentType?)person.DocumentTypeId,
                    DocumentNumber = person.DocumentNumber,
                    BloodTypeId = (BloodType?)person.BloodTypeId,
                    Phone = person.Phone,
                    Email = person.Email,
                    Address = person.Address,
                    CityId = person.CityId > 0 ? person.CityId : 1,
                    PhotoPath = person.PhotoPath,
                    PhotoUrl = person.PhotoUrl
                    
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
                        {
                            if (int.TryParse(request.NewValue, out int id))
                            {
                                personUpdateDto.DocumentTypeId = (DocumentType)id;
                            }
                            break;
                        }

                    case ModificationField.BloodTypeId:
                        {
                            if (int.TryParse(request.NewValue, out int id))
                            {
                                personUpdateDto.BloodTypeId = (BloodType)id;
                            }
                            break;
                        }

                    case ModificationField.CityId:
                        if (int.TryParse(request.NewValue, out var cityId))
                            personUpdateDto.CityId = cityId;
                        break;
                    case ModificationField.PhotoUrl:
                        {
                            string newUrl = await HandlePersonPhotoUpdateAsync(person, request.NewValue);
                            break;
                        }

                    default:
                        throw new InvalidOperationException("Campo de modificación no válido.");
                }

                // 5️⃣ Actualizar persona
                if (request.Field != ModificationField.PhotoUrl)
                {
                    await _personBusiness.Update(personUpdateDto);
                }

                // 6️⃣ Actualizar solicitud
                request.Status = ModificationRequestStatus.Approved;
                request.UpdatedById = _currentUser.UserId;
                request.UpdateAt = DateTime.UtcNow;
                request.Message = approvalMessage;

                await _data.UpdateAsync(request);

                // 7️⃣ Notificar al usuario
                await NotifyUserOnResultAsync(request, NotificationTemplateType.ModificationApproved);

                await _unitOfWork.CommitAsync();
                await SendModificationRequestEmail(request, "Approved");
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
            await SendModificationRequestEmail(request, "Rejected");
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

                IEnumerable<UserDTO> admins = await _userBusiness.GetUsersByRoleAsync("OrgAdmin");

                foreach (var admin in admins)
                {
                    var dto = await NotificationFactory.Create(
                        NotificationTemplateType.ModificationRequest,
                        fullName,
                        request.FieldName,
                        admin.Id,
                        request.Id
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
                await _notificationBusiness.SendTemplateAsync(
                    type,
                    request.Field.GetDisplayName(),
                    request.UserId,
                    request.Id,
                    request.Message ?? "No especificado"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error notificando resultado de solicitud {Id}", request.Id);
            }
        }


        /// <summary>
        /// Obtiene una solicitud por su ID.
        /// Incluye nombres legibles para enums y foreign keys.
        /// </summary>
        public override async Task<ModificationRequestResponseDto?> GetById(int id)
        {
            // Buscar la solicitud
            ModificationRequest? entity = await _data.GetByIdAsync(id);
            if (entity == null)
                return null;

            // Mapear a DTO
            ModificationRequestResponseDto dto = _mapper.Map<ModificationRequestResponseDto>(entity);

            // Asignar valores legibles
            ModificationField field = (ModificationField)dto.FieldId;

            dto.OldValueName = await GetReadableValueAsync(field, dto.OldValue, dto.Status, entity.UserId);
            dto.NewValueName = await GetReadableValueAsync(field, dto.NewValue, dto.Status, entity.UserId);

            return dto;
        }


        /// <summary>
        /// Obtiene todas las solicitudes de modificación.
        /// Incluye nombres legibles para enums y foreign keys.
        /// </summary>
        public override async Task<IEnumerable<ModificationRequestResponseDto>> GetAll()
        {
            // Obtener desde la capa Data
            IEnumerable<ModificationRequest> list = await _data.GetAllAsync();

            // Mapear entidades → DTOs
            IEnumerable<ModificationRequestResponseDto> response = _mapper.Map<IEnumerable<ModificationRequestResponseDto>>(list);

            // Construir valores legibles
            foreach (var item in response)
            {
                if (Enum.TryParse<ModificationField>(item.FieldId.ToString(), out var field))
                {
                    item.OldValueName = await GetReadableValueAsync(field, item.OldValue, item.Status, item.UserId);
                    item.NewValueName = await GetReadableValueAsync(field, item.NewValue, item.Status, item.UserId);
                }
            }

            return response;
        }


        /// <summary>
        /// Obtiene el nombre legible del valor, soporta enums y foreign keys.
        /// </summary>
        private async Task<string?> GetReadableValueAsync(ModificationField field, string rawValue, ModificationRequestStatus status,int userId)
        {
            if (string.IsNullOrWhiteSpace(rawValue))
                return null;

            switch (field)
            {
                case ModificationField.DocumentTypeId:
                    if (int.TryParse(rawValue, out int docId))
                        return ((DocumentType)docId).GetDisplayName();
                    return rawValue;

                case ModificationField.BloodTypeId:
                    if (int.TryParse(rawValue, out int bloodId))
                        return ((BloodType)bloodId).GetDisplayName();
                    return rawValue;

                case ModificationField.CityId:
                    if (int.TryParse(rawValue, out int cityId))
                    {
                        var city = await _cityBusiness.GetById(cityId);
                        return city?.Name ?? rawValue;
                    }
                    return rawValue;

                case ModificationField.PhotoUrl:
                    {
                        // Si NO está aprobada -> rawValue es base64 o uri local
                        if (status == ModificationRequestStatus.Pending)
                            return rawValue;

                        // Si YA está aprobada -> tomar la foto de la persona actualizada
                        UserDTO? user = await _userBusiness.GetById(userId);
                        PersonDto? person = await _personBusiness.GetById(user.PersonId);
                        return person.PhotoUrl;

                    }

                default:
                    return rawValue;
            }
        }



        /// <summary>
        /// Maneja la actualización de la foto de la persona a partir del Base64 recibido.
        /// </summary>
        private async Task<string> HandlePersonPhotoUpdateAsync(PersonDto person, string base64)
        {
            // Validar base64
            if (string.IsNullOrWhiteSpace(base64))
                throw new InvalidOperationException("La imagen está vacía.");

            byte[] bytes;

            try
            {
                bytes = Convert.FromBase64String(base64);
            }
            catch
            {
                throw new InvalidOperationException("La imagen no tiene un formato base64 válido.");
            }

            using var stream = new MemoryStream(bytes);

            string contentType = "image/jpeg";
            string fileName = $"person_{person.Id}_{DateTime.UtcNow.Ticks}.jpg";

            var (publicUrl, _) = await _personBusiness.UpsertPersonPhotoAsync(
                person.Id,
                stream,
                contentType,
                fileName
            );

            return publicUrl;
        }


        /// <summary>
        /// Envía un correo electrónico al usuario sobre el estado de su solicitud de modificación.
        /// </summary>
        private async Task<bool> SendModificationRequestEmail(ModificationRequest request, string status)
        {
            try
            {
                // Obtener usuario y persona
                var user = await _userBusiness.GetById(request.UserId);
                if (user == null)
                {
                    _logger.LogWarning("No se encontró el usuario {UserId} para enviar email", request.UserId);
                    return false;
                }


                // Preparar datos para la plantilla
                var model = new Dictionary<string, object>
                {
                    ["user_name"] = $"{user.NamePerson}".Trim(),
                    ["status"] = status, // "Approved", "Rejected", "Pending"
                    ["field"] = request.Field.GetDisplayName(),
                    ["request_date"] = request.RequestDate.ToString("dd/MM/yyyy HH:mm"),
                    ["company_name"] = "Sistema de Carnetización Digital",
                    ["app_url"] = "https://carnet.tuempresa.com/solicitudes"
                };

                // Determinar asunto según el estado
                string subject = status switch
                {
                    "Approved" => "Solicitud Aprobada",
                    "Rejected" => "Solicitud Rechazada",
                    _ => "Actualización de Solicitud"
                };

                // Renderizar plantilla HTML
                var html = await EmailTemplates.RenderAsync("ModificationRequest.html", model);

                // Enviar email
                await _notificationSender.NotifyAsync(
                    "email",
                    user.EmailPerson,
                    subject,
                    html
                );

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "No se pudo enviar email de solicitud de modificación al usuario {UserId}", request.UserId);
                return false;
            }
        }

        /// <summary>
        /// Obtiene todas las solicitudes de modificación realizadas por un usuario específico.
        /// </summary>
        public async Task<IEnumerable<ModificationRequestResponseDto>> GetByUserIdAsync(int userId)
        {
            // Obtener desde Data
            var list = await _data.GetByUserIdAsync(userId);

            if (list == null || !list.Any())
                return Enumerable.Empty<ModificationRequestResponseDto>();

            // Mapear entidad → DTO
            return list.Select(r => _mapper.Map<ModificationRequestResponseDto>(r));
        }

    }
}
