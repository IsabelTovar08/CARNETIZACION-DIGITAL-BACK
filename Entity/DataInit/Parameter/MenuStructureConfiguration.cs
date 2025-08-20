using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DataInit.Parameter
{
    using Entity.Models.ModelSecurity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class MenuStructureConfiguration : IEntityTypeConfiguration<MenuStructure>
    {
        public void Configure(EntityTypeBuilder<MenuStructure> builder)
        {
            builder.ToTable("MenuStructures", schema: "Parameter");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Type).HasMaxLength(20).IsRequired(); // group|collapse|item
            builder.Property(x => x.OrderIndex).IsRequired();

            builder.HasIndex(x => new { x.ParentMenuId, x.OrderIndex });

            // Check constraint para asegurar tipos válidos
            builder.ToTable(t => t.HasCheckConstraint("CK_MenuStructures_Type", "[Type] IN ('group','collapse','item')"));

            builder.HasOne(x => x.Parent)
                   .WithMany(x => x.Children)
                   .HasForeignKey(x => x.ParentMenuId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Module)
                   .WithMany()
                   .HasForeignKey(x => x.ModuleId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Form)
                   .WithMany()
                   .HasForeignKey(x => x.FormId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Nivel 0: Grupos (usan Module.Name/Icon)
            builder.HasData(
                new MenuStructure { Id = 1, ParentMenuId = null, ModuleId = 1, FormId = null, Type = "group", OrderIndex = 1 },
                new MenuStructure { Id = 2, ParentMenuId = null, ModuleId = 2, FormId = null, Type = "group", OrderIndex = 2 },
                new MenuStructure { Id = 3, ParentMenuId = null, ModuleId = 3, FormId = null, Type = "group", OrderIndex = 3 },
                new MenuStructure { Id = 4, ParentMenuId = null, ModuleId = 4, FormId = null, Type = "group", OrderIndex = 4 },
                new MenuStructure { Id = 5, ParentMenuId = null, ModuleId = 5, FormId = null, Type = "group", OrderIndex = 5 }
            );

            // Menú Principal → items (usan Form.Name/Icon)
            builder.HasData(
                new MenuStructure { Id = 6, ParentMenuId = 1, FormId = 1, Type = "item", OrderIndex = 1 },
                new MenuStructure { Id = 7, ParentMenuId = 1, FormId = 2, Type = "item", OrderIndex = 2 }
            );

            // Organizacional → collapse (requiere Title/Icon propios)
            builder.HasData(
                new MenuStructure { Id = 8, ParentMenuId = 2, Type = "collapse", OrderIndex = 1, Title = "Estructura Organizativa", Icon = "account_tree" }
            );

            // Organizacional → items
            builder.HasData(
                new MenuStructure { Id = 9, ParentMenuId = 8, FormId = 3, Type = "item", OrderIndex = 1 },
                new MenuStructure { Id = 10, ParentMenuId = 8, FormId = 4, Type = "item", OrderIndex = 2 },
                new MenuStructure { Id = 11, ParentMenuId = 8, FormId = 5, Type = "item", OrderIndex = 3 },
                new MenuStructure { Id = 12, ParentMenuId = 8, FormId = 6, Type = "item", OrderIndex = 4 },
                new MenuStructure { Id = 13, ParentMenuId = 8, FormId = 7, Type = "item", OrderIndex = 5 },
                new MenuStructure { Id = 14, ParentMenuId = 8, FormId = 8, Type = "item", OrderIndex = 6 }
            );

            // Operacional → collapse
            builder.HasData(
                new MenuStructure { Id = 15, ParentMenuId = 3, Type = "collapse", OrderIndex = 1, Title = "Eventos y Control de Acceso", Icon = "event_available" }
            );

            // Operacional → items
            builder.HasData(
                new MenuStructure { Id = 16, ParentMenuId = 15, FormId = 9, Type = "item", OrderIndex = 1 },
                new MenuStructure { Id = 17, ParentMenuId = 15, FormId = 10, Type = "item", OrderIndex = 2 },
                new MenuStructure { Id = 18, ParentMenuId = 15, FormId = 11, Type = "item", OrderIndex = 3 },
                new MenuStructure { Id = 19, ParentMenuId = 15, FormId = 12, Type = "item", OrderIndex = 4 }
            );

            // Parámetros → collapses
            builder.HasData(
                new MenuStructure { Id = 20, ParentMenuId = 4, Type = "collapse", OrderIndex = 1, Title = "Configuración General", Icon = "settings_applications" },
                new MenuStructure { Id = 21, ParentMenuId = 4, Type = "collapse", OrderIndex = 2, Title = "Ubicación", Icon = "location_on" }
            );

            // Parámetros → items
            builder.HasData(
                new MenuStructure { Id = 22, ParentMenuId = 20, FormId = 13, Type = "item", OrderIndex = 1 },
                new MenuStructure { Id = 23, ParentMenuId = 20, FormId = 14, Type = "item", OrderIndex = 2 },
                new MenuStructure { Id = 24, ParentMenuId = 20, FormId = 15, Type = "item", OrderIndex = 3 },

                new MenuStructure { Id = 25, ParentMenuId = 21, FormId = 16, Type = "item", OrderIndex = 1 },
                new MenuStructure { Id = 26, ParentMenuId = 21, FormId = 17, Type = "item", OrderIndex = 2 }
            );

            // Seguridad → collapse
            builder.HasData(
                new MenuStructure { Id = 27, ParentMenuId = 5,Type = "collapse", OrderIndex = 1, Title = "Gestión de Seguridad", Icon = "admin_panel_settings" }
            );

            // Seguridad → items
            builder.HasData(
                new MenuStructure { Id = 28, ParentMenuId = 27, FormId = 18, Type = "item", OrderIndex = 1 },
                new MenuStructure { Id = 29, ParentMenuId = 27, FormId = 19, Type = "item", OrderIndex = 2 },
                new MenuStructure { Id = 30, ParentMenuId = 27, FormId = 20, Type = "item", OrderIndex = 3 },
                new MenuStructure { Id = 31, ParentMenuId = 27, FormId = 21, Type = "item", OrderIndex = 4 },
                new MenuStructure { Id = 32, ParentMenuId = 27, FormId = 22, Type = "item", OrderIndex = 5 },
                new MenuStructure { Id = 33, ParentMenuId = 27, FormId = 23, Type = "item", OrderIndex = 6 },
                new MenuStructure { Id = 34, ParentMenuId = 27, FormId = 24, Type = "item", OrderIndex = 7 }
            );

        }
    }

}
