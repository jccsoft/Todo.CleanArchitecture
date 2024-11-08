using Newtonsoft.Json;
using Todo.Application.Abstractions.Data;
using Todo.Infrastructure.Outbox;

namespace Todo.Infrastructure.Database.EFCore;

internal class AppDbContext(DbContextOptions<AppDbContext> options,
                            IOutboxRepository outboxRepository,
                            IDateTimeProvider dateTimeProvider) : DbContext(options), IUnitOfWork
{
    public DbSet<TodoItem> TodoItems { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        modelBuilder.HasDefaultSchema(Schemas.Default);
    }

    public async Task<int> SaveChangesAsync(List<Entity>? entities = null, CancellationToken cancellationToken = default)
    {
        bool success = await AddDomainEventsAsOutboxMessages();
        if (success == false) return 0;

        return await base.SaveChangesAsync(cancellationToken);
    }

    private async Task<bool> AddDomainEventsAsOutboxMessages()
    {
        var outboxMessages = ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                var domainEvents = entity.DomainEvents;

                entity.ClearDomainEvents();

                return domainEvents;
            })
            .Select(domainEvent => new OutboxMessage(
                Guid.NewGuid(),
                dateTimeProvider.UtcNow,
                domainEvent.GetType().Name,
                JsonConvert.SerializeObject(domainEvent, JsonSettings.SerializerSettings)))
            .ToList();


        if (outboxMessages is null || outboxMessages.Count == 0) return true;

        int affectedMessages = await outboxRepository.AddRangeAsync(outboxMessages);

        return affectedMessages > 0;
    }
}
