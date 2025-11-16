using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Classes.Base;
using Data.Interfases.Operational;
using Entity.Context;
using Entity.Models.Operational;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Implementations.Operational
{
    public class CardTemplateData : BaseData<CardTemplate>, ICardTemplateData
    {
        public CardTemplateData(ApplicationDbContext context, ILogger<CardTemplate> logger) : base(context, logger)
        {
        }


        /// <inheritdoc/>
        public async Task<CardTemplate> GetTemplateByCardConfigurationIdAsync(int cardConfigurationId)
        {
            try
            {
                // Obtener directamente el template usando la relación de CardConfiguration → CardTemplateId
                var cardTemplate = await _context.CardTemplates
                    .Join(
                        _context.CardsConfigurations,
                        t => t.Id,
                        c => c.CardTemplateId,
                        (t, c) => new { Template = t, Config = c }
                    )
                    .Where(x => x.Config.Id == cardConfigurationId && !x.Template.IsDeleted && !x.Config.IsDeleted)
                    .Select(x => x.Template)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (cardTemplate == null)
                    throw new InvalidOperationException($"No se encontró la plantilla asociada al CardConfiguration {cardConfigurationId}.");

                return cardTemplate;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el CardTemplate para CardConfigurationId {CardConfigurationId}", cardConfigurationId);
                throw;
            }
        }

    }
}
