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
        /// Obtiene el CardTemplate asociado a un CardConfiguration específico.
        /// </summary>
        /// <param name="cardConfigurationId">Identificador del CardConfiguration.</param>
        /// <returns>Entidad CardTemplate encontrada.</returns>
        Task<CardTemplateResponse> GetTemplateByCardConfigurationId(int cardConfigurationId);
    }
}
