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
    public class UserRolConfiguration : IEntityTypeConfiguration<UserRoles>
    {
        public void Configure(EntityTypeBuilder<UserRoles> builder)
        {
            builder.HasData(
               new UserRoles { Id = 1, RolId = 1, UserId = 1 }, // Funcionario
               new UserRoles { Id = 2, RolId = 2, UserId = 2 }, // Estudiante
               new UserRoles { Id = 3, RolId = 3, UserId = 3 }, // Admin
               new UserRoles { Id = 4, RolId = 4, UserId = 4 },  // Usuario

               new UserRoles { Id = 5, RolId = 1, UserId = 2 },  // Usuario
               new UserRoles { Id = 6, RolId = 1, UserId = 3 },  // Usuario
               new UserRoles { Id = 7, RolId = 1, UserId = 4 }  // Usuario

           );

            builder.HasIndex(x => new { x.RolId, x.UserId }).IsUnique();

            builder.ToTable("UserRoles", schema: "ModelSecurity");
            builder.HasOne(e => e.User)
               .WithMany(c => c.UserRoles)
               .HasForeignKey(e => e.UserId);

            builder.HasOne(e => e.Rol)
               .WithMany(c => c.UserRoles)
               .HasForeignKey(e => e.RolId);

        }

    }
}
