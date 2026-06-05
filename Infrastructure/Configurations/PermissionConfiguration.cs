using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permissions", "public");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id).HasColumnName("id").IsRequired();
        builder.Property(p => p.Name).HasColumnName("name").IsRequired();

        // Description
        builder.Property(p => p.Description)
            .IsRequired(false)
            .HasMaxLength(500);

        // Code - уникальный код разрешения
        builder.Property(p => p.Code)
            .IsRequired()
            .HasMaxLength(100);
        
        // Group - группировка разрешений
        builder.Property(p => p.Group)
            .IsRequired()
            .HasMaxLength(100);

        // CreatedAt
        builder.Property(p => p.CreatedAt)
            .IsRequired();
        
        builder.HasMany(p => p.RolePermissions)
            .WithOne(pr => pr.Permission)
            .HasForeignKey(p => p.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}