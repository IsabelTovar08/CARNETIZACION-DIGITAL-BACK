using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Models.Organizational;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.DataInit.Operational
{

    public class AccessPointConfiguration : IEntityTypeConfiguration<AccessPoint>
    {
        public void Configure(EntityTypeBuilder<AccessPoint> builder)
        {
            builder.HasData(
                new AccessPoint
                {
                    Id = 1,
                    Name = "Punto Norte",
                    Description = "Acceso norte del evento",
                    EventId = 1,
                    TypeId = 1
                },
                new AccessPoint
                {
                    Id = 2,
                    Name = "Punto Sur",
                    Description = "Acceso sur del evento",
                    EventId = 1,
                    TypeId = 2
                },
                new AccessPoint
                {
                    Id = 3,
                    Name = "Punto Principal",
                    Description = "Acceso principal",
                    EventId = 2,
                    TypeId = 1
                }
            );

            builder.Property(x => x.IsDeleted)
                   .HasDefaultValue(false);

            // Relación con CustomType
            builder.HasOne(ap => ap.AccessPointType)
                   .WithMany()
                   .HasForeignKey(ap => ap.TypeId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Relación con Event
            builder.HasOne(ap => ap.Event)
                   .WithMany(e => e.AccessPoints)
                   .HasForeignKey(ap => ap.EventId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Columna grande para el Base64 del QR
            builder.Property(ap => ap.QrCode)
       .HasColumnType("nvarchar(max)")
       .IsRequired(false);


            builder.ToTable("AccessPoints", schema: "Operational");
        }
    }
}
