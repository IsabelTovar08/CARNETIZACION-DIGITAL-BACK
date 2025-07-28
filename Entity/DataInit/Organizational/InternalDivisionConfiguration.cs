using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Models.Organizational.Structure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.DataInit.Organizational
{
    public class InternalDivisionConfiguration : IEntityTypeConfiguration<InternalDivision>
    {
        public void Configure(EntityTypeBuilder<InternalDivision> builder)
        {
            builder.HasData(
                // División para Facultad de Ingeniería
                new InternalDivision
                {
                    Id = 1,
                    Name = "Escuela de Sistemas",
                    Description = "División académica enfocada en ingeniería de software y sistemas.",
                    OrganizationalUnitId = 1,
                    AreaCategoryId = 1,
                    IsDeleted = false
                },
                new InternalDivision
                {
                    Id = 2,
                    Name = "Escuela de Civil",
                    Description = "División académica centrada en ingeniería civil y estructuras.",
                    OrganizationalUnitId = 1,
                    AreaCategoryId = 1,
                    IsDeleted = false
                },

                // División para Facultad de Ciencias Económicas
                new InternalDivision
                {
                    Id = 3,
                    Name = "Departamento de Contaduría",
                    Description = "Encargado de contabilidad, auditoría y normativas contables.",
                    OrganizationalUnitId = 2,
                    AreaCategoryId = 4,
                    IsDeleted = false
                },
                new InternalDivision
                {
                    Id = 4,
                    Name = "Departamento de Economía",
                    Description = "Área enfocada en teoría económica, micro y macroeconomía.",
                    OrganizationalUnitId = 2,
                    AreaCategoryId = 4,
                    IsDeleted = false
                },

                // División para Facultad de Artes
                new InternalDivision
                {
                    Id = 5,
                    Name = "Escuela de Música",
                    Description = "Formación profesional en teoría musical, instrumentos y composición.",
                    OrganizationalUnitId = 3,
                    AreaCategoryId = 2,
                    IsDeleted = false
                }
            );


            builder.HasIndex(i => i.Name).IsUnique();

            builder.Property(i => i.IsDeleted)
                .HasDefaultValue(false);

            builder.ToTable("InternalDivisions", schema: "Organizational");
        }
    }
}
