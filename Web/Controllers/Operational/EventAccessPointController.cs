using Business.Interfaces.Operational;
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.Models.Operational;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.Base;

namespace Web.Controllers.Operational
{
    public class EventAccessPointController : GenericController<EventAccessPoint, EventAccessPointDtoRequest, EventAccessPointDto>
    {
        protected readonly IEventAccessPointBusiness _business;
        public EventAccessPointController(IEventAccessPointBusiness business, ILogger<EventAccessPointController> logger) : base(business, logger)
        {
            _business = business;
        }

        /// <summary>
        /// Crea EventAccessPoint con validación de negocio y manejo de excepciones.
        /// </summary>
        public override async Task<IActionResult> Create(EventAccessPointDtoRequest dto)
        {
            try
            {
                var response = await _business.Save(dto);

                return Ok(response);
            }
            catch (InvalidOperationException ex) // duplicado
            {
                return Conflict(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (ArgumentException ex) // validaciones de parametros
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error interno en SaveAsync.");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error interno del servidor."
                });
            }
        }



        /// <summary>
        /// Actualiza EventAccessPoint con manejo de excepciones.
        /// </summary>
        public override async Task<IActionResult> Update(EventAccessPointDtoRequest dto)
        {
            try
            {
                var response = await _business.Update(dto);

                return Ok(response);
            }
            catch (InvalidOperationException ex) // duplicado
            {
                return Conflict(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (ArgumentException ex) // parametros invalidos
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error interno en UpdateAsync.");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error interno del servidor."
                });
            }
        }
    }
}
