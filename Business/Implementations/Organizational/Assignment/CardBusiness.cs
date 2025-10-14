using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Organizational.Assignment;
using Data.Implementations.Organizational.Assignment;
using Data.Interfases;
using Data.Interfases.Organizational.Assignment;
using Entity.DTOs.Organizational.Assigment.Request;
using Entity.DTOs.Organizational.Assigment.Response;
using Entity.DTOs.Specifics;
using Entity.Models.Organizational.Assignment;
using Microsoft.Extensions.Logging;

namespace Business.Implementations.Organizational.Assignment
{
    public class CardBusiness : BaseBusiness<Card, CardDtoRequest, CardDto>, ICardBusiness
    {
        public readonly ICardData _cardData;
        public CardBusiness(ICardData data, ILogger<Card> logger, IMapper mapper, ICardData cardData) : base(data, logger, mapper)
        {
            _cardData = cardData;
        }

        public override Task<CardDto> Save(CardDtoRequest entity)
        {
            
            return base.Save(entity);
        }

        /// <summary>
        /// Obtiene el listado de carnets emitidos agrupados por Unidad Organizativa.
        /// </summary>
        public async Task<List<CarnetsByUnitDto>> GetCarnetsByOrganizationalUnitAsync()
        {
            try
            {
                var result = await _cardData.GetCarnetsByOrganizationalUnitAsync();

                if (result == null || result.Count == 0)
                    throw new Exception("No se encontraron carnets por unidad organizativa.");

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa de negocio al consultar carnets por unidad organizativa.", ex);
            }
        }

        /// <summary>
        /// Obtiene carnets emitidos agrupados por División Interna de una Unidad.
        /// </summary>
        public async Task<List<CarnetsByDivisionDto>> GetCarnetsByInternalDivisionAsync(int organizationalUnitId)
        {
            try
            {
                return await _cardData.GetCarnetsByInternalDivisionAsync(organizationalUnitId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa de negocio al consultar carnets por división interna.", ex);
            }
        }


        /// <summary>
        /// Obtiene el listado de carnets emitidos agrupados por Jornada (Schedule en Card).
        /// </summary>
        public async Task<List<CarnetsBySheduleDto>> GetCarnetsBySheduleAsync()
        {
            try
            {
                var result = await _cardData.GetCarnetsBySheduleAsync();

                if (result == null || result.Count == 0)
                    throw new Exception("No se encontraron carnets por jornada.");

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa de negocio al consultar carnets por jornada.", ex);
            }
        }

        /// <summary>
        /// Retorna el total de carnets activos (no eliminados)
        /// </summary>
        /// <returns>Total de carnets</returns>
        public async Task<int> GetTotalNumberOfIDCardsAsync()
        {
            try
            {
                return await _cardData.getTotalNumberOfIDCards();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en Business al obtener el total de carnets");
                throw;
            }
        }
    }
}
