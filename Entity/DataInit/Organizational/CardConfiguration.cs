using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Models.Organizational.Assignment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.DataInit.Organizational
{
    public class CardConfiguration : IEntityTypeConfiguration<Card>
    {
        public  void Configure(EntityTypeBuilder<Card> builder)
        {
            builder.HasData(
                new Card
                {
                    Id = 1,
                    QRCode = "QR12345",
                    CreationDate = DateTime.Parse("2024-01-01"),
                    ExpirationDate = DateTime.Parse("2025-01-01"),
                    StatusId = 1,
                    PersonDivissionProfileId = 1,
                    IsDeleted = false
                }
            );

            // Relaciones

            builder.HasOne(c => c.PersonDivisionProfile)
                   .WithOne(pdp => pdp.Card)
                   .HasForeignKey<Card>(c => c.PersonDivissionProfileId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Status)
                   .WithMany()
                   .HasForeignKey(c => c.StatusId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Tabla y esquema
            builder.ToTable("Cards", schema: "Organizational");
        }
    }
}
