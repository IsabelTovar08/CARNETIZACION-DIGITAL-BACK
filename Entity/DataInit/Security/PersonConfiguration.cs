using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Models;
using Entity.Models.ModelSecurity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.DataInit.Security

{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.HasData(
                 new Person { Id = 1, FirstName = "Demo", LastName = "Funcionario", DocumentNumber = "1234567890", Phone = "3200001111" },
                 new Person { Id = 2, FirstName = "Laura", LastName = "Estudiante", DocumentNumber = "9876543210", Phone = "3100002222"  },
                 new Person { Id = 3, FirstName = "Ana", LastName = "Administrador", DocumentNumber = "1122334455", Phone = "3001234567"},
                 new Person { Id = 4, FirstName = "José", LastName = "Usuario", DocumentNumber = "9988776655", Phone = "3151234567"},
                 new Person { Id = 5, FirstName = "María", LastName = "Tovar", DocumentNumber = "1234561630", Phone = "3200056311" },
                 new Person { Id = 6, FirstName = "Camilo", LastName = "Charry", DocumentNumber = "1245567890", Phone = "3200014311" },
                 new Person { Id = 7, FirstName = "Marcos", LastName = "Alvarez", DocumentNumber = "1235267890", Phone = "320026111" }

             );

            builder
           .HasIndex(f => f.DocumentNumber)
           .IsUnique();

            builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false);

            builder.ToTable("People", schema: "ModelSecurity");
        }
    }
}
