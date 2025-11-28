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
    public class IssuedCardConfiguration : IEntityTypeConfiguration<IssuedCard>
    {
        public void Configure(EntityTypeBuilder<IssuedCard> builder)
        {
            builder.HasData(
                new IssuedCard
                {
                    Id = 1,
                    PersonId = 1,
                    SheduleId = 1, // Estudiante
                    InternalDivisionId = 1,
                    IsCurrentlySelected = true,
                    IsDeleted = false,
                    QRCode = "QR-0001",
                    UniqueId = new Guid(),
                    CardId = 1,
                    StatusId = 1
                },
                new IssuedCard
                {
                    Id = 2,
                    PersonId = 2,
                    SheduleId = 2, // Profesor
                    InternalDivisionId = 1,
                    IsCurrentlySelected = true,
                    IsDeleted = false,
                    QRCode = "QR-0002",
                    UniqueId = new Guid(),
                    CardId = 1,
                    StatusId = 1
                },
                new IssuedCard
                {
                    Id = 3,
                    PersonId = 5,
                    SheduleId = 2, // Profesor
                    InternalDivisionId = 1,
                    IsCurrentlySelected = true,
                    IsDeleted = false,
                    QRCode = "QR-0003",
                    UniqueId = new Guid(),
                    CardId = 1,
                    StatusId = 1
                },
                new IssuedCard
                {
                    Id = 4,
                    PersonId = 6,
                    SheduleId = 2, // Profesor
                    InternalDivisionId = 1,
                    IsCurrentlySelected = true,
                    IsDeleted = false,
                    QRCode = "QR-0004",
                    UniqueId = new Guid(),
                    CardId = 1,
                    StatusId = 1
                },
                new IssuedCard
                {
                    Id = 5,
                    PersonId = 7,
                    SheduleId = 2, // Profesor
                    InternalDivisionId = 1,
                    IsCurrentlySelected = true,
                    IsDeleted = false,
                    QRCode = "QR-0005",
                    UniqueId = new Guid(),
                    CardId = 1,
                    StatusId = 1
                }
            );

            builder.HasIndex(p => new { p.SheduleId, p.InternalDivisionId })
                   .IsUnique(false);

            builder.HasOne(x => x.Person)
              .WithMany(x => x.IssuedCard)
              .HasForeignKey(x => x.PersonId)
              .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.PersonId).IsUnique(false);

            builder.Property(x => x.IsCurrentlySelected)
                   .HasDefaultValue(false);

            builder.Property(x => x.IsDeleted)
                   .HasDefaultValue(false);

            builder.ToTable("IssuedCards", schema: "Organizational");
        }
    }
}
