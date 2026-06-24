using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class InboxMessageConfiguration : IEntityTypeConfiguration<InboxMessage>
{
    public void Configure(EntityTypeBuilder<InboxMessage> builder)
    {
        builder.ToTable("inbox_messages", "integration");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.EventId)
            .HasColumnName("event_id")
            .IsRequired();

        builder.Property(x => x.ConsumerName)
            .HasColumnName("consumer_name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.ReceivedOnUtc)
            .HasColumnName("received_on_utc")
            .IsRequired();

        builder.Property(x => x.ProcessedOnUtc)
            .HasColumnName("processed_on_utc");

        builder.Property(x => x.Status)
            .HasColumnName("status")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.Error)
            .HasColumnName("error");

        // Идемпотентность обработки
        builder.HasIndex(x => new
            {
                x.EventId,
                x.ConsumerName
            })
            .IsUnique();

        // Для Recovery Job
        builder.HasIndex(x => x.Status);

        // Для поиска зависших сообщений
        builder.HasIndex(x => x.ReceivedOnUtc);
    }

}
