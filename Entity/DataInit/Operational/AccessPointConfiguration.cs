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
                    TypeId = 1
                },
                new AccessPoint
                {
                    Id = 2,
                    Name = "Punto Sur",
                    Description = "Acceso sur del evento",
                    TypeId = 2
                },
                new AccessPoint
                {
                    Id = 3,
                    Name = "Punto Principal",
                    Description = "Acceso principal",
                    TypeId = 1
                }
            );

            builder.Property(x => x.IsDeleted)
                   .HasDefaultValue(false);

            builder.HasOne(ap => ap.AccessPointType)
                   .WithMany()
                   .HasForeignKey(ap => ap.TypeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(ap => ap.QrCode)
                   .HasColumnType("nvarchar(max)")
                   .IsRequired(false);

            builder.ToTable("AccessPoints", schema: "Operational");
        }

    }
}
