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
    public class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.HasData(
                new City { Id = 1, Name = "Bogotá", DeparmentId = 1, IsDeleted = false },
                new City { Id = 2, Name = "Medellín", DeparmentId = 2, IsDeleted = false },
                new City { Id = 3, Name = "Cali", DeparmentId = 3, IsDeleted = false }
            );

            builder.HasIndex(c => c.Name)
                   .IsUnique();

            builder.Property(c => c.IsDeleted)
                   .HasDefaultValue(false);

            builder.ToTable("Cities", schema: "Organizational");
        }
    }
}
