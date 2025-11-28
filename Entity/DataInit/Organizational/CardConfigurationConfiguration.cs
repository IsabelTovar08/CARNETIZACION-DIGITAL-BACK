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
    public class CardConfigurationConfiguration : IEntityTypeConfiguration<CardConfiguration>
    {
        public  void Configure(EntityTypeBuilder<CardConfiguration> builder)
        {
            builder.HasData(
                new CardConfiguration
                {
                    Id = 1,
                    Name = "Default Card",
                    IsDeleted = false,
                    ProfileId = 1,
                    CardTemplateId = 1,
                    ValidFrom = DateTime.SpecifyKind(new DateTime(2025, 7, 27, 10, 0, 0), DateTimeKind.Utc),
                    ValidTo = DateTime.SpecifyKind(new DateTime(2027, 7, 27, 10, 0, 0), DateTimeKind.Utc)
                }
            );


            // Tabla y esquema
            builder.ToTable("CardConfigurations", schema: "Organizational");
        }
    }
}
