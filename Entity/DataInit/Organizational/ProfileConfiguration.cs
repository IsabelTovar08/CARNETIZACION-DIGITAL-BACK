using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Models.Organizational.Assignment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.DataInit.Organizational
{
    public class ProfileConfiguration : IEntityTypeConfiguration<Profiles>
    {
        public void Configure(EntityTypeBuilder<Profiles> builder)
        {
            builder.HasData(
                new Profiles { Id = 1, Name = "Estudiante", Description = "Perfil para estudiantes de la institución", IsDeleted = false },
                new Profiles { Id = 2, Name = "Profesor", Description = "Perfil para docentes o instructores", IsDeleted = false },
                new Profiles { Id = 3, Name = "Administrativo", Description = "Perfil para personal administrativo", IsDeleted = false },
                new Profiles { Id = 4, Name = "Pasante", Description = "Perfil para pasantes o practicantes", IsDeleted = false },
                new Profiles { Id = 5, Name = "Invitado", Description = "Perfil para usuarios externos o visitantes", IsDeleted = false }
            );

            builder.HasIndex(p => p.Name).IsUnique();

            builder.Property(x => x.IsDeleted)
                   .HasDefaultValue(false);

            builder.ToTable("Profiles", schema: "Organizational");
        }
    }
}
