using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Operational;
using Business.Interfaces.Storage;
using Business.Services.CodeGenerator;
using Data.Implementations.Operational;
using Data.Interfases;
using Data.Interfases.Operational;
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.Models.Operational;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Business.Implementations.Operational
{
    public class CardTemplateBusiness : BaseBusiness<CardTemplate, CardTemplateRequest, CardTemplateResponse>, ICardTemplateBusiness
    {
        protected readonly ICardTemplateData _cardTemplateData;
        private readonly IAssetUploader _assetUploader;
        public CardTemplateBusiness(ICrudBase<CardTemplate> data, ILogger<CardTemplate> logger, IMapper mapper, ICardTemplateData cardTemplateData,
            IAssetUploader assetUploader,
            ICodeGeneratorService<CardTemplate>? codeService = null
            ) : base(data, logger, mapper, codeService)
        {
            _cardTemplateData = cardTemplateData;
            _assetUploader = assetUploader;
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


        // ============================================================
        // 🚀 CREATE
        // ============================================================
        public override async Task<CardTemplateResponse> Save(CardTemplateRequest dto)
        {
            var entity = _mapper.Map<CardTemplate>(dto);

            entity.FrontBackgroundUrl = "Pending";
            entity.BackBackgroundUrl = "Pending";
            entity.FrontElementsJson = "Pending";
            entity.BackElementsJson = "Pending";

            // guardar primero para obtener ID
            entity = await _data.SaveAsync(entity);

            // subir front/back si vienen
            var frontUrl = await UploadSvgAsync(dto.FrontFile, entity.Id, "front");
            var backUrl = await UploadSvgAsync(dto.BackFile, entity.Id, "back");

            if (frontUrl != null) entity.FrontBackgroundUrl = frontUrl;
            if (backUrl != null) entity.BackBackgroundUrl = backUrl;

            await _data.UpdateAsync(entity);

            return _mapper.Map<CardTemplateResponse>(entity);
        }


        // ============================================================
        // 🔄 UPDATE
        // ============================================================
        public override async Task<CardTemplateResponse> Update(CardTemplateRequest dto)
        {
            var entity = await _data.GetByIdAsync(dto.Id)
                ?? throw new KeyNotFoundException("Plantilla no encontrada.");

            entity.Name = dto.Name;
            entity.FrontElementsJson = dto.FrontElementsJson;
            entity.BackElementsJson = dto.BackElementsJson;

            var frontUrl = await UploadSvgAsync(dto.FrontFile, dto.Id, "front");
            var backUrl = await UploadSvgAsync(dto.BackFile, dto.Id, "back");

            if (frontUrl != null) entity.FrontBackgroundUrl = frontUrl;
            if (backUrl != null) entity.BackBackgroundUrl = backUrl;

            var updated = await _data.UpdateAsync(entity);

            return _mapper.Map<CardTemplateResponse>(updated);
        }



        // ============================================================
        // 🔥 MÉTODO PRIVADO REUTILIZABLE PARA SUBIR FRONT/BACK
        // ============================================================
        private async Task<string> UploadSvgAsync(
            IFormFile? file,
            int templateId,
            string side // "front" o "back"
        )
        {
            if (file == null)
                return null;

            using var stream = file.OpenReadStream();

            var (url, _) = await _assetUploader.UpsertAsync(
                new[] { "templates", templateId.ToString(), side },
                null,                  
                stream,
                "image/svg+xml",
                $"{side}.svg",
                "Templates"
            );

            return url;
        }

    }
}

