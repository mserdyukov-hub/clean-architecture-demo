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

        builder.Property(x => x.Topic)
            .HasColumnName("topic")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Partition)
            .HasColumnName("partition")
            .IsRequired();

        builder.Property(x => x.Offset)
            .HasColumnName("offset")
            .IsRequired();

        builder.Property(x => x.ReceivedOnUtc)
            .HasColumnName("received_on_utc")
            .IsRequired();

        builder.Property(x => x.Error).HasColumnName("error");

        builder.Property(x => x.ProcessedOnUtc).HasColumnName("processed_on_utc");

        builder.HasIndex(x => new
            {
                x.Topic,
                x.Partition,
                x.Offset
            })
            .IsUnique();
    }

}
