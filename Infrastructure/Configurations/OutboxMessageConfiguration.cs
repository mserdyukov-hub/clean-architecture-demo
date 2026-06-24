using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("outbox_messages", "integration");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedNever().IsRequired();

        builder.Property(x => x.EventId).HasColumnName("event_id").IsRequired();
        builder.Property(x=>x.OccurredOnUtc).HasColumnName("occurred_on_utc").IsRequired();
        builder.Property(x => x.Topic).HasColumnName("topic").HasMaxLength(100).IsRequired();
        builder.Property(x=>x.EventType).HasColumnName("event_type").HasMaxLength(500).IsRequired();
        builder.Property(x=>x.Payload).HasColumnName("payload").HasMaxLength(500).IsRequired();
        builder.Property(x=>x.ProcessedOnUtc).HasColumnName("processed_on_utc");
        builder.Property(x=>x.Error).HasColumnName("error").HasMaxLength(500);
        builder.Property(x=>x.RetryCount).HasColumnName("retry_count").IsRequired();

        builder.HasIndex(x => x.ProcessedOnUtc).HasDatabaseName("ix_outbox_messages_processed_on_utc");
    }
}
