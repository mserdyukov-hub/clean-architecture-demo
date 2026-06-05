using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("userroles", "public");

        builder.HasKey(ur => new { ur.UserId, ur.RoleId });

        builder.Property(ur => ur.UserId).HasColumnName("userid").IsRequired();
        builder.Property(ur => ur.RoleId).HasColumnName("roleId").IsRequired();

        // AssignedAt
        builder.Property(ur => ur.AssignedAt)
            .IsRequired();

        // Навигационные свойства уже настроены в User и Role конфигурациях
        // builder.HasOne(ur => ur.User)
        //     .WithMany(u => u.UserRoles)
        //     .HasForeignKey(ur => ur.UserId);
        //
        // builder.HasOne(ur => ur.Role)
        //     .WithMany(r => r.UserRoles)
        //     .HasForeignKey(ur => ur.RoleId);
    }
}