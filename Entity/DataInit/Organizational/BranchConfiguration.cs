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
    public class BranchConfiguration : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.HasData(
                new Branch
                {
                    Id = 1,
                    Name = "Sucursal Principal",
                    Location = "Centro",
                    Phone = "123456789",
                    Email = "principal@org.com",
                    Address = "Calle 1 # 2-34",
                    CityId = 1,
                    OrganizationId = 1,
                    IsDeleted = false
                },
                new Branch
                {
                    Id = 2,
                    Name = "Sucursal Norte",
                    Location = "Zona Norte",
                    Phone = "987654321",
                    Email = "norte@org.com",
                    Address = "Carrera 45 # 67-89",
                    CityId = 1,
                    OrganizationId = 1,
                    IsDeleted = false
                }
            );

            builder.HasIndex(b => b.Name).IsUnique();

            builder.Property(b => b.IsDeleted)
                .HasDefaultValue(false);

            builder.ToTable("Branches", schema: "Organizational");
        }
    }
}
