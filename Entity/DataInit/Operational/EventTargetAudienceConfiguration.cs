using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Models.Organizational;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.DataInit.Organizational
{
    public class EventTargetAudienceConfiguration : IEntityTypeConfiguration<EventTargetAudience>
    {
        public void Configure(EntityTypeBuilder<EventTargetAudience> builder)
        {
            builder.HasData(
               new EventTargetAudience
               {
                   Id = 1,
                   TypeId = 6, // Ej: Division
                   ReferenceId = 1,
                   EventId = 1
               },
               new EventTargetAudience
               {
                   Id = 2,
                   TypeId = 6, // Ej: Profile
                   ReferenceId = 2,
                   EventId = 1
               },
               new EventTargetAudience
               {
                   Id = 3,
                   TypeId = 6, // Ej: Role
                   ReferenceId = 3,
                   EventId = 2
               }

              );

            // Relaciones
            builder
                .HasOne(e => e.CustomType)
                .WithMany()
                .HasForeignKey(e => e.TypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(e => e.Event)
                .WithMany(e => e.EventTargetAudiences)
                .HasForeignKey(e => e.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("EventTargetAudience", schema: "Organizational");

        }
    }
}
