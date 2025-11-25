using Entity.Models.Operational;
using Entity.Models.Organizational;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Business.Services.Supervisors
{
    public class EventAttendancePdfService : IEventAttendancePdfService
    {
        public async Task<byte[]> GenerateEventAttendancePdfAsync(
            Event ev,
            List<Attendance> attendees,
            List<EventSupervisor> supervisors)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(25);
                    page.Size(PageSizes.A4);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    // ================================
                    // ENCABEZADO
                    // ================================
                    page.Header().Column(col =>
                    {
                        col.Item().Text($"Reporte de Asistencia — {ev.Name}")
                            .Bold().FontSize(18).AlignCenter();

                        col.Item().Text($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}")
                            .FontSize(10).AlignCenter();

                        // ================================
                        // SUPERVISORES
                        // ================================
                        if (supervisors.Count > 0)
                        {
                            col.Item().PaddingTop(10).Text("Supervisores asignados:")
                                .Bold().FontSize(14);

                            foreach (var s in supervisors)
                            {
                                col.Item().Text(
                                    $"{s.User.Person.FirstName} {s.User.Person.LastName}  —  " +
                                    $"Documento: {s.User.Person.DocumentNumber}  —  " +
                                    $"Email: {s.User.Person.Email}"
                                )
                                .FontSize(11);
                            }
                        }
                    });

                    // ================================
                    // TABLA DE ASISTENCIA
                    // ================================
                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(c =>
                        {
                            c.ConstantColumn(40); // #
                            c.RelativeColumn(2);  // Nombre
                            c.RelativeColumn(2);  // Documento
                            c.RelativeColumn(2);  // Punto Entrada
                            c.RelativeColumn(2);  // Hora Entrada
                            c.RelativeColumn(2);  // Hora Salida
                            c.RelativeColumn(2); //Punto de salida

                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("#").Bold();
                            header.Cell().Text("Nombre").Bold();
                            header.Cell().Text("Documento").Bold();
                            header.Cell().Text("Punto Entrada").Bold();
                            header.Cell().Text("Hora Entrada").Bold();
                            header.Cell().Text("Hora Salida").Bold();
                            header.Cell().Text("Punto Salida").Bold();

                        });

                        int index = 1;

                        foreach (var a in attendees)
                        {
                            table.Cell().Text(index++);
                            table.Cell().Text($"{a.Person.FirstName} {a.Person.LastName}");
                            table.Cell().Text(a.Person.DocumentNumber);
                            table.Cell().Text(a.EventAccessPointEntry.AccessPoint.Name);
                            table.Cell().Text(a.TimeOfEntry.ToString("dd/MM/yyyy HH:mm"));
                            table.Cell().Text(a.TimeOfExit?.ToString("dd/MM/yyyy HH:mm") ?? "-");
                            table.Cell().Text(a.EventAccessPointExit?.AccessPoint?.Name ?? "Sin salida"
                            );

                        }
                    });

                    page.Footer().AlignRight()
                        .Text($"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}");
                });
            });

            using var stream = new MemoryStream();
            document.GeneratePdf(stream);
            return stream.ToArray();
        }
    }
}
