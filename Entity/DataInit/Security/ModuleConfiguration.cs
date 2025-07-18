using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.DataInit.Security
{
    public class ModuleConfiguration : IEntityTypeConfiguration<Module>
    {
        public void Configure(EntityTypeBuilder<Module> builder)
        {
            builder.HasData(
                new Module { Id = 1, Name = "Carnetización", Description = "Gestión y emisión de carnets digitales" },
                new Module { Id = 2, Name = "Validación", Description = "Validación de identidad y correos" },
                new Module { Id = 3, Name = "Asistencia", Description = "Módulo para control de asistencia en eventos/clases" }
            );

            builder
           .HasIndex(f => f.Name)
           .IsUnique();

            builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false);

            builder.ToTable("Modules", schema: "ModelSecurity");
        }
    }
}
