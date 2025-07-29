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
    public class OrganizationalUnitBranchConfiguration : IEntityTypeConfiguration<OrganizationalUnitBranch>
    {
        public void Configure(EntityTypeBuilder<OrganizationalUnitBranch> builder)
        {
            builder.HasData(
                new OrganizationalUnitBranch { Id = 1, BranchId = 1, OrganizationUnitId = 1, IsDeleted = false }, // Ej: Ingeniería - Sede Medellín
                new OrganizationalUnitBranch { Id = 2, BranchId = 2, OrganizationUnitId = 1, IsDeleted = false }, // Ingeniería - Sede Bogotá
                new OrganizationalUnitBranch { Id = 3, BranchId = 1, OrganizationUnitId = 2, IsDeleted = false }, // Medicina - Sede Medellín
                new OrganizationalUnitBranch { Id = 4, BranchId = 2, OrganizationUnitId = 3, IsDeleted = false }  // Derecho - Sede Cali
            );

            builder.Property(x => x.IsDeleted)
                   .HasDefaultValue(false);

            builder.ToTable("OrganizationalUnitBranches", schema: "Organizational");
        }
    }
}
