using Business.Interfaces.Services;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using ClosedXML.Excel;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations;

namespace Business.Services.Export
{
    public class ExportService : IExportService
    {
        private string GetDisplayName(PropertyInfo prop)
        {
            var displayAttr = prop.GetCustomAttribute<DisplayAttribute>();
            return displayAttr?.Name ?? prop.Name; // usa el [Display(Name="...")] si existe
        }

        public async Task<byte[]> ExportToPdfAsync<T>(IEnumerable<T> data, string title, CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                QuestPDF.Settings.License = LicenseType.Community;

                using var stream = new MemoryStream();
                var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Margin(40);

                        // --- Header ---
                        page.Header().AlignCenter().Text("📋 " + title)
                            .FontSize(18).Bold().FontColor("#2e2e2e");

                        // --- Table ---
                        page.Content().PaddingVertical(20).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                foreach (var _ in properties)
                                    columns.RelativeColumn();
                            });

                            // Encabezados
                            table.Header(header =>
                            {
                                foreach (var prop in properties)
                                {
                                    var displayName = GetDisplayName(prop);
                                    header.Cell()
                                        .Background("#eeeeee")
                                        .Border(1)
                                        .BorderColor("#cccccc")
                                        .Padding(6)
                                        .Text(displayName)
                                        .Bold()
                                        .FontSize(11)
                                        .AlignCenter();
                                }
                            });

                            // Filas con zebra
                            bool zebra = false;
                            foreach (var item in data)
                            {
                                zebra = !zebra;
                                foreach (var prop in properties)
                                {
                                    var value = prop.GetValue(item)?.ToString() ?? "";
                                    table.Cell()
                                        .Background(zebra ? "#f9f9f9" : "#ffffff")
                                        .BorderBottom(0.5f)
                                        .BorderColor("#cccccc")
                                        .Padding(5)
                                        .Text(value)
                                        .FontSize(10);
                                }
                            }
                        });

                        // --- Footer ---
                        page.Footer().AlignCenter().Text(txt =>
                        {
                            txt.Span("Generado el ").FontSize(9).FontColor("#666666");
                            txt.Span(DateTime.Now.ToString("dd/MM/yyyy HH:mm")).FontSize(9).FontColor("#666666");
                        });
                    });
                }).GeneratePdf(stream);

                return stream.ToArray();
            }, cancellationToken);
        }

        public async Task<byte[]> ExportToExcelAsync<T>(IEnumerable<T> data, string sheetName, CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add(sheetName);

                var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                int currentRow = 1;

                // --- Título ---
                var titleCell = worksheet.Cell(currentRow, 1);
                titleCell.Value = "📋 " + sheetName;
                titleCell.Style.Font.Bold = true;
                titleCell.Style.Font.FontSize = 16;
                titleCell.Style.Font.FontColor = XLColor.DarkGreen;
                worksheet.Range(currentRow, 1, currentRow, properties.Length).Merge().Style
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                currentRow += 2;

                // --- Encabezados ---
                for (int i = 0; i < properties.Length; i++)
                {
                    var displayName = GetDisplayName(properties[i]);
                    var cell = worksheet.Cell(currentRow, i + 1);
                    cell.Value = displayName;
                    cell.Style.Font.Bold = true;
                    cell.Style.Fill.BackgroundColor = XLColor.Gray;
                    cell.Style.Font.FontColor = XLColor.White;
                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                }

                currentRow++;

                // --- Datos ---
                bool zebra = false;
                foreach (var item in data)
                {
                    zebra = !zebra;
                    for (int i = 0; i < properties.Length; i++)
                    {
                        var value = properties[i].GetValue(item)?.ToString() ?? "";
                        var cell = worksheet.Cell(currentRow, i + 1);
                        cell.Value = value;
                        cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                        if (zebra)
                            cell.Style.Fill.BackgroundColor = XLColor.LightGray;
                    }
                    currentRow++;
                }

                worksheet.Columns().AdjustToContents();

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                return stream.ToArray();
            }, cancellationToken);
        }
    }
}
