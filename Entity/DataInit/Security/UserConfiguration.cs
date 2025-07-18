using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.DataInit.Security

{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(
                new User { Id = 1, UserName = "admin", Email = "funcionario@carnet.edu", Password = "123", PersonId = 1 },
                new User { Id = 2, Email = "laura.estudiante@correo.com", Password = "L4d!Estudiante2025", PersonId = 2 },
                new User { Id = 3, Email = "admin@carnet.edu", Password = "Adm!nCarnet2025", PersonId = 3 },
                new User { Id = 4, Email = "usuario@carnet.edu", Password = "Usr!Carnet2025", PersonId = 4 }
            );

            builder
           .HasIndex(f => f.Email)
           .IsUnique();

            builder.ToTable("Users", schema: "ModelSecurity"); 

            builder.HasOne(e => e.Person)
               .WithMany(c => c.Users)
               .HasForeignKey(e => e.PersonId);
        }
    }
}
