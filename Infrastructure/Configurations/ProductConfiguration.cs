using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products","shop");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Description)
            .HasColumnName("description")
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(x => x.StockQuantity)
            .HasColumnName("stock_quantity")
            .IsRequired();

        builder.Property(x => x.IsActive)
            .HasColumnName("is_active")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(x => x.CategoryId)
            .HasColumnName("category_id")
            .IsRequired();

        builder.OwnsOne(x => x.Price, price =>
        {
            price.Property(p => p.Amount)
                .HasColumnName("Price")
                .HasPrecision(18, 2);

            price.Property(p => p.Currency)
                .HasColumnName("Currency")
                .HasMaxLength(3);
        });

        // Создать Foreign Key в БД.
        // Не позволить сохранить Product с несуществующей Category.
        // Не позволить удалить Category, если на неё ссылаются Products.
        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
