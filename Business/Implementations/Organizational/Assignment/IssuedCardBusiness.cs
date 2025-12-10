using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Notifications;
using Business.Interfaces.Operational;
using Business.Interfaces.Organizational.Assignment;
using Business.Interfaces.Security;
using Business.Services.Cards;
using Business.Services.Notifications;
using Data.Interfases;
using Data.Interfases.Organizational.Assignment;
using Entity.DTOs.Operational.Response;
using Entity.DTOs.Organizational.Assigment.Request;
using Entity.DTOs.Organizational.Assigment.Response;
using Entity.DTOs.Specifics;
using Entity.DTOs.Specifics.Cards;
using Entity.Enums.Specifics;
using Entity.Models.Organizational.Assignment;
using Entity.Models.Parameter;
using Infrastructure.Notifications.Interfases;
using Microsoft.Extensions.Logging;
using Utilities.Helpers;
using Utilities.Notifications.Implementations.Templates.Email;

namespace Business.Implementations.Organizational.Assignment
{
    public class IssuedCardBusiness : BaseBusiness<IssuedCard, IssuedCardDtoRequest, IssuedCardDto>, IIssuedCardBusiness
    {
        public readonly IIssuedCardData _issuedCardData;
        protected readonly ICardTemplateBusiness _cardTemplateBusiness;
        protected readonly ICardPdfService _cardPdfService;
        private readonly ICardConfigurationBusiness _cardConfigurationBusiness;
        private readonly INotify _notify;
        private readonly INotificationBusiness _notificationBusiness;
        private readonly IPersonBusiness _personBusiness;


        public IssuedCardBusiness(IIssuedCardData data, ILogger<IssuedCard> logger, IMapper mapper, ICardTemplateBusiness cardTemplateBusiness, ICardPdfService cardPdfService,
            ICardConfigurationBusiness cardConfigurationBusiness,
            INotify notify,
            INotificationBusiness notificationBusiness,
            IPersonBusiness personBusiness
            ) : base(data, logger, mapper)
        {
            _issuedCardData = data;
            _cardTemplateBusiness = cardTemplateBusiness;
            _cardPdfService = cardPdfService;
            _cardConfigurationBusiness = cardConfigurationBusiness;
            _notify = notify;
            _notificationBusiness = notificationBusiness;
            _personBusiness = personBusiness;
        }

        /// <summary>
        /// Sobrescribe el método SaveAsync para asignar UUID y código QR al crear un carnet.
        /// </summary>
        /// <param name="request">Datos del carnet a emitir.</param>
        /// <returns>Carnet emitido con UUID y QR asignados.</returns>
        public override async Task<IssuedCardDto> Save(IssuedCardDtoRequest request)
        {
            try
            {
                // === 1️⃣ Generar UUID único para el carnet ===
                Guid uuid = Guid.NewGuid();

                // === 2️⃣ Generar QR Code basado en el UUID ===
                string qrBase64 = QrCodeHelper.ToPngBase64(uuid.ToString());

                if(request.CardId is null || request.CardId <= 0)
                {
                    CardConfigurationDtoRequest cardRequest = new CardConfigurationDtoRequest
                    {
                        ProfileId = request.ProfileId ?? 0,
                        CardTemplateId = request.CardTemplateId ?? 0,
                        Name = request.CardName ?? "",
                        ValidFrom = request.ValidFrom ?? new DateTime(),
                        ValidTo = request.ValidFrom ?? new DateTime()

                    };

                    CardConfigurationDto cardSaved = await _cardConfigurationBusiness.Save(cardRequest);
                    request.CardId = cardSaved.Id;
                }

                // === 3️⃣ Mapear DTO a entidad ===
                IssuedCard entity = _mapper.Map<IssuedCard>(request);
                entity.UniqueId = Guid.NewGuid();
                entity.QRCode = qrBase64;
                entity.Card = null;
                entity.StatusId = 1;
                // === 4️⃣ Guardar en base de datos usando la capa Data ===
                IssuedCard saved = await _issuedCardData.SaveAsync(entity);

                // Obtener datos completos para enviar correo
                CardUserData userData = await _issuedCardData.GetCardDataByIssuedIdAsync(saved.Id);

                // Enviar notificación
                await SendCardAssignedNotificationAsync(saved, userData);

                var person = await _personBusiness.GetById(request.PersonId);
                // Notificación 
                var notificationRequest = await NotificationFactory.Create(
                    NotificationTemplateType.CardAssigned,
                    userData.Name,                 // args[0]
                    saved.UniqueId.ToString()      // args[1]
                );

                // Forzar userId del destinatario
                notificationRequest.UserId = person?.UserId ?? 0;

                // Enviar y registrar la notificación
                await _notificationBusiness.CreateAndSendAsync(notificationRequest);

                // === 5️⃣ Mapear de vuelta a DTO ===
                return _mapper.Map<IssuedCardDto>(saved);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar el carnet con UUID y QR.");  
                throw new Exception("Error al guardar el carnet con UUID y QR.", ex);
            }
        }

        /// <summary>
        /// Actualiza la URL pública del PDF del carnet en la base de datos.
        /// </summary>
        public async Task<IssuedCardDto> UpdatePdfUrlAsync(int cardId, string pdfUrl)
        {
            try
            {
                IssuedCard card = await _issuedCardData.UpdatePdfUrlAsync(cardId, pdfUrl);
                if (card == null)
                    throw new Exception("No se encontró el carnet para actualizar la URL del PDF.");

                return _mapper.Map<IssuedCardDto>(card);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa de negocio al actualizar la URL del PDF del carnet.", ex);
            }
        }

        /// <summary>
        /// Consulta la información completa del carnet, incluyendo organización y sucursal.
        /// </summary>
        //public async Task<CardUserData> GetCardDataByIssuedIdAsync(int issuedCardId)
        //{
        //    try
        //    {
        //        CardUserData card = await _issuedCardData.GetCardDataByIssuedIdAsync(issuedCardId);
        //        if (card == null)
        //            throw new Exception("No se encontró el carnet para actualizar la URL del PDF.");

        //        //return _mapper.Map<IssuedCardDto>(card);
        //        return card;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error en la capa de negocio al actualizar la URL del PDF del carnet.", ex);
        //    }
        //}


        public async Task<CardUserData> GetCardDataByIssuedId(int issuedCardId)
        {
            try
            {
                // 1. Obtener los datos del carnet emitido (CardUserData)
                CardUserData userData = await _issuedCardData.GetCardDataByIssuedIdAsync(issuedCardId);
                return userData;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error controlado al generar el PDF del carnet {IssuedCardId}", issuedCardId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error interno al generar el PDF del carnet {IssuedCardId}", issuedCardId);
                throw new InvalidOperationException("Error interno al generar el PDF del carnet.", ex);
            }
        }




        /// <inheritdoc/>
        public async Task<byte[]> GenerateCardPdfBase64Async(int issuedCardId)
        {
            try
            {
                // 1. Obtener los datos del carnet emitido (CardUserData)
                CardUserData userData = await GetCardDataByIssuedId(issuedCardId);

                if (userData == null)
                    throw new InvalidOperationException($"No se encontraron datos para el carnet emitido {issuedCardId}.");

                // 2. Obtener el ID de configuración desde el IssuedCard real
                IssuedCardDto? issuedCardEntity = await GetById(issuedCardId);

                if (issuedCardEntity == null)
                    throw new InvalidOperationException($"No se encontró el registro IssuedCard con ID {issuedCardId}.");

                int cardConfigurationId = issuedCardEntity.CardId; // aquí CardId = CardConfigurationId

                // 3. Consultar la plantilla asociada a ese CardConfiguration
                CardTemplateResponse cardTemplate = await _cardTemplateBusiness.GetTemplateByCardConfigurationId(cardConfigurationId);

                if (cardTemplate == null)
                    throw new InvalidOperationException($"No se encontró plantilla asociada al CardConfiguration {cardConfigurationId}.");

                // 4. Generar el PDF en memoria
                using var ms = new MemoryStream();

                await _cardPdfService.GenerateCardAsync(cardTemplate, userData, ms);
                ms.Position = 0;

                return ms.ToArray();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error controlado al generar el PDF del carnet {IssuedCardId}", issuedCardId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error interno al generar el PDF del carnet {IssuedCardId}", issuedCardId);
                throw new InvalidOperationException("Error interno al generar el PDF del carnet.", ex);
            }
        }

        /// <summary>
        /// Obtiene el listado de carnets emitidos agrupados por Unidad Organizativa.
        /// </summary>
        public async Task<List<CarnetsByUnitDto>> GetCarnetsByOrganizationalUnitAsync()
        {
            try
            {
                var result = await _issuedCardData.GetCarnetsByOrganizationalUnitAsync();

                if (result == null || result.Count == 0)
                    throw new Exception("No se encontraron carnets por unidad organizativa.");

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa de negocio al consultar carnets por unidad organizativa.", ex);
            }
        }

        /// <summary>
        /// Obtiene carnets emitidos agrupados por División Interna de una Unidad.
        /// </summary>
        public async Task<List<CarnetsByDivisionDto>> GetCarnetsByInternalDivisionAsync(int organizationalUnitId)
        {
            try
            {
                return await _issuedCardData.GetCarnetsByInternalDivisionAsync(organizationalUnitId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa de negocio al consultar carnets por división interna.", ex);
            }
        }


        /// <summary>
        /// Obtiene el listado de carnets emitidos agrupados por Jornada (Schedule en Card).
        /// </summary>
        public async Task<List<CarnetsBySheduleDto>> GetCarnetsBySheduleAsync()
        {
            try
            {
                var result = await _issuedCardData.GetCarnetsBySheduleAsync();

                if (result == null || result.Count == 0)
                    throw new Exception("No se encontraron carnets por jornada.");

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa de negocio al consultar carnets por jornada.", ex);
            }
        }

        /// <summary>
        /// Retorna el total de carnets activos (no eliminados)
        /// </summary>
        /// <returns>Total de carnets</returns>
        public async Task<int> GetTotalNumberOfIDCardsAsync()
        {
            try
            {
                return await _issuedCardData.GetTotalNumberOfIDCardsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en Business al obtener el total de carnets");
                throw;
            }
        }


        private async Task SendCardAssignedNotificationAsync(IssuedCard card, CardUserData data)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(data.Email))
                    return;

                var model = new Dictionary<string, object>
                {
                    ["user_name"] = $"{data.Name}",
                    ["division"] = data.InternalDivisionName,
                    ["profile"] = data.Profile,
                    ["valid_from"] = data.ValidFrom.ToString("dd/MM/yyyy") ?? "",
                    ["valid_to"] = data.ValidUntil.ToString("dd/MM/yyyy") ?? "",
                    ["company_name"] = "Sistema de Carnetización Digital",
                    ["app_url"] = "https://carnet.go.com"
                };

                string html = await EmailTemplates.RenderAsync("CardAssigned.html", model);

                await _notify.NotifyAsync(
                    "email",
                    data.Email,
                    "Nuevo Carnet Asignado",
                    html
                );
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "No se pudo enviar correo de asignación del carnet.");
            }
        }

        /// <summary>
        /// Para traer todos los carnets que tiene la persona
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<IssuedCardDto>> GetIssuedCardsByUserIdAsync(int userId)
        {
            var cards = await _issuedCardData.GetIssuedCardsByUserIdAsync(userId);

            if (cards == null || !cards.Any())
                return new List<IssuedCardDto>();

            return _mapper.Map<List<IssuedCardDto>>(cards);
        }

    }
}
