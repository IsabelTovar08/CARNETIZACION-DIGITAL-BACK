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
        /// Genera el PDF del carnet emitido y lo devuelve en un ApiResponse con contenido binario.
        /// </summary>
        /// <param name="issuedCardId">Identificador del carnet emitido.</param>
        /// <returns>Archivo PDF dentro de un ApiResponse.</returns>
        [HttpGet("generate/{issuedCardId:int}")]
        [ProducesResponseType(typeof(ApiResponse<byte[]>), 200)]
        [ProducesResponseType(typeof(ApiResponse<byte[]>), 400)]
        [ProducesResponseType(typeof(ApiResponse<byte[]>), 500)]
        [Produces("application/pdf")]
        public async Task<IActionResult> GeneratePdfAsync(int issuedCardId)
        {
            try
            {
                // 🔹 Generar el PDF como arreglo de bytes
                var pdfBytes = await _business.GenerateCardPdfBase64Async(issuedCardId);

                if (pdfBytes == null || pdfBytes.Length == 0)
                    return NotFound(ApiResponse<byte[]>.Fail("No se pudo generar el PDF del carnet."));

                // 🔹 Swagger mostrará el binario, y se mantiene el formato ApiResponse
                return File(pdfBytes, "application/pdf", $"carnet_{issuedCardId}.pdf");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error controlado al generar el PDF del carnet {IssuedCardId}", issuedCardId);
                return BadRequest(ApiResponse<byte[]>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error interno al generar el PDF del carnet {IssuedCardId}", issuedCardId);
                return StatusCode(500, ApiResponse<byte[]>.Fail("Ocurrió un error interno al generar el PDF del carnet."));
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

