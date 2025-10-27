using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Organizational.Assignment;
using Data.Interfases;
using Data.Interfases.Organizational.Assignment;
using Entity.DTOs.Organizational.Assigment.Request;
using Entity.DTOs.Organizational.Assigment.Response;
using Entity.Models.Organizational.Assignment;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Helpers;

namespace Business.Implementations.Organizational.Assignment
{
    public class IssuedCardBusiness : BaseBusiness<IssuedCard, IssuedCardDtoRequest, IssuedCardDto>, IIssuedCardBusiness
    {
        public readonly IIssuedCardData _issuedCardData;
        public IssuedCardBusiness(IIssuedCardData data, ILogger<IssuedCard> logger, IMapper mapper) : base(data, logger, mapper)
        {
            _issuedCardData = data;
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

                // === 3️⃣ Mapear DTO a entidad ===
                IssuedCard entity = _mapper.Map<IssuedCard>(request);
                entity.UniqueId = Guid.NewGuid();
                entity.QRCode = qrBase64;

                // === 4️⃣ Guardar en base de datos usando la capa Data ===
                IssuedCard saved = await _issuedCardData.SaveAsync(entity);

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
    }
}
