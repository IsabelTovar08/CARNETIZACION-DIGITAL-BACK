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
    public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Schedule> builder)
        {
            builder.HasData(
               new Schedule
               {
                   Id = 1,
                   Name = "Horario Jornada A",
                   StartTime = TimeOnly.Parse("07:00"),
                   EndTime = TimeOnly.Parse("18:00"),
                   OrganizationId = 1,
                   IsDeleted = false
               },
               new Schedule
               {
                   Id = 2,
                   Name = "Horario Jornada B",
                   StartTime = TimeOnly.Parse("08:00"),
                   EndTime = TimeOnly.Parse("17:00"),
                   OrganizationId = 1,
                   IsDeleted = false
               },
               new Schedule
               {
                   Id = 3,
                   Name = "Horario Jornada C",
                   StartTime = TimeOnly.Parse("06:30"),
                   EndTime = TimeOnly.Parse("19:00"),
                   OrganizationId = 1,
                   IsDeleted = false
               }
           );

            builder.Property(x => x.IsDeleted)
                   .HasDefaultValue(false);

            builder.ToTable("Schedules", schema: "Organizational");
        }
    }
}
