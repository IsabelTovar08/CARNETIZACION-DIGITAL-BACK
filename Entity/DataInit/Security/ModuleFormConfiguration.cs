using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.DataInit.Security
{
    public class ModuleFormConfiguration : IEntityTypeConfiguration<ModuleForm>
    {
        public void Configure(EntityTypeBuilder<ModuleForm> builder)
        {
            builder.HasData(
                 new ModuleForm { Id = 1, FormId = 1, ModuleId = 1 },
                 new ModuleForm { Id = 2, FormId = 2, ModuleId = 2 },
                 new ModuleForm { Id = 3, FormId = 3, ModuleId = 3 },
                 new ModuleForm { Id = 4, FormId = 4, ModuleId = 3 }
             );

            builder.HasIndex(x => new { x.ModuleId, x.FormId }).IsUnique();

            builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false);


            builder.ToTable("ModuleForms", schema: "ModelSecurity");
            builder.HasOne(e => e.Module)
                   .WithMany(c => c.ModuleForm)
                   .HasForeignKey(e => e.ModuleId);
        }
    }
}
