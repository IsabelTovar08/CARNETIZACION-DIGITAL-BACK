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
    public class AreaConfiguration : IEntityTypeConfiguration<AreaCategory>
    {
        public void Configure(EntityTypeBuilder<AreaCategory> builder)
        {
            builder.HasData(
               new AreaCategory
               {
                   Id = 1,
                   Name = "Tecnología",
                   Description = "Área relacionada con sistemas, informática y desarrollo tecnológico",
                   IsDeleted = false
               },
               new AreaCategory
               {
                   Id = 2,
                   Name = "Humanidades",
                   Description = "Área enfocada en estudios sociales, filosofía, literatura y cultura",
                   IsDeleted = false

               },
               new AreaCategory
               {
                   Id = 3,
                   Name = "Ciencias",
                   Description = "Área de física, química, biología y otras ciencias naturales",
                   IsDeleted = false

               },
               new AreaCategory
               {
                   Id = 4,
                   Name = "Educación",
                   Description = "Área dedicada a la enseñanza y formación académica",
                   IsDeleted = false

               },
               new AreaCategory
               {
                   Id = 5,
                   Name = "Administración",
                   Description = "Área de gestión institucional y procesos administrativos",
                   IsDeleted = false

               }
           );
            builder
               .HasMany(ac => ac.InternalDivisions)
               .WithOne(id => id.AreaCategory)
               .HasForeignKey(id => id.AreaCategoryId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("Areas", schema: "Organizational");

        }
    }
}
