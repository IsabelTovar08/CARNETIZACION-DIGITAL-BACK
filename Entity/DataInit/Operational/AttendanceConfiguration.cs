using System;
using Entity.Models.Organizational;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.DataInit.Operational
{
    public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
    {
        public void Configure(EntityTypeBuilder<Attendance> builder)
        {
            // Datos iniciales (puedes dejar TimeOfExit con valor o null)
            builder.HasData(
               new Attendance
               {
                   Id = 1,
                   PersonId = 1,
                   TimeOfEntry = DateTime.Parse("2023-01-01 08:00:00"),
                   TimeOfExit = DateTime.Parse("2023-01-01 12:00:00"),
                   AccessPointOfEntry = 1,
                   AccessPointOfExit = 2,
                   IsDeleted = false
               },
               new Attendance
               {
                   Id = 2,
                   PersonId = 2,
                   TimeOfEntry = DateTime.Parse("2023-01-01 09:30:00"),
                   TimeOfExit = DateTime.Parse("2023-01-01 13:45:00"),
                   AccessPointOfEntry = 1,
                   AccessPointOfExit = 2,
                   IsDeleted = false
               }
            );

            // Relaciones
            builder.HasOne(a => a.AccessPointEntry)
                   .WithMany(ap => ap.AttendancesEntry)
                   .HasForeignKey(a => a.AccessPointOfEntry)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.AccessPointExit)
                   .WithMany(ap => ap.AttendancesExit)
                   .HasForeignKey(a => a.AccessPointOfExit)
                   .OnDelete(DeleteBehavior.Restrict);

            // Propiedades
            builder.Property(a => a.TimeOfEntry).IsRequired();

            // Tu modelo es DateTime? -> debe ser NO requerido en la BD
            builder.Property(a => a.TimeOfExit).IsRequired(false);

            //  QrCode 
            builder.Property(a => a.QrCode)
                   .HasColumnType("nvarchar(max)")
                   .IsRequired(false);

            builder.Property(x => x.IsDeleted).HasDefaultValue(false);

            // Esquema y tabla
            builder.ToTable("Attendances", schema: "Operational");
        }
    }
}
