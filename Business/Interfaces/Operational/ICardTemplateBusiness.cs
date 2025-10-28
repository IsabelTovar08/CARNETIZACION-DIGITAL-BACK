using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfases;
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.Models.Operational;

namespace Business.Interfaces.Operational
{
    public interface ICardTemplateBusiness : IBaseBusiness<CardTemplate, CardTemplateRequest, CardTemplateResponse>
    {
        /// <summary>
        /// Obtiene la plantilla (CardTemplate) asociada a una tarjeta emitida (IssuedCard)
        /// mediante su relación con CardConfiguration.
        /// </summary>
        /// <param name="issuedCardId">Id de la tarjeta emitida</param>
        /// <returns>Objeto CardTemplate asociado a la tarjeta emitida</returns>
        Task<CardTemplateResponse?> GetCardTemplateByIssuedCard(int issuedCardId);
    }
}
