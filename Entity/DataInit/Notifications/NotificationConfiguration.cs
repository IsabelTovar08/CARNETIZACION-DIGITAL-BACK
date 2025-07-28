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
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasData(
               new Notification
               {
                   Id = 1,
                   Title = "Verificación de cuenta",
                   Message = "Por favor verifica tu cuenta haciendo clic en el enlace enviado.",
                   CreateDate = new DateTime(2025, 7, 27, 10, 0, 0),
                   NotificationTypeId = 1,
                   IsDeleted = false
               },
               new Notification
               {
                   Id = 2,
                   Title = "Invitación a evento",
                   Message = "Estás invitado al evento de bienvenida. Confirma tu asistencia.",
                   CreateDate = new DateTime(2025, 7, 28, 9, 30, 0),
                   NotificationTypeId = 2,
                   IsDeleted = false
               }
           );

            builder.Property(n => n.Title)
               .IsRequired()
               .HasMaxLength(200);

            builder.Property(n => n.Message)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(n => n.CreateDate)
                .HasColumnType("datetime");

            builder.HasOne(n => n.NotificationType)
                .WithMany()
                .HasForeignKey(n => n.NotificationTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(n => n.NotificationReceiveds)
                .WithOne(nr => nr.Notification)
                .HasForeignKey(nr => nr.NotificationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Notification", schema: "Notifications");
        }
    }
}
