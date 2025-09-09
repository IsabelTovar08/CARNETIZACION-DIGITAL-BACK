using Business.Services.Excel;
using Entity.DTOs.ModelSecurity.Response;
using Entity.DTOs.Organizational.Assigment.Request;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Others
{
    [ApiController]
    [Route("api/excel")]
    public class ExcelController : ControllerBase
    {
        private readonly IExcelPersonParser _excel;

        public ExcelController(IExcelPersonParser excel)
        {
            _excel = excel;
        }

        /// <summary>
        /// Import Excel (.xlsx) with columns:
        /// FirstName, MiddleName, LastName, SecondLastName, DocumentTypeId, DocumentNumber, BloodTypeId, Phone, Email, Address, CityId
        /// </summary>
        [HttpPost("people")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(50_000_000)]
        public async Task<ActionResult<BulkImportResultDto>> ImportExcel(MassiveInfluxOfPeople req)
        {
            if (req?.File is null)
                return BadRequest("Debe adjuntar el archivo en el campo 'file'.");

            if (!req.File.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
                return BadRequest("Formato no soportado. Solo .xlsx.");

            using var stream = req.File.OpenReadStream(); 
            var result = await _excel.ImportFromExcelAsync(stream);

            return Ok(result);
        }
    }
}
