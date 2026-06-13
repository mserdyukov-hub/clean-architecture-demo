using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class InboxMessageConfiguration : IEntityTypeConfiguration<InboxMessage>
{
    public void Configure(EntityTypeBuilder<InboxMessage> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Topic)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Partition)
            .IsRequired();

        builder.Property(x => x.Offset)
            .IsRequired();

        builder.Property(x => x.ReceivedOnUtc)
            .IsRequired();

        builder.Property(x => x.Error);

        builder.Property(x => x.ProcessedOnUtc);

        builder.HasIndex(x => new
            {
                x.Topic,
                x.Partition,
                x.Offset
            })
            .IsUnique();
    }

}
