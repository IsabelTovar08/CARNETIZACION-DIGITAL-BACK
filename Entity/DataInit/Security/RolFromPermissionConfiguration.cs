using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.DataInit.Security

{
    public class SeedSecurityDataConfiguration : IEntityTypeConfiguration<RolFormPermission>
    {
       
        public void Configure(EntityTypeBuilder<RolFormPermission> builder)
        {
            builder.HasData(
                new RolFormPermission { Id = 1, RolId = 3, FormId = 1, PermissionId = 1 },
                new RolFormPermission { Id = 2, RolId = 3, FormId = 2, PermissionId = 3 },
                new RolFormPermission { Id = 3, RolId = 3, FormId = 3, PermissionId = 2 }
            );

            builder.HasIndex(x => new { x.RolId, x.FormId, x.PermissionId }).IsUnique();

            builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false);

            builder.ToTable("RolFormPermissions", schema: "ModelSecurity");

            builder.HasOne(e => e.Rol)
                   .WithMany(c => c.RolFormPermissions)
                   .HasForeignKey(e => e.RolId);

            builder.HasOne(e => e.Form)
                   .WithMany(c => c.RolFormPermissions)
                   .HasForeignKey(e => e.FormId);

            builder.HasOne(e => e.Permission)
                   .WithMany(c => c.RolFormPermissions)
                   .HasForeignKey(e => e.PermissionId);
        }
    }
}
