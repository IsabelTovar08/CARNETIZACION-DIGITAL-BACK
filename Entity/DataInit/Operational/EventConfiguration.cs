using System;
using Entity.Models.Organizational;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.DataInit.Operational
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            
            builder.HasData(
                new Event
                {
                    Id = 1,
                    Name = "Conferencia de Tecnología",
                    Code = "TECH2025",
                    Description = "Evento principal de tecnología y software.",
                    ScheduleDate = DateTime.Parse("2023-07-30"),
                    ScheduleTime = DateTime.Parse("1900-01-01 10:00:00"),
                    EventStart = DateTime.Parse("2023-07-30 10:00:00"),
                    EventEnd = DateTime.Parse("2023-07-30 14:00:00"),
                    IsPublic = true,
                    StatusId = 1,
                    EventTypeId = 1,
                    IsDeleted = false
                },
                new Event
                {
                    Id = 2,
                    Name = "Charla de Salud",
                    Code = "SALUD2025",
                    Description = "Sesión sobre bienestar y salud ocupacional.",
                    ScheduleDate = DateTime.Parse("2023-08-05"),
                    ScheduleTime = DateTime.Parse("1900-01-01 09:00:00"),
                    EventStart = DateTime.Parse("2023-08-05 09:00:00"),
                    EventEnd = DateTime.Parse("2023-08-05 12:00:00"),
                    IsPublic = false,
                    StatusId = 1,
                    EventTypeId = 2,
                    IsDeleted = false
                }
            );

            //  Propiedades
            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Code)
                .IsRequired();

            builder.Property(e => e.IsPublic)
                .HasDefaultValue(true);

            builder.Property(e => e.IsDeleted)
                .HasDefaultValue(false);

            //  Relaciones
            builder.HasOne(e => e.EventType)
                   .WithMany()
                   .HasForeignKey(e => e.EventTypeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Status)
                   .WithMany()
                   .HasForeignKey(e => e.StatusId)
                   .OnDelete(DeleteBehavior.Restrict);

            //  Tabla destino
            builder.ToTable("Events", schema: "Operational");
        }
    }
}
