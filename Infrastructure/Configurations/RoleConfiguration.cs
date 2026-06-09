using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles","identity");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .ValueGeneratedNever();

        // Name
        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(r => r.Name)
            .IsUnique();

        // Description
        builder.Property(r => r.Description)
            .IsRequired(false)
            .HasMaxLength(200);

        // IsSystem
        builder.Property(r => r.IsSystem)
            .IsRequired()
            .HasDefaultValue(false);

        // CreatedAt
        builder.Property(r => r.CreatedAt)
            .IsRequired();

        // Навигационные свойства
        builder.HasMany(r => r.UserRoles)
            .WithOne(ur => ur.Role)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(r => r.RolePermissions)
            .WithOne(rp => rp.Role)
            .HasForeignKey(rp => rp.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
