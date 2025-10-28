using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Models.Operational;

namespace Data.Interfases.Operational
{
    public interface ICardTemplateData : ICrudBase<CardTemplate>
    {
        /// <summary>
        /// Obtiene la plantilla (CardTemplate) asociada a una tarjeta emitida (IssuedCard)
        /// mediante su relación con CardConfiguration.
        /// </summary>
        /// <param name="issuedCardId">Id de la tarjeta emitida</param>
        /// <returns>Objeto CardTemplate asociado a la tarjeta emitida</returns>
        Task<CardTemplate?> GetCardTemplateByIssuedCardAsync(int issuedCardId);
    }
}
