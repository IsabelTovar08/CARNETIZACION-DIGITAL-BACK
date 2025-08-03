using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entity.Models.Organizational.Structure;


namespace Entity.DataInit.Organizational
{
    public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
    {
        public void Configure(EntityTypeBuilder<Organization> builder)
        {
            builder.HasData(
                new Organization { Id = 1, Name = "Universidad Nacional", Description = "Institución de educación superior", Logo = "logo_unal.png", TypeId = 1, IsDeleted = false },
                new Organization { Id = 2, Name = "Hospital San José", Description = "Centro de atención médica", Logo = "logo_hsj.png", TypeId = 2, IsDeleted = false },
                new Organization { Id = 3, Name = "Fundación Futuro", Description = "Fundación sin ánimo de lucro", Logo = "logo_fundacion.png", TypeId = 3, IsDeleted = false }
            );

            builder.HasIndex(o => o.Name)
                   .IsUnique();

            builder.Property(o => o.IsDeleted)
                   .HasDefaultValue(false);

            builder.ToTable("Organizations", schema: "Organizational");
        }
    }
}
