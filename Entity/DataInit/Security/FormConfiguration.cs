using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Entity.Models;
using Entity.Models.ModelSecurity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.DataInit.Security
{
    public class FormConfiguration : IEntityTypeConfiguration<Form>
    {
        public void Configure(EntityTypeBuilder<Form> builder)
        {
            builder.ToTable("Forms", schema: "ModelSecurity");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(120).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(500);
            builder.Property(x => x.Icon).HasMaxLength(50);
            builder.Property(x => x.Url).HasMaxLength(300).IsRequired();
            builder.Property(x => x.IsDeleted).HasDefaultValue(false);

            builder.HasIndex(f => f.Name).IsUnique();
            builder.HasIndex(f => f.Url).IsUnique(); // recomendable

            builder.HasOne(f => f.Module)
                   .WithMany(m => m.Forms)
                   .HasForeignKey(f => f.ModuleId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Soft delete
            //builder.HasQueryFilter(x => !x.IsDeleted);

            // ====== SEED ======
            builder.HasData(
              // Menú Principal (ModuleId = 1)
              new Form { Id = 1, Name = "Inicio", Description = "Panel principal", Icon = "home", Url = "/dashboard", ModuleId = 1 },
              new Form { Id = 2, Name = "Ayuda", Description = "Centro de ayuda y documentación", Icon = "help", Url = "/dashboard/ayuda", ModuleId = 1 },

              // Organizacional >  ModuleId = 2
              new Form { Id = 3, Name = "Resumen", Description = "Vista general de la estructura", Icon = "dashboard_customize", Url = "/dashboard/organizational/structure", ModuleId = 2 },
              new Form { Id = 4, Name = "Sucursales", Description = "Administración de sucursales", Icon = "store", Url = "/dashboard/organizational/structure/branch", ModuleId = 2 },
              new Form { Id = 5, Name = "Unidades Organizativas", Description = "Gestión de unidades organizativas", Icon = "schema", Url = "/dashboard/organizational/structure/unit", ModuleId = 2 },
              new Form { Id = 6, Name = "Divisiones Internas", Description = "Administración de divisiones internas", Icon = "account_tree", Url = "/dashboard/organizational/structure/internal-division", ModuleId = 2 },
              new Form { Id = 7, Name = "Perfiles", Description = "Perfiles de las personas en el sistema", Icon = "badge", Url = "/dashboard/organizational/profile", ModuleId = 2 },
              new Form { Id = 8, Name = "Jornadas", Description = "Configuración de horarios/jornadas", Icon = "schedule", Url = "/dashboard/organizational/structure/schedule", ModuleId = 2 },

              // Operacional >  ModuleId = 3
              new Form { Id = 9, Name = "Eventos", Description = "Gestión de eventos", Icon = "event", Url = "/dashboard/operational/events", ModuleId = 3 },
              new Form { Id = 10, Name = "Tipos de Evento", Description = "Catálogo de tipos de evento", Icon = "category", Url = "/dashboard/operational/event-types", ModuleId = 3 },
              new Form { Id = 11, Name = "Puntos de Acceso", Description = "Administración de puntos de acceso", Icon = "sensor_door", Url = "/dashboard/operational/access-points", ModuleId = 3 },
              new Form { Id = 12, Name = "Asistencias", Description = "Registro y consulta de asistencias", Icon = "how_to_reg", Url = "/dashboard/operational/attendance", ModuleId = 3 },

              // Parámetros > Configuración General / Ubicación  =>  ModuleId = 4
              new Form { Id = 13, Name = "Estados", Description = "Estados del sistema", Icon = "check_circle_unread", Url = "/dashboard/parametros/status", ModuleId = 4 },
              new Form { Id = 14, Name = "Tipos y Categorías", Description = "Tipos y categorías del sistema", Icon = "category", Url = "/dashboard/parametros/types-category", ModuleId = 4 },
              new Form { Id = 15, Name = "Menu", Description = "Configuración del Menú del sistema", Icon = "background_dot_small", Url = "/dashboard/parametros/menu", ModuleId = 4 },
              new Form { Id = 16, Name = "Departamentos", Description = "Catálogo de departamentos", Icon = "flag", Url = "/dashboard/organizational/location/department", ModuleId = 4 },
              new Form { Id = 17, Name = "Municipios", Description = "Catálogo de municipios", Icon = "place", Url = "/dashboard/organizational/location/municipality", ModuleId = 4 },

              // Seguridad > ModuleId = 5
              new Form { Id = 18, Name = "Personas", Description = "Gestión de personas", Icon = "person_pin_circle", Url = "/dashboard/seguridad/people", ModuleId = 5 },
              new Form { Id = 19, Name = "Usuarios", Description = "Gestión de usuarios", Icon = "groups_2", Url = "/dashboard/seguridad/users", ModuleId = 5 },
              new Form { Id = 20, Name = "Roles", Description = "Gestión de roles", Icon = "add_moderator", Url = "/dashboard/seguridad/roles", ModuleId = 5 },
              new Form { Id = 21, Name = "Gestión de Permisos", Description = "Permisos por formulario", Icon = "folder_managed", Url = "/dashboard/seguridad/permission-forms", ModuleId = 5 },
              new Form { Id = 22, Name = "Permisos", Description = "Catálogo de permisos", Icon = "lock_open_circle", Url = "/dashboard/seguridad/permissions", ModuleId = 5 },
              new Form { Id = 23, Name = "Formularios", Description = "Catálogo de formularios", Icon = "lists", Url = "/dashboard/seguridad/forms", ModuleId = 5 },
              new Form { Id = 24, Name = "Módulos", Description = "Catálogo de módulos", Icon = "dashboard_2", Url = "/dashboard/seguridad/modules", ModuleId = 5 }
          );

        }
    }
}
