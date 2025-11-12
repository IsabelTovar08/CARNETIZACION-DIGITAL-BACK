using Entity.Models.Operational;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.DataInit.Operational
{
    public class EventAccessPointConfiguration : IEntityTypeConfiguration<EventAccessPoint>
    {
        public void Configure(EntityTypeBuilder<EventAccessPoint> builder)
        {
            // Nombre de tabla y esquema
            builder.ToTable("EventAccessPoints", schema: "Operational");

            // Usa Id como clave principal (ya lo tienes en BaseModel)
            builder.HasKey(eap => eap.Id);

            //  Relación con Event
            builder.HasOne(eap => eap.Event)
                   .WithMany(e => e.EventAccessPoints)
                   .HasForeignKey(eap => eap.EventId)
                   .OnDelete(DeleteBehavior.Cascade);

            //Relación con AccessPoint
            builder.HasOne(eap => eap.AccessPoint)
                   .WithMany(ap => ap.EventAccessPoints)
                   .HasForeignKey(eap => eap.AccessPointId)
                   .OnDelete(DeleteBehavior.Restrict);

         
            builder.HasData(
                new EventAccessPoint
                {
                    Id = 1,
                    EventId = 1,       // Evento "Conferencia de Tecnología"
                    AccessPointId = 1  // Primer punto de acceso
                },
                new EventAccessPoint
                {
                    Id = 2,
                    EventId = 1,       // Mismo evento
                    AccessPointId = 2  // Segundo punto de acceso
                }
            );
        }
    }
}
