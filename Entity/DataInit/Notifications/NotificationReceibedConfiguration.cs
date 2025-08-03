using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Models.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.DataInit.Notifications
{
    public class NotificationReceibedConfiguration : IEntityTypeConfiguration<NotificationReceived>
    {
        public void Configure(EntityTypeBuilder<NotificationReceived> builder)
        {
            builder.HasData(
               new NotificationReceived
               {
                   Id = 1,
                   NotificationId = 1, // Verificación de cuenta
                   UserId = 1,
                   StatusId = 1, // Enviado
                   SendDate = new DateTime(2025, 7, 27, 10, 5, 0),
                   ReadDate = null,
                   ExpirationDate = new DateTime(2025, 8, 1),
                   IsDeleted = false
               },
               new NotificationReceived
               {
                   Id = 2,
                   NotificationId = 2, // Invitación a evento
                   UserId = 2,
                   StatusId = 2, // Leído
                   SendDate = new DateTime(2025, 7, 28, 9, 35, 0),
                   ReadDate = new DateTime(2025, 7, 28, 10, 15, 0),
                   ExpirationDate = new DateTime(2025, 8, 5),
                   IsDeleted = false
               }
           );

            // Relaciones
            builder.HasOne(nr => nr.Notification)
                .WithMany(n => n.NotificationReceiveds)
                .HasForeignKey(nr => nr.NotificationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(nr => nr.User)
                .WithMany()
                .HasForeignKey(nr => nr.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(nr => nr.Status)
                .WithMany()
                .HasForeignKey(nr => nr.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("NotificationReceived", schema: "Notifications");
        }
    }
}
