using Entity.Models.Operational;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DataInit.Operational
{
    public class EventAccessPointConfiguration : IEntityTypeConfiguration<EventAccessPoint>
    {
        public void Configure(EntityTypeBuilder<EventAccessPoint> builder)
        {
            builder.ToTable("EventAccessPoints", schema: "Operational");

            builder.HasKey(eap => new { eap.EventId, eap.AccessPointId });

            builder.HasOne(eap => eap.Event)
                   .WithMany(e => e.EventAccessPoints)
                   .HasForeignKey(eap => eap.EventId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(eap => eap.AccessPoint)
                   .WithMany(ap => ap.EventAccessPoints)
                   .HasForeignKey(eap => eap.AccessPointId)
                   .OnDelete(DeleteBehavior.Restrict);

            // 🔹 Aquí sembras los vínculos
            //builder.HasData(
            //    new EventAccessPoint { EventId = 1, AccessPointId = 1 },
            //    new EventAccessPoint { EventId = 2, AccessPointId = 2 },
            //    new EventAccessPoint { EventId = 3, AccessPointId = 3 }
            //);
        }
    }

}
