using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo.Infrastructure.Outbox;
using Todo.Infrastructure.Setup;

namespace Todo.Infrastructure.Database.EFCore.Configurations;

internal sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("outbox_messages");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Content)
            .HasColumnType(Config.DatabaseType == DatabaseTypes.MySql ? "json" : "jsonb");
    }
}
