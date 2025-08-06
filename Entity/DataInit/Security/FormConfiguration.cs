using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.DataInit.Security
{
    public class FormConfiguration : IEntityTypeConfiguration<Form>
    {
        public void Configure(EntityTypeBuilder<Form> builder)
        {
            builder.HasData(
                  new Form { Id = 1, Name = "Crear Carnet", Description = "Formulario para generar un nuevo carnet digital",  Url = "/formulario" },
                  new Form { Id = 2, Name = "Validar Correo", Description = "Formulario para validar el correo del usuario", Url = "/formulario" },
                  new Form { Id = 3, Name = "Ver Carnet", Description = "Formulario donde se visualiza el carnet", Url = "/formulario" },
                  new Form { Id = 4, Name = "Control de Asistencia", Description = "Formulario para registrar y consultar asistencia", Url = "/formulario" }
              );

            builder
            .HasIndex(f => f.Name)
            .IsUnique();

            builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false);
            //builder
            //    .HasOne(f => f.Module)
            //    .WithMany(m => m.Forms)
            //    .HasForeignKey(f => f.ModuleId)
            //    .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("Forms", schema: "ModelSecurity");

        }
    }
}
