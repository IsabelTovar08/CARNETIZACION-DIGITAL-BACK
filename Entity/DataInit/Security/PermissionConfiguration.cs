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
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.HasData(
                 new Permission { Id = 1, Name = "crear", Description = "Puede crear nuevos registros" },
                 new Permission { Id = 2, Name = "editar", Description = "Puede editar registros existentes" },
                 new Permission { Id = 3, Name = "validar", Description = "Puede validar datos (correo, QR)" }
             );

            builder
           .HasIndex(f => f.Name)
           .IsUnique();

            builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false);

            builder.ToTable("Permissions", schema: "ModelSecurity");
        }
    }
}
