using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Specifics;
using Entity.Models.Organizational.Assignment;

namespace Data.Interfases.Organizational.Assignment
{
    public interface ICardData  : ICrudBase<Card>
    {

        /// <summary>
        /// Obtiene el listado de carnets emitidos agrupados por Unidad Organizativa.
        /// </summary>
        Task<List<CarnetsByUnitDto>> GetCarnetsByOrganizationalUnitAsync();

        /// <summary>
        /// Retorna carnets emitidos agrupados por División Interna
        /// de una Unidad Organizativa específica.
        /// </summary>
        Task<List<CarnetsByDivisionDto>> GetCarnetsByInternalDivisionAsync(int organizationalUnitId);

        /// <summary>
        /// Obtiene el listado de carnets emitidos agrupados por Jornada (Schedule en Card).
        /// </summary>
        Task<List<CarnetsBySheduleDto>> GetCarnetsBySheduleAsync();

        /// <summary>
        /// Retorna el total de carnets que no están eliminados lógicamente
        /// </summary>
        /// <returns>Total de carnets</returns>
        Task<int> getTotalNumberOfIDCards();
    }
}
