using Newtonsoft.Json;
using System.Data;
using Todo.Application.Abstractions.Data;
using Todo.Infrastructure.Outbox;

namespace Todo.Infrastructure.Database;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly IDbConnection _dbConnection;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IOutboxRepository _outboxRepository;
    private IDbTransaction _dbTransaction;

    private static readonly JsonSerializerSettings _jsonSerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All
    };

    public UnitOfWork(IDbConnectionFactory dbConnectionFactory, IDateTimeProvider dateTimeProvider, IOutboxRepository outboxRepository)
    {
        _dbConnection = dbConnectionFactory.GetOpenConnection();
        _dbConnection.Open();
        _dbTransaction = _dbConnection.BeginTransaction();
        _dateTimeProvider = dateTimeProvider;
        _outboxRepository = outboxRepository;
    }

    public async Task<int> SaveChangesAsync(List<Entity>? entities = default, CancellationToken cancellationToken = default)
    {
        try
        {
            await AddDomainEventsAsOutboxMessages(entities);

            _dbTransaction.Commit();
            _dbTransaction = _dbConnection.BeginTransaction();

            return 1;// Task.FromResult(1);
        }
        catch
        {
            _dbTransaction.Rollback();

            return 0; // Task.FromResult(0);
        }
    }

    private async Task<int> AddDomainEventsAsOutboxMessages(List<Entity>? entities)
    {
        if (entities is null) return 0;

        var outboxMessages =
            entities
            .SelectMany(entity =>
            {
                var domainEvents = entity.DomainEvents;

                entity.ClearDomainEvents();

                return domainEvents;
            })
            .Select(domainEvent => new OutboxMessage(
                Guid.NewGuid(),
                _dateTimeProvider.UtcNow,
                domainEvent.GetType().Name,
                JsonConvert.SerializeObject(domainEvent, _jsonSerializerSettings)))
            .ToList();

        return await _outboxRepository.AddRangeAsync(outboxMessages);
    }

    public void Dispose()
    {
        _dbTransaction?.Rollback();
        _dbTransaction?.Dispose();
        _dbConnection.Close();
    }
}
