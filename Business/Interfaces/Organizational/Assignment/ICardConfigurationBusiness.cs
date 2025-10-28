using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfases;
using Data.Implementations.Organizational.Assignment;
using Entity.DTOs.Organizational.Assigment.Request;
using Entity.DTOs.Organizational.Assigment.Response;
using Entity.DTOs.Specifics;
using Entity.Models.Organizational.Assignment;

namespace Business.Interfaces.Organizational.Assignment
{
    public interface ICardConfigurationBusiness : IBaseBusiness<CardConfiguration,CardConfigurationDtoRequest,CardConfigurationDto>
    {

        /// <summary>
        /// Obtiene el listado de carnets emitidos agrupados por Unidad Organizativa.
        /// </summary>
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
        /// Retorna el total de carnets activos (no eliminados)
        /// </summary>
        Task<int> GetTotalNumberOfIDCardsAsync();
    }
}
