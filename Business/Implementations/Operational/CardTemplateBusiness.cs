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
        public readonly ICardTemplateData _cardTemplateData;
        public CardTemplateBusiness(ICardTemplateData data, ILogger<CardTemplate> logger, IMapper mapper, ICodeGeneratorService<CardTemplate>? codeService = null) : base(data, logger, mapper, codeService)
        {
            _cardTemplateData = data;
        }

        public async Task<CardTemplateResponse?> GetCardTemplateByIssuedCard(int issuedCardId)
        {
            try
            {
                if (issuedCardId > 0)
                {
                    CardTemplate? template = await _cardTemplateData.GetCardTemplateByIssuedCardAsync(issuedCardId);
                    return _mapper.Map<CardTemplateResponse?>(template);
                }
                else
                {
                    throw new ArgumentException("El ID de la tarjeta emitida debe ser mayor que cero.", nameof(issuedCardId));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener CardTemplate para IssuedCard ID {issuedCardId}");
                throw;
            }
        }
    }
}
