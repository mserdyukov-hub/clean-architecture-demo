using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users", "public");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id).HasColumnName("id").ValueGeneratedNever().IsRequired();
        builder.Property(u => u.UserName).HasColumnName("username").HasMaxLength(255);
        builder.Property(u => u.FirstName).HasColumnName("first_name").HasMaxLength(50);
        builder.Property(u => u.LastName).HasColumnName("last_name").HasMaxLength(50);
        builder.Property(u => u.Email).HasColumnName("email").HasMaxLength(50)
            .HasConversion(email=>email.Value, value => new Email(value)).IsRequired();
        builder.Property(u => u.PasswordHash).HasColumnName("password_hash").HasMaxLength(255)
            .HasConversion(hash => hash.Value, value => new PasswordHash(value)).IsRequired();
        builder.Property(u => u.CreatedAt).HasColumnName("created_at").HasColumnType("datetime").IsRequired();
        builder.Property(u => u.IsActive).HasColumnName("isactive").IsRequired();
        builder.Property(u => u.LastLoginAt).HasColumnName("last_login_at");
        builder.Property(u => u.FailedLoginAttempts).HasColumnName("failed_login_attempts");
        builder.Property(u => u.LockoutEnd).HasColumnName("lockout_end");
        
        builder.HasMany(u => u.UserRoles)
            .WithOne(ur => ur.User)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}