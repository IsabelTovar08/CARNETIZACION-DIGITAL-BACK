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

namespace Business.Implementations.Organizational.Assignment
{
    public class IssuedCardBusiness : BaseBusiness<IssuedCard, IssuedCardDtoRequest, IssuedCardDto>, IIssuedCardBusiness
    {
        public readonly IIssuedCardData _issuedCardData;
        public IssuedCardBusiness(ICrudBase<IssuedCard> data, ILogger<IssuedCard> logger, IMapper mapper, IIssuedCardData issuedCardData) : base(data, logger, mapper)
        {
            _issuedCardData = issuedCardData;
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
