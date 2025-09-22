using Business.Interfaces.Operational;
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.DTOs.Reports;
using Entity.Models.Organizational;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Web.Controllers.Base;

namespace Web.Controllers.Operational
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AttendanceController : GenericController<Attendance, AttendanceDtoRequest, AttendanceDtoResponse>
    {
        private readonly IAttendanceBusiness _attendanceBusiness;
        private readonly IAccessPointBusiness _accessPointBusiness;
        private readonly ILogger<AttendanceController> _logger;

        public AttendanceController(
            IAttendanceBusiness attendanceBusiness,
            IAccessPointBusiness accessPointBusiness,
            ILogger<AttendanceController> logger
        ) : base(attendanceBusiness, logger)
        {
            _attendanceBusiness = attendanceBusiness;
            _accessPointBusiness = accessPointBusiness;
            _logger = logger;
        }

        // ---------------- EXISTENTES ----------------

        [HttpPost("scan")]
        public async Task<IActionResult> RegisterAttendance([FromBody] AttendanceDtoRequest dto)
        {
            if (dto == null)
                return BadRequest(new { success = false, message = "Solicitud inválida: body vacío." });

            var result = await _attendanceBusiness.RegisterAttendanceAsync(dto);
            if (result == null)
                return BadRequest(new { success = false, message = "No se pudo registrar la asistencia." });

            return Ok(new { success = true, message = "Asistencia registrada correctamente.", data = result });
        }

        [HttpPost("register-entry")]
        public async Task<IActionResult> RegisterEntry([FromBody] AttendanceDtoRequestSpecific dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _attendanceBusiness.RegisterEntryAsync(dto);
                return Ok(new { success = true, message = "Entrada registrada correctamente.", data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar ENTRADA.");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("register-exit")]
        public async Task<IActionResult> RegisterExit([FromBody] AttendanceDtoRequestSpecific dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _attendanceBusiness.RegisterExitAsync(dto);
                return Ok(new { success = true, message = "Salida registrada correctamente.", data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar SALIDA.");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] int? personId,
            [FromQuery] int? eventId,
            [FromQuery] DateTime? fromUtc,
            [FromQuery] DateTime? toUtc,
            [FromQuery] string? sortBy = "TimeOfEntry",
            [FromQuery] string? sortDir = "DESC",
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken ct = default)
        {
            var (items, total) = await _attendanceBusiness.SearchAsync(
                personId, eventId, fromUtc, toUtc, sortBy, sortDir, page, pageSize, ct);

            if (total == 0)
                return Ok(new { items = Array.Empty<object>(), total = 0, page, pageSize, message = "Sin resultados." });

            return Ok(new { items, total, page, pageSize });
        }

        [HttpPost("register-by-qr")]
        public async Task<IActionResult> RegisterByQr([FromBody] AttendanceDtoRequest dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.QrCode))
                return BadRequest(new { success = false, message = "Debe incluir el QrCode y el PersonId." });

            var result = await _accessPointBusiness.RegisterAttendanceByQrAsync(dto.QrCode, dto.PersonId);

            if (result == null || !result.Success)
                return BadRequest(new { success = false, message = result?.Message ?? "No se pudo registrar la asistencia." });

            return Ok(new { success = true, message = result.Message, data = result });
        }

        [HttpPost("report")]
        public async Task<IActionResult> GetReport([FromBody] ReportFilterDto filters, CancellationToken ct = default)
        {
            var result = await _attendanceBusiness.GetReportAsync(filters, ct);

            if (result == null || result.Count == 0)
                return Ok(new { success = true, message = "Sin resultados.", data = Array.Empty<object>() });

            return Ok(new { success = true, total = result.Count, data = result });
        }

        // ---------------- NUEVO: PDF estilizado ----------------
        [HttpPost("report-pdf")]
        public async Task<IActionResult> GetReportPdf([FromBody] ReportFilterDto filters, CancellationToken ct = default)
        {
            var result = await _attendanceBusiness.GetReportAsync(filters, ct);
            if (result == null || result.Count == 0)
                return BadRequest(new { success = false, message = "No hay datos para generar PDF." });

            var doc = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Size(PageSizes.A4);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    page.Header()
                        .Text("📋 Reporte de Asistencias")
                        .FontSize(18)
                        .Bold()
                        .AlignCenter();

                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(120); // Persona
                            columns.RelativeColumn();    // Evento
                            columns.ConstantColumn(90);  // Entrada
                            columns.ConstantColumn(90);  // Salida
                            columns.RelativeColumn();    // Punto Entrada
                            columns.RelativeColumn();    // Punto Salida
                        });

                        // Cabeceras
                        string[] headers = { "Persona", "Evento", "Entrada", "Salida", "Punto Entrada", "Punto Salida" };
                        table.Header(header =>
                        {
                            foreach (var h in headers)
                            {
                                header.Cell().Background("#DDDDDD").Padding(5).Text(h).Bold().FontSize(11).AlignCenter();
                            }
                        });

                        // Filas
                        foreach (var r in result)
                        {
                            table.Cell().BorderBottom(0.5f).Padding(4).Text(r.PersonFullName ?? "-");
                            table.Cell().BorderBottom(0.5f).Padding(4).Text(r.EventName ?? "-");
                            table.Cell().BorderBottom(0.5f).Padding(4).Text(r.TimeOfEntryStr ?? "-");
                            table.Cell().BorderBottom(0.5f).Padding(4).Text(r.TimeOfExitStr ?? "-");
                            table.Cell().BorderBottom(0.5f).Padding(4).Text(r.AccessPointOfEntryName ?? "-");
                            table.Cell().BorderBottom(0.5f).Padding(4).Text(r.AccessPointOfExitName ?? "-");
                        }
                    });

                    page.Footer()
                        .AlignRight()
                        .Text($"Generado el {DateTime.Now:dd/MM/yyyy HH:mm}")
                        .FontSize(9);
                });
            });

            byte[] pdfBytes = doc.GeneratePdf();
            return File(pdfBytes, "application/pdf", $"ReporteAsistencias_{DateTime.UtcNow:yyyyMMdd}.pdf");
        }

        // ---------------- NUEVO: Excel estilizado ----------------
        [HttpPost("report-excel")]
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReportExcel([FromBody] ReportFilterDto filters, CancellationToken ct)
        {
            var data = await _attendanceBusiness.GetReportAsync(filters, ct);

            if (data == null || !data.Any())
                return Ok(new { success = true, message = "Sin resultados para exportar.", data = Array.Empty<object>() });

            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("Reporte Asistencias");

            string[] headers = { "Persona", "Evento", "Entrada", "Salida", "Punto Entrada", "Punto Salida" };

            // Cabeceras
            for (int i = 0; i < headers.Length; i++)
                ws.Cells[1, i + 1].Value = headers[i];

            using (var range = ws.Cells[1, 1, 1, headers.Length])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            }

            // Datos
            int row = 2;
            foreach (var item in data)
            {
                ws.Cells[row, 1].Value = item.PersonFullName;
                ws.Cells[row, 2].Value = item.EventName;
                ws.Cells[row, 3].Value = item.TimeOfEntryStr;
                ws.Cells[row, 4].Value = item.TimeOfExitStr;
                ws.Cells[row, 5].Value = item.AccessPointOfEntryName;
                ws.Cells[row, 6].Value = item.AccessPointOfExitName;

                for (int col = 1; col <= headers.Length; col++)
                    ws.Cells[row, col].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Hair);

                row++;
            }

            ws.Cells[ws.Dimension.Address].AutoFitColumns();

            var fileBytes = package.GetAsByteArray();
            var fileName = $"ReporteAsistencias_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
