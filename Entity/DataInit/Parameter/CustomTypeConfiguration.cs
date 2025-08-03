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
    public class CustomTypeConfiguration : IEntityTypeConfiguration<CustomType>
    {
        public void Configure(EntityTypeBuilder<CustomType> builder)
        {
            builder.HasData(
                // Tipo de documento
                new CustomType { Id = 1, Name = "CC", Description = "Cédula de ciudadanía", TypeCategoryId = 4, IsDeleted = false },
                new CustomType { Id = 2, Name = "CE", Description = "Cédula de extranjería", TypeCategoryId = 4, IsDeleted = false },
                new CustomType { Id = 3, Name = "TI", Description = "Tarjeta de identidad", TypeCategoryId = 4, IsDeleted = false },
                new CustomType { Id = 4, Name = "PA", Description = "Pasaporte", TypeCategoryId = 4, IsDeleted = false },
                new CustomType { Id = 5, Name = "NIT", Description = "Número de Identificación Tributaria", TypeCategoryId = 4, IsDeleted = false },

                // Tipo de sangre
                new CustomType { Id = 6, Name = "O+", Description = "Sangre tipo O positivo", TypeCategoryId = 5, IsDeleted = false },
                new CustomType { Id = 7, Name = "O-", Description = "Sangre tipo O negativo", TypeCategoryId = 5, IsDeleted = false },
                new CustomType { Id = 8, Name = "A+", Description = "Sangre tipo A positivo", TypeCategoryId = 5, IsDeleted = false },
                new CustomType { Id = 9, Name = "A-", Description = "Sangre tipo A negativo", TypeCategoryId = 5, IsDeleted = false },
                new CustomType { Id = 10, Name = "B+", Description = "Sangre tipo B positivo", TypeCategoryId = 5, IsDeleted = false },
                new CustomType { Id = 11, Name = "B-", Description = "Sangre tipo B negativo", TypeCategoryId = 5, IsDeleted = false },
                new CustomType { Id = 12, Name = "AB+", Description = "Sangre tipo AB positivo", TypeCategoryId = 5, IsDeleted = false },
                new CustomType { Id = 13, Name = "AB-", Description = "Sangre tipo AB negativo", TypeCategoryId = 5, IsDeleted = false },

                // Tipo de organización
                new CustomType { Id = 14, Name = "Empresa", Description = "Organización tipo empresa", TypeCategoryId = 1, IsDeleted = false },
                new CustomType { Id = 15, Name = "Colegio", Description = "Organización tipo colegio", TypeCategoryId = 1, IsDeleted = false },
                new CustomType { Id = 16, Name = "Universidad", Description = "Organización tipo universidad", TypeCategoryId = 1, IsDeleted = false },
                new CustomType { Id = 17, Name = "Sede Principal", Description = "Organización sede principal", TypeCategoryId = 1, IsDeleted = false },
                new CustomType { Id = 18, Name = "Sucursal", Description = "Organización tipo sucursal", TypeCategoryId = 1, IsDeleted = false },

              // Tipo de notificación
                new CustomType { Id = 19, Name = "Verificación", Description = "Notificación para verificación de identidad o datos", TypeCategoryId = 3, IsDeleted = false },
                new CustomType { Id = 20, Name = "Invitación", Description = "Notificación de invitación a evento o sistema", TypeCategoryId = 3, IsDeleted = false },
                new CustomType { Id = 21, Name = "Recordatorio", Description = "Notificación de recordatorio de evento o tarea", TypeCategoryId = 3, IsDeleted = false },
                new CustomType { Id = 22, Name = "Alerta", Description = "Notificación de alerta por evento crítico", TypeCategoryId = 3, IsDeleted = false },

                // Punto de acceso
                new CustomType { Id = 23, Name = "Entrada", Description = "Punto de acceso solo de entrada", TypeCategoryId = 2, IsDeleted = false },
                new CustomType { Id = 24, Name = "Salida", Description = "Punto de acceso solo de salida", TypeCategoryId = 2, IsDeleted = false },
                new CustomType { Id = 25, Name = "Entrada y salida", Description = "Punto de acceso bidireccional", TypeCategoryId = 2, IsDeleted = false },

                //Filtros para eventos privados 
                new CustomType { Id = 26, Name = "Division", Description = "Descripción", TypeCategoryId = 6, IsDeleted = false },
                new CustomType { Id = 27, Name = "Profile", Description = "Descripción", TypeCategoryId = 6, IsDeleted = false },
                new CustomType { Id = 28, Name = "Perfil", Description = "Descripción", TypeCategoryId = 6, IsDeleted = false }
            );

            builder.Property(ct => ct.Description).HasMaxLength(255);
            builder.HasIndex(ct => ct.Name).IsUnique();
            builder.Property(ct => ct.IsDeleted).HasDefaultValue(false);

            // Relación con TypeCategory
            builder.HasOne(ct => ct.TypeCategory)
                   .WithMany(tc => tc.Types)
                   .HasForeignKey(ct => ct.TypeCategoryId);

            // Relaciones inversas
            builder.HasMany(ct => ct.Notifications)
                   .WithOne(n => n.NotificationType)
                   .HasForeignKey(n => n.NotificationTypeId);

            builder.HasMany(ct => ct.PersonDocumentType)
                   .WithOne(p => p.DocumentType)
                   .HasForeignKey(p => p.DocumentTypeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(ct => ct.PersonBlodType)
                   .WithOne(p => p.BloodType)
                   .HasForeignKey(p => p.BloodTypeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(ct => ct.Organization)
                   .WithOne(o => o.OrganizaionType)
                   .HasForeignKey(o => o.TypeId);

            builder.HasMany(ct => ct.AccessPoints)
                   .WithOne(ap => ap.AccessPointType)
                   .HasForeignKey(ap => ap.TypeId);

            builder.ToTable("CustomTypes", schema: "Parameter");
        }
    }
}
