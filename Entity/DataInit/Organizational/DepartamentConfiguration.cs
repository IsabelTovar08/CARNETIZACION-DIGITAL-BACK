using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Models.Organizational.Location;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.DataInit.Organizational
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasData(
                new Department { Id = 1, Name = "Cundinamarca", IsDeleted = false },
                new Department { Id = 2, Name = "Antioquia", IsDeleted = false },
                new Department { Id = 3, Name = "Valle del Cauca",  IsDeleted = false }
            );

            builder.HasIndex(d => d.Name)
                   .IsUnique();

            builder.Property(d => d.IsDeleted)
                   .HasDefaultValue(false);

            builder.ToTable("Departments", schema: "Organizational");
        }
    }
}
