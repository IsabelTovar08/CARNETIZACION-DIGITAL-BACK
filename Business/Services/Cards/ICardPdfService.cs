using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Operational.Response;
using Entity.DTOs.Specifics.Cards;
using Entity.Models.Operational;

namespace Business.Services.Cards
{
    /// <summary>
    /// Servicio encargado de generar el PDF de los carnets.
    /// </summary>
    public interface ICardPdfService
    {
        /// <summary>
        /// Genera el PDF del carnet con base en la plantilla y los datos del usuario.
        /// </summary>
        Task GenerateCardAsync(CardTemplateResponse template, CardUserData userData, Stream output);
    }
}
