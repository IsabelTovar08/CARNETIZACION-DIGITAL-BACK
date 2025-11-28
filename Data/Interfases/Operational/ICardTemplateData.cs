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
        /// Obtiene el CardTemplate asociado a un CardConfiguration específico.
        /// </summary>
        /// <param name="cardConfigurationId">Identificador del CardConfiguration.</param>
        /// <returns>Entidad CardTemplate encontrada.</returns>
        Task<CardTemplate> GetTemplateByCardConfigurationIdAsync(int cardConfigurationId);
    }
}
