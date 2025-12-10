using Business.Services.Excel;
using Entity.DTOs.Specifics;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.ModelSecurity;

namespace Web.Controllers.Others
{
    [ApiController]
    [Route("api/excel")]
    public class ExcelController : ControllerBase
    {
        private readonly IExcelPersonParser _excel;
        private readonly IExcelBulkImporter _importer;


        public ExcelController(IExcelPersonParser excel, IExcelBulkImporter importer)
        {
            _excel = excel;
            _importer = importer;
        }

        /// <summary>
        /// Import Excel (.xlsx) with columns:
        /// FirstName, MiddleName, LastName, SecondLastName, DocumentTypeId, DocumentNumber, BloodTypeId, Phone, Email, Address, CityId
        /// </summary>
        //[HttpPost("people")]
        //[Consumes("multipart/form-data")]
        //[RequestSizeLimit(50_000_000)]
        //public async Task<ActionResult<BulkImportResultDto>> ImportExcel(MassiveInfluxOfPeople req)
        //{
        //    if (req?.File is null)
        //        return BadRequest("Debe adjuntar el archivo en el campo 'file'.");

        //    if (!req.File.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
        //        return BadRequest("Formato no soportado. Solo .xlsx.");

        //    using var stream = req.File.OpenReadStream(); 
        //    var result = await _excel.ImportFromExcelAsync(stream);

        //    return Ok(result);
        //}


        [HttpPost("import/people")]
        public async Task<IActionResult> ImportPeople([FromForm] ImportContextCard ctx, [FromForm] UploadFile fileRequest)
        {
            try
            {
                var file = fileRequest.file;
                if (file == null || file.Length == 0) return BadRequest("Archivo Excel vacío.");

                using var stream = file.OpenReadStream();
                var result = await _importer.ImportAsync(stream, ctx); // IExcelBulkImporter

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                // ⚠️ Error esperado para archivos vacíos o inválidos
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // ⚠️ Error inesperado
                return StatusCode(500, new { message = "Error interno procesando el archivo." });
            }
        }
    }
}
