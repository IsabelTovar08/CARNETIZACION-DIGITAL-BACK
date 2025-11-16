using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Specifics;
using Entity.Models.Operational;
using Entity.Models.Organizational.Assignment;

namespace Data.Interfases.Organizational.Assignment
{
    public interface ICardConfigurationData : ICrudBase<CardConfiguration>
    {

        /// <summary>
        /// Retorna el total de carnets que no están eliminados lógicamente
        /// </summary>
        /// <returns>Total de carnets</returns>
        Task<int> getTotalNumberOfIDCardConfigurations();

        /// <summary>
        /// Obtiene la plantilla (CardTemplate) asociada a un CardConfiguration específico.
        /// </summary>
        Task<CardTemplate> GetTemplateByCardConfigurationIdAsync(int cardConfigurationId);
    }
}
