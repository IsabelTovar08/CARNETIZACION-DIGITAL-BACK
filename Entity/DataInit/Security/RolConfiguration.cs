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
    public class RolConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(
                // SuperAdmin: acceso total global
                new Role { Id = 1, Name = "SuperAdmin", Description = "Acceso total al sistema.", HasAllPermissions = true },

                // OrgAdmin: TODO en Carnets y Eventos (de su organización)
                new Role { Id = 2, Name = "OrgAdmin", Description = "Administra carnets y eventos de su organización.", HasAllPermissions = false },

                // Supervisor: SOLO Eventos
                new Role { Id = 3, Name = "Supervisor", Description = "Gestiona únicamente los eventos (creación, control y reportes).", HasAllPermissions = false },

                //Administrativos de la organización
                new Role { Id = 4, Name = "Administrativo", Description = "Funcionario (docentes, coordinadores, etc.) con visualización de su propio carnet.", HasAllPermissions = false },

                // Opcionales de lectura/uso básico
                new Role { Id = 5, Name = "Estudiante", Description = "Consulta su propio carnet y asistencia.", HasAllPermissions = false },
                new Role { Id = 6, Name = "Usuario", Description = "Acceso mínimo/público.", HasAllPermissions = false }
            );

            builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false);

            builder
           .HasIndex(f => f.Name)
           .IsUnique();

            builder.ToTable("Roles", schema: "ModelSecurity");
        }
    }
}
