﻿using Entity.Models.Operational;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.DataInit.Operational
{
    public class CardTemplateConfiguration : IEntityTypeConfiguration<CardTemplate>
    {
        public void Configure(EntityTypeBuilder<CardTemplate> builder)
        {
            builder.HasData(
                new CardTemplate
                {
                    Id = 1,
                    Name = "Plantilla Básica",
                    FrontBackgroundUrl = "https://drgxicjtijjdhrvsjgvd.supabase.co/storage/v1/object/public/Templates/ladoprincipal.svg",
                    BackBackgroundUrl = "https://drgxicjtijjdhrvsjgvd.supabase.co/storage/v1/object/public/Templates/ladotrasero.svg",
                    FrontElementsJson = @"
                    {
                        ""qr"": { ""x"": 332, ""y"": 48 },
                        ""underQrText"": { ""x"": 302, ""y"": 115 },
                        ""companyName"": { ""x"": 70, ""y"": 78 },
                        ""logo"": { ""x"": 7, ""y"": 97 },
                        ""userPhoto"": { ""x"": -16, ""y"": -1 },
                        ""name"": { ""x"": 240, ""y"": 209 },
                        ""profile"": { ""x"": 240, ""y"": 333 },
                        ""categoryArea"": { ""x"": 138, ""y"": 371 },
                        ""phoneNumber"": { ""x"": 46, ""y"": 502 },
                        ""bloodTypeValue"": { ""x"": 379, ""y"": 462 },
                        ""email"": { ""x"": 144, ""y"": 560 },
                        ""cardId"": { ""x"": 164, ""y"": 603 }
                    }",
                    BackElementsJson = @"
                    {
                        ""title"": { ""x"": 91, ""y"": 202 },
                        ""guides"": { ""x"": 36, ""y"": 371 },
                        ""address"": { ""x"": 43, ""y"": 568 },
                        ""phoneNumber"": { ""x"": 269, ""y"": 568 },
                        ""email"": { ""x"": 271, ""y"": 590 }
                    }"
                }
            );

            builder
                .HasIndex(f => f.Name)
                .IsUnique();

            builder.Property(x => x.IsDeleted)
                .HasDefaultValue(false);

            builder.ToTable("CardTemplates", schema: "Operational");
        }
    }
}
