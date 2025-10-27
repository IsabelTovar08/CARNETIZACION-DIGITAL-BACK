using System;
using System.Collections.Generic;
using System.IO;
using ClosedXML.Excel;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Entity.DTOs.Operational.Response;

namespace Utilities.Helpers
{
    public static class ExportHelper
    {
        // =========================
        //       EXPORT PDF
        // =========================
        public static byte[] ExportAttendancesToPdf(IEnumerable<AttendanceDtoResponse> data, string title)
        {
            using var stream = new MemoryStream();

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(40);

                    // --- Header ---
                    page.Header()
                        .AlignCenter()                          // <- al contenedor, antes de Text
                        .Text("📋 " + title)
                        .FontSize(18)
                        .Bold()
                        .FontColor("#2e2e2e");

                    // --- Table ---
                    page.Content().PaddingVertical(20).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(2); // Persona
                            columns.RelativeColumn(2); // Evento
                            columns.RelativeColumn(2); // Entrada
                            columns.RelativeColumn(2); // Salida
                            columns.RelativeColumn(2); // Punto Entrada
                            columns.RelativeColumn(2); // Punto Salida
                            columns.RelativeColumn(2); // Código
                        });

                        // Encabezados (AlignCenter ANTES de Text)
                        table.Header(header =>
                        {
                            string[] headers =
                            {
                                "Persona", "Evento", "Entrada", "Salida",
                                "Punto Entrada", "Punto Salida", "Código"
                            };

                            foreach (var h in headers)
                            {
                                header.Cell()
                                      .Background("#eeeeee")
                                      .Border(1)
                                      .BorderColor("#cccccc")
                                      .Padding(6)
                                      .AlignCenter()              // <- aquí
                                      .Text(h)                    // <- después el texto
                                      .Bold()
                                      .FontSize(11);
                            }
                        });

                        // Filas con zebra
                        bool zebra = false;
                        foreach (var item in data)
                        {
                            zebra = !zebra;
                            var values = new[]
                            {
                                item.PersonFullName ?? "",
                                item.EventName ?? "",
                                item.TimeOfEntryStr ?? item.TimeOfEntry.ToString("dd/MM/yyyy HH:mm"),
                                item.TimeOfExitStr ?? (item.TimeOfExit?.ToString("dd/MM/yyyy HH:mm") ?? ""),
                                item.AccessPointOfEntryName ?? "",
                                item.AccessPointOfExitName ?? "",
                                item.Code ?? ""
                            };

                            foreach (var val in values)
                            {
                                table.Cell()
                                     .Background(zebra ? "#f9f9f9" : "#ffffff")
                                     .BorderBottom(0.5f)
                                     .BorderColor("#cccccc")
                                     .Padding(5)
                                     .Text(val)
                                     .FontSize(10);
                            }
                        }
                    });

                    // --- Footer ---
                    page.Footer()
                        .AlignCenter()                          // <- al contenedor, antes de Text
                        .Text(txt =>
                        {
                            txt.Span("Generado el ").FontSize(9).FontColor("#666666");
                            txt.Span(DateTime.Now.ToString("dd/MM/yyyy HH:mm")).FontSize(9).FontColor("#666666");
                        });
                });
            }).GeneratePdf(stream);

            return stream.ToArray();
        }

        // =========================
        //      EXPORT EXCEL
        // =========================
        public static byte[] ExportAttendancesToExcel(IEnumerable<AttendanceDtoResponse> data, string sheetName)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(sheetName);

            string[] headers = {
                "Persona", "Evento", "Entrada", "Salida",
                "Punto Entrada", "Punto Salida", "Código"
            };

            // Título
            worksheet.Cell(1, 1).Value = "📋 " + sheetName;
            worksheet.Range(1, 1, 1, headers.Length).Merge()
                .Style.Font.SetBold().Font.FontSize = 16;
            worksheet.Range(1, 1, 1, headers.Length).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // Encabezados
            for (int i = 0; i < headers.Length; i++)
            {
                var cell = worksheet.Cell(3, i + 1);
                cell.Value = headers[i];
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.Gray;
                cell.Style.Font.FontColor = XLColor.White;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            }

            // Datos
            int row = 4;
            foreach (var item in data)
            {
                worksheet.Cell(row, 1).Value = item.PersonFullName ?? "";
                worksheet.Cell(row, 2).Value = item.EventName ?? "";
                worksheet.Cell(row, 3).Value = item.TimeOfEntryStr ?? item.TimeOfEntry.ToString("dd/MM/yyyy HH:mm");
                worksheet.Cell(row, 4).Value = item.TimeOfExitStr ?? (item.TimeOfExit?.ToString("dd/MM/yyyy HH:mm") ?? "");
                worksheet.Cell(row, 5).Value = item.AccessPointOfEntryName ?? "";
                worksheet.Cell(row, 6).Value = item.AccessPointOfExitName ?? "";
                worksheet.Cell(row, 7).Value = item.Code ?? "";
                row++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}
