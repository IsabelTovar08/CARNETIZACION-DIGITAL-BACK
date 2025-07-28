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
    public class EventTypeConfiguration : IEntityTypeConfiguration<EventType>
    {
        public void Configure(EntityTypeBuilder<EventType> builder)
        {
            builder.HasData(
               new EventType
               {
                   Id = 1,
                   Name = "Bienvenida",
                   Description = "Eventos de bienvenida institucional",
                   IsDeleted = false
               },
               new EventType
               {
                   Id = 2,
                   Name = "Planificación",
                   Description = "Reuniones privadas para planificación interna",
                   IsDeleted = false,
               },
               new EventType
               {
                   Id = 3,
                   Name = "Capacitación",
                   Description = "Sesiones de formación para empleados o estudiantes",
                   IsDeleted = false
               },
               new EventType
               {
                   Id = 4,
                   Name = "Jornada de Estudio",
                   Description = "Espacios destinados a la concentración y repaso académico",
                   IsDeleted = false
               },
               new EventType
               {
                   Id = 5,
                   Name = "Jornada de Trabajo",
                   Description = "Actividades laborales organizadas por jornada",
                   IsDeleted = false
               },
               new EventType
               {
                   Id = 6,
                   Name = "Taller",
                   Description = "Eventos prácticos y participativos",
                   IsDeleted = false
               },
               new EventType
               {
                   Id = 7,
                   Name = "Encuentro",
                   Description = "Reuniones de carácter informal o comunitario",
                   IsDeleted = false
               }
           );

            builder.Property(et => et.Name)
                   .IsRequired();

            builder.HasMany(et => et.Events)
           .WithOne(e => e.EventType)
           .HasForeignKey(e => e.EventTypeId)
           .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("EventTypes", schema: "Operational");

        }
    }
}
