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
                   new Role { Id = 1, Name = "Funcionario", Description = "Rol para personal autorizado a validar y emitir carnets" },
                   new Role { Id = 2, Name = "Estudiante", Description = "Rol con permisos limitados a visualización de carnet y asistencia" },
                   new Role { Id = 3, Name = "Admin", Description = "Acceso total al sistema de carnetización digital" },
                   new Role { Id = 4, Name = "Usuario", Description = "Acceso restringido, solo visualización" }
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
