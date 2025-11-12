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
    public class IssuedCardController : GenericController<IssuedCard, IssuedCardDtoRequest, IssuedCardDto>
    {
        protected readonly IIssuedCardBusiness _business;
        public IssuedCardController(IIssuedCardBusiness business, ILogger<IssuedCardController> logger) : base(business, logger)
        {
            _business = business;
        }

        /// <summary>
        /// Genera el PDF del carnet emitido y lo devuelve ya sea como archivo PDF
        /// o como JSON con base64, dependiendo del header Accept del cliente.
        /// </summary>
        /// <param name="issuedCardId">Identificador del carnet emitido.</param>
        /// <returns>Archivo PDF o JSON con base64, según la solicitud.</returns>
        [HttpGet("generate/{issuedCardId:int}")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GeneratePdfAsync(int issuedCardId)
        {
            try
            {
                // 🔹 Generar el PDF como arreglo de bytes
                var pdfBytes = await _business.GenerateCardPdfBase64Async(issuedCardId);

                if (pdfBytes == null || pdfBytes.Length == 0)
                    return NotFound("No se pudo generar el PDF del carnet.");

                var fileName = $"carnet_{issuedCardId}.pdf";

                // 🔹 Detectar el encabezado Accept del cliente
                var accept = Request.Headers["Accept"].ToString();

                // Si el cliente pide PDF directamente (app móvil, navegador)
                if (accept.Contains("application/pdf", StringComparison.OrdinalIgnoreCase))
                    return File(pdfBytes, "application/pdf", fileName);

                // Si Swagger o Postman piden JSON, devolvemos base64 dentro del ApiResponse
                var base64 = Convert.ToBase64String(pdfBytes);
                return Ok(ApiResponse<string>.Ok(base64));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error controlado al generar el PDF del carnet {IssuedCardId}", issuedCardId);
                return BadRequest(ApiResponse<string>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error interno al generar el PDF del carnet {IssuedCardId}", issuedCardId);
                return StatusCode(500, ApiResponse<string>.Fail("Ocurrió un error interno al generar el PDF del carnet."));
            }
        }




        /// <summary>
        /// Genera el PDF del carnet emitido y permite su descarga directa desde Swagger o el navegador.
        /// </summary>
        /// <param name="issuedCardId">Identificador del carnet emitido.</param>
        /// <returns>Archivo PDF descargable.</returns>
        [HttpGet("download/{issuedCardId:int}")]
        [Produces("application/pdf")]
        [ProducesResponseType(typeof(FileContentResult), 200)]
        public async Task<IActionResult> DownloadPdfAsync(int issuedCardId)
        {
            try
            {
                // 🔹 Generar el PDF en memoria desde la capa de negocio
                var pdfBytes = await _business.GenerateCardPdfBase64Async(issuedCardId);

                if (pdfBytes == null || pdfBytes.Length == 0)
                    return NotFound("No se pudo generar el PDF del carnet.");

                // 🔹 Enviar el archivo al cliente
                var fileName = $"carnet_{issuedCardId}.pdf";
                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error controlado al generar el PDF del carnet {IssuedCardId}", issuedCardId);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error interno al generar el PDF del carnet {IssuedCardId}", issuedCardId);
                return StatusCode(500, "Error interno al generar el PDF del carnet.");
            }
        }
    }
}

