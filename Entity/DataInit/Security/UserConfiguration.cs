using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Entity.Models;
using Entity.Models.ModelSecurity;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.DataInit.Security

{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(
                new User { Id = 1, UserName = "admin", Password = "123", PersonId = 1, Active = true },
                new User { Id = 2, Password = "L4d!Estudiante2025", PersonId = 2, Active = false },
                new User { Id = 3, Password = "Adm!nCarnet2025", PersonId = 3, Active = false },
                new User { Id = 4, Password = "Usr!Carnet2025", PersonId = 4, Active = false }
            );

            builder
           .HasIndex(f => f.UserName)
           .IsUnique();

            builder.ToTable("Users", schema: "ModelSecurity");
            builder.Property(u => u.Active)
           .HasDefaultValue(false);

            builder
              .HasOne(u => u.Person)
                .WithOne(p => p.User)
                .HasForeignKey<User>(u => u.PersonId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
