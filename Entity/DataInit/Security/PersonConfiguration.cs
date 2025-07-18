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
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.HasData(
                 new Person { Id = 1, FirstName = "Carlos", LastName = "Funcionario", Identification = "1234567890", Phone = "3200001111" },
                 new Person { Id = 2, FirstName = "Laura", LastName = "Estudiante", Identification = "9876543210", Phone = "3100002222" },
                 new Person { Id = 3, FirstName = "Ana", LastName = "Administrador", Identification = "1122334455", Phone = "3001234567" },
                 new Person { Id = 4, FirstName = "José", LastName = "Usuario", Identification = "9988776655", Phone = "3151234567" }
             );

            builder
           .HasIndex(f => f.Identification)
           .IsUnique();

            builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false);

            builder.ToTable("People", schema: "ModelSecurity");
        }
    }
}
