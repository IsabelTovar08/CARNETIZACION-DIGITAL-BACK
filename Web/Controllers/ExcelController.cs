using Business.Services.Excel;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/excel")]
    public class ExcelController : ControllerBase
    {
        private readonly ExcelReaderService _excel;

        public ExcelController(ExcelReaderService excel) => _excel = excel;

        [HttpPost("preview")]
        [Consumes("multipart/form-data")] // <- importante para Swagger
        public async Task<IActionResult> Preview([FromForm] ExcelPreviewForm form)
        {
            if (form.File is null || form.File.Length == 0)
                return BadRequest("Archivo vacío.");

            var path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.xlsx");
            using (var stream = System.IO.File.Create(path))
                await form.File.CopyToAsync(stream);

            var data = _excel.ReadPreview(path, form.PreviewRows);
            var token = Path.GetFileName(path); // o guarda el path/token en un diccionario si prefieres
            return Ok(new { token, preview = data });
        }

        [HttpPost("confirm")]
        public IActionResult Confirm([FromBody] string token)
        {
            var path = Path.Combine(Path.GetTempPath(), token);
            if (!System.IO.File.Exists(path))
                return BadRequest("Token inválido");

            var data = _excel.ReadPreview(path, int.MaxValue); // aquí leerías todo y lo mapearías/guardarías
            System.IO.File.Delete(path);

            return Ok(new { imported = data.Count });
        }
        public class ExcelPreviewForm
        {
            // nombre del campo en el form-data: "file"
            [FromForm(Name = "file")]
            public IFormFile File { get; set; } = default!;

            // si quieres permitir cambiar el número de filas en el mismo form-data
            [FromForm(Name = "previewRows")]
            public int PreviewRows { get; set; } = 50;
        }
    }
}
