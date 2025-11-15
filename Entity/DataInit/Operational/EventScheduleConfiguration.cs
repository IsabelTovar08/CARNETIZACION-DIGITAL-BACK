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
    public class EventScheduleConfiguration : IEntityTypeConfiguration<EventSchedule>
    {
        public void Configure(EntityTypeBuilder<EventSchedule> builder)
        {
            builder.ToTable("EventSchedules", schema: "Operational");

            builder.HasKey(es => new { es.EventId, es.ScheduleId });

            builder.HasOne(es => es.Event)
                   .WithMany(e => e.EventSchedules)
                   .HasForeignKey(es => es.EventId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(es => es.Schedule)
                   .WithMany(s => s.EventSchedules)
                   .HasForeignKey(es => es.ScheduleId)
                   .OnDelete(DeleteBehavior.Restrict);

            
            builder.HasData(
                 new EventSchedule { EventId = 1, ScheduleId = 1 },
                 new EventSchedule { EventId = 1, ScheduleId = 2 },
                 new EventSchedule { EventId = 2, ScheduleId = 3}
             );

        }
    }
}
