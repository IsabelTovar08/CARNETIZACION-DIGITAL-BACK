using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Models.Parameter;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.DataInit.Parameter
{
    public class TypeCategoryConfiguration : IEntityTypeConfiguration<TypeCategory>
    {
        public void Configure(EntityTypeBuilder<TypeCategory> builder)
        {
            builder.HasData(
                new TypeCategory { Id = 1, Name = "Organización", IsDeleted = false },
                new TypeCategory { Id = 2, Name = "Punto de acceso", IsDeleted = false },
                new TypeCategory { Id = 3, Name = "Notificación", IsDeleted = false },
                new TypeCategory { Id = 4, Name = "Tipo de documento", IsDeleted = false },
                new TypeCategory { Id = 5, Name = "Tipo de sangre", IsDeleted = false },
                new TypeCategory { Id = 6, Name = "Filtros para eventos privados", IsDeleted = false }

            );

            builder.HasIndex(tc => tc.Name).IsUnique();
            builder.Property(tc => tc.IsDeleted).HasDefaultValue(false);

            builder.HasMany(tc => tc.Types)
                   .WithOne(t => t.TypeCategory)
                   .HasForeignKey(t => t.TypeCategoryId);

            builder.ToTable("TypeCategories", schema: "Parameter");
        }
    }
}
