using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("rolepermissions", "identity");

        builder.HasKey(rp => new { rp.RoleId, rp.PermissionId });

        builder.Property(rp => rp.RoleId).HasColumnName("roleid").IsRequired();
        builder.Property(rp => rp.PermissionId).HasColumnName("permissionid").IsRequired();

        // AssignedAt
        builder.Property(rp => rp.AssignedAt)
            .IsRequired();

        // Навигационные свойства уже настроены в Role и Permission конфигурациях
        // builder.HasOne(rp => rp.Role)
        //     .WithMany(r => r.RolePermissions)
        //     .HasForeignKey(rp => rp.RoleId);
        //
        // builder.HasOne(rp => rp.Permission)
        //     .WithMany(rp => rp.RolePermissions)
        //     .HasForeignKey(rp => rp.PermissionId);
    }
}
