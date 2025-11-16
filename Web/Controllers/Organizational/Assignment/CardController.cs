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
    public class CardController : GenericController<CardConfiguration, CardConfigurationDtoRequest, CardConfigurationDto>
    {
        protected readonly ICardConfigurationBusiness _cardBusiness;
        public CardController(ICardConfigurationBusiness business, ILogger<CardController> logger, ICardConfigurationBusiness cardBusiness)
            : base(business, logger)
        {
            _cardBusiness = cardBusiness;
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
