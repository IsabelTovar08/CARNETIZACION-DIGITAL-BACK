using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Models.Parameter;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.DataInit.Parameter
{
    public class StatusConfiguration : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> builder)
        {
            builder.HasData(
                new Status { Id = 1, Name = "Activo", IsDeleted = false },
                new Status { Id = 2, Name = "Inactivo", IsDeleted = false },
                new Status { Id = 3, Name = "Pendiente", IsDeleted = false },
                new Status { Id = 4, Name = "Procesando", IsDeleted = false },
                new Status { Id = 5, Name = "Rechazado", IsDeleted = false },
                new Status { Id = 6, Name = "Entregado", IsDeleted = false },
                new Status { Id = 7, Name = "Leída", IsDeleted = false },
                new Status { Id = 8, Name = "En curso", IsDeleted = false },
                new Status { Id = 9, Name = "Finalizado", IsDeleted = false },
                new Status { Id = 10, Name = "Cancelado", IsDeleted = false },
                new Status { Id = 11, Name = "Expirado", IsDeleted = false },
                new Status { Id = 12, Name = "Renovado", IsDeleted = false }
            );


            builder.HasIndex(s => s.Name).IsUnique();
            builder.Property(s => s.IsDeleted).HasDefaultValue(false);

            // Relaciones uno a muchos
            builder.HasMany(s => s.NotificatiosReceived)
                   .WithOne(n => n.Status)
                   .HasForeignKey(n => n.StatusId);

            builder.HasMany(s => s.cards)
                   .WithOne(c => c.Status)
                   .HasForeignKey(c => c.StatusId);

            builder.HasMany(s => s.Events)
                   .WithOne(e => e.Status)
                   .HasForeignKey(e => e.StatusId);

            builder.ToTable("Statuses", schema: "Parameter");
        }
    }
}
