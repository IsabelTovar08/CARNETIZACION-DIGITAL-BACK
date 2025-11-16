using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Operational;
using Business.Services.CodeGenerator;
using Data.Interfases;
using Data.Interfases.Operational;
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.Models.Operational;
using Microsoft.Extensions.Logging;

namespace Business.Implementations.Operational
{
    public class CardTemplateBusiness : BaseBusiness<CardTemplate, CardTemplateRequest, CardTemplateResponse>, ICardTemplateBusiness
    {
        protected readonly ICardTemplateData _cardTemplateData;
        public CardTemplateBusiness(ICrudBase<CardTemplate> data, ILogger<CardTemplate> logger, IMapper mapper, ICardTemplateData cardTemplateData, ICodeGeneratorService<CardTemplate>? codeService = null) : base(data, logger, mapper, codeService)
        {
            _cardTemplateData = cardTemplateData;
        }

        /// <summary>
        /// Mapea la entidad CardTemplate a su DTO CardTemplateResponse.
        /// </summary>
        /// <param name="cardConfigurationId">Identificador del CardConfiguration.</param>
        /// <returns>DTO de tipo CardTemplateResponse mapeado desde la entidad CardTemplate.</returns>
        public async Task<CardTemplateResponse> GetTemplateByCardConfigurationId(int cardConfigurationId)
        {
            try
            {
                // Consultar la entidad desde la capa Data
                CardTemplate cardTemplateEntity = await _cardTemplateData.GetTemplateByCardConfigurationIdAsync(cardConfigurationId);

                if (cardTemplateEntity == null)
                    throw new InvalidOperationException($"No se encontró la plantilla asociada al CardConfiguration {cardConfigurationId}.");

                // Mapear la entidad a DTO usando AutoMapper
                CardTemplateResponse cardTemplateDto = _mapper.Map<CardTemplateResponse>(cardTemplateEntity);

                return cardTemplateDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al mapear la plantilla a DTO para CardConfigurationId {CardConfigurationId}", cardConfigurationId);
                throw new InvalidOperationException("Error al mapear la plantilla del carnet.", ex);
            }
        }

    }
}
