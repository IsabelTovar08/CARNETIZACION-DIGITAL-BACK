using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Classes.Base;
using Data.Interfases.Operational;
using Entity.Context;
using Entity.Models.Operational;
using Entity.Models.Organizational.Assignment;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Implementations.Operational
{
    public class CardTemplateData : BaseData<CardTemplate>, ICardTemplateData
    {
        public CardTemplateData(ApplicationDbContext context, ILogger<CardTemplate> logger) : base(context, logger)
        {
        }


        /// <summary>
        /// Obtiene la plantilla (CardTemplate) asociada a una tarjeta emitida (IssuedCard)
        /// mediante su relación con CardConfiguration.
        /// </summary>
        /// <param name="issuedCardId">Id de la tarjeta emitida</param>
        /// <returns>Objeto CardTemplate asociado a la tarjeta emitida</returns>
        public async Task<CardTemplate?> GetCardTemplateByIssuedCardAsync(int issuedCardId)
        {
            try
            {
                // Carga la tarjeta emitida junto con su configuración y la plantilla asociada
                var issuedCard = await _context.Set<IssuedCard>()
                    .Include(ic => ic.Card)
                        .ThenInclude(cc => cc.CardTemplate)
                    .FirstOrDefaultAsync(ic => ic.Id == issuedCardId);

                // Retorna la plantilla si existe
                return issuedCard?.Card?.CardTemplate;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener CardTemplate para IssuedCard ID {issuedCardId}");
                throw;
            }
        }

    }
}
