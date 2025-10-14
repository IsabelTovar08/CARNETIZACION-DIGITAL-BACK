using Business.Interfaces.Organizational.Assignment;
using Business.Interfases;
using Entity.DTOs.Organizational.Assigment.Request;
using Entity.DTOs.Organizational.Assigment.Response;
using Entity.Models.Organizational.Assignment;
using Microsoft.AspNetCore.Mvc;
using Utilities.Responses;
using Web.Controllers.Base;

namespace Web.Controllers.Organizational.Assignment
{
    public class CardController : GenericController<Card, CardDtoRequest, CardDto>
    {
        protected readonly ICardBusiness _cardBusiness;
        public CardController(ICardBusiness business, ILogger<CardController> logger, ICardBusiness cardBusiness)
            : base(business, logger)
        {
            _cardBusiness = cardBusiness;
        }

        /// <summary>
        /// Retorna carnets emitidos agrupados por Unidad Organizativa.
        /// </summary>
        [HttpGet("by-unit")]
        public async Task<IActionResult> GetByUnitAsync()
        {
            try
            {
                var result = await _cardBusiness.GetCarnetsByOrganizationalUnitAsync();
                var total = await _cardBusiness.GetTotalNumberOfIDCardsAsync();

                return Ok(ApiResponse<object>.Ok(new
                {
                    Total = total,
                    Data = result
                }, "Carnets agrupados por unidad organizativa obtenidos correctamente"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener carnets por unidad organizativa");
                return BadRequest(ApiResponse<object>.Fail("Error al obtener carnets por unidad organizativa", new[] { ex.Message }));
            }
        }

        /// <summary>
        /// Retorna carnets emitidos agrupados por División Interna de una Unidad.
        /// </summary>
        [HttpGet("by-unit/{organizationalUnitId}/divisions")]
        public async Task<IActionResult> GetByInternalDivisionAsync(int organizationalUnitId)
        {
            try
            {
                var result = await _cardBusiness.GetCarnetsByInternalDivisionAsync(organizationalUnitId);

                return Ok(ApiResponse<object>.Ok(result, "Carnets agrupados por divisiones internas obtenidos correctamente"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener carnets por divisiones internas en unidad {organizationalUnitId}");
                return BadRequest(ApiResponse<object>.Fail("Error al obtener carnets por divisiones internas", new[] { ex.Message }));
            }
        }

        /// <summary>
        /// Retorna carnets emitidos agrupados por Jornada (Schedule en Card).
        /// </summary>
        [HttpGet("by-shedule")]
        public async Task<IActionResult> GetBySheduleAsync()
        {
            try
            {
                var result = await _cardBusiness.GetCarnetsBySheduleAsync();

                return Ok(ApiResponse<object>.Ok(result, "Carnets agrupados por jornada obtenidos correctamente"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener carnets por jornada");
                return BadRequest(ApiResponse<object>.Fail("Error al obtener carnets por jornada", new[] { ex.Message }));
            }
        }

        /// <summary>
        /// Retorna el total general de carnets emitidos en el sistema.
        /// </summary>
        [HttpGet("total")]
        public async Task<IActionResult> GetTotalNumberOfIDCardsAsync()
        {
            try
            {
                var total = await _cardBusiness.GetTotalNumberOfIDCardsAsync();

                return Ok(ApiResponse<int>.Ok(total, "Total de carnets obtenidos correctamente"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en Business al obtener el total de carnets");
                return BadRequest(ApiResponse<int>.Fail("Error al obtener el total de carnets", new[] { ex.Message }));
            }
        }
    }
}
