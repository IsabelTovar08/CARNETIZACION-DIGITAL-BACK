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
                    StartTime = new TimeSpan(7, 0, 0),
                    EndTime = new TimeSpan(18, 0, 0),
                    IsDeleted = false
                },
                new Schedule
                {
                    Id = 2,
                    Name = "Horario Jornada B",
                    StartTime = new TimeSpan(8, 0, 0),
                    EndTime = new TimeSpan(17, 0, 0),
                    IsDeleted = false
                },
                new Schedule
                {
                    Id = 3,
                    Name = "Horario Jornada C",
                    StartTime = new TimeSpan(6, 30, 0),
                    EndTime = new TimeSpan(19, 0, 0),
                    IsDeleted = false
                }
            );

            builder.Property(x => x.IsDeleted)
                   .HasDefaultValue(false);

            /// <summary>
            /// Configura los tipos de datos para compatibilidad entre proveedores.
            /// SQL Server no soporta directamente TimeOnly, por lo tanto se almacena como TimeSpan.
            /// PostgreSQL y MySQL soportan 'time' de forma nativa.
            /// </summary>
 



            builder.ToTable("Schedules", schema: "Organizational");
        }
    }
}
