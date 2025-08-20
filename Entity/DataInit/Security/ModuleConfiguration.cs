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
            builder.ToTable("Modules", schema: "ModelSecurity");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(120).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(500);
            builder.Property(x => x.Icon).HasMaxLength(50);
            builder.Property(x => x.IsDeleted).HasDefaultValue(false);

            // Clave natural recomendable
            builder.HasIndex(f => f.Name).IsUnique();

            // Soft delete
            //builder.HasQueryFilter(x => !x.IsDeleted);

            builder.HasData(
                // 1: Menú Principal
                new Module { Id = 1, Name = "Menú Principal", Description = "Grupo principal de navegación", Icon = "home" },

                // 2: Organizacional
                new Module { Id = 2, Name = "Organizacional", Description = "Dominio Organizacional", Icon = "apartment" },

                // 4: Operacional
                new Module { Id = 3, Name = "Operacional", Description = "Dominio Operacional", Icon = "event_available" },

                // 6: Parámetros
                new Module { Id = 4, Name = "Parámetros", Description = "Parámetros y configuración", Icon = "settings_applications" },

                // 9: Seguridad
                new Module { Id = 5, Name = "Seguridad", Description = "Dominio de seguridad", Icon = "admin_panel_settings" }
            );

        }
    }
}
