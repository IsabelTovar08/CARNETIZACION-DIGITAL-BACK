using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Models.Organizational.Structure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.DataInit.Organizational
{
    public class OrganizationalUnitConfiguration : IEntityTypeConfiguration<OrganizationalUnit>
    {
        public void Configure(EntityTypeBuilder<OrganizationalUnit> builder)
        {
            builder.HasData(
                new OrganizationalUnit
                {
                    Id = 1,
                    Name = "Facultad de Ingeniería",
                    Description = null,
                    IsDeleted = false
                },
                new OrganizationalUnit
                {
                    Id = 2,
                    Name = "Facultad de Ciencias Económicas",
                    Description = null,
                    IsDeleted = false
                },
                new OrganizationalUnit
                {
                    Id = 3,
                    Name = "Facultad de Artes",
                    Description = null,
                    IsDeleted = false
                }
            );

            builder.HasIndex(o => o.Name).IsUnique();

            builder.Property(o => o.IsDeleted)
                .HasDefaultValue(false);

            builder.ToTable("OrganizationalUnits", schema: "Organizational");
        }
    }
}
