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
    public class PersonDivisionProfileConfiguration : IEntityTypeConfiguration<PersonDivisionProfile>
    {
        public void Configure(EntityTypeBuilder<PersonDivisionProfile> builder)
        {
            builder.HasData(
                new PersonDivisionProfile
                {
                    Id = 1,
                    PersonId = 1,
                    ProfileId = 1, // Estudiante
                    InternalDivisionId = 1,
                    IsCurrentlySelected = false,
                    IsDeleted = false
                },
                new PersonDivisionProfile
                {
                    Id = 2,
                    PersonId = 2,
                    ProfileId = 2, // Profesor
                    InternalDivisionId = 1,
                    IsCurrentlySelected = true,
                    IsDeleted = false
                }
            );

            builder.HasIndex(p => new { p.PersonId, p.ProfileId, p.InternalDivisionId })
                   .IsUnique();

            builder.Property(x => x.IsCurrentlySelected)
                   .HasDefaultValue(false);

            builder.Property(x => x.IsDeleted)
                   .HasDefaultValue(false);

            builder.ToTable("PersonDivisionProfiles", schema: "Organizational");
        }
    }
}
