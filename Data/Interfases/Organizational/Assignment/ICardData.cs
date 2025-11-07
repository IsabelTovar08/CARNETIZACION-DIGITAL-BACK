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

        ///// <summary>
        ///// Obtiene el listado de carnets emitidos agrupados por Unidad Organizativa.
        ///// </summary>
        //Task<List<CarnetsByUnitDto>> GetCarnetsByOrganizationalUnitAsync();

        ///// <summary>
        ///// Retorna carnets emitidos agrupados por División Interna
        ///// de una Unidad Organizativa específica.
        ///// </summary>
        //Task<List<CarnetsByDivisionDto>> GetCarnetsByInternalDivisionAsync(int organizationalUnitId);

        ///// <summary>
        ///// Obtiene el listado de carnets emitidos agrupados por Jornada (Schedule en Card).
        ///// </summary>
        //Task<List<CarnetsBySheduleDto>> GetCarnetsBySheduleAsync();

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
