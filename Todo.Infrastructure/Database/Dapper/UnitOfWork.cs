using Newtonsoft.Json;
using System.Data;
using Todo.Application.Abstractions.Data;
using Todo.Infrastructure.Outbox;

namespace Todo.Infrastructure.Database.Dapper;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly IDbConnection _dbConnection;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IOutboxRepository _outboxRepository;
    private IDbTransaction _dbTransaction;

    public UnitOfWork(IDbConnectionFactory dbConnectionFactory, IDateTimeProvider dateTimeProvider, IOutboxRepository outboxRepository)
    {
        _dbConnection = dbConnectionFactory.GetOpenConnection();
        _dbConnection.Open();
        _dbTransaction = _dbConnection.BeginTransaction();
        _dateTimeProvider = dateTimeProvider;
        _outboxRepository = outboxRepository;
    }

    public async Task<int> SaveChangesAsync(List<Entity>? modifiedEntities = default, CancellationToken cancellationToken = default)
    {
        try
        {
            bool success = await AddDomainEventsAsOutboxMessages(modifiedEntities);

            if (success)
                _dbTransaction.Commit();
            else
                _dbTransaction.Rollback();

            _dbTransaction = _dbConnection.BeginTransaction();

            return success ? 1 : 0;
        }
        catch
        {
            _dbTransaction.Rollback();

            return 0;
        }
    }

    private async Task<bool> AddDomainEventsAsOutboxMessages(List<Entity>? entities)
    {
        if (entities is null) return true;

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
                JsonConvert.SerializeObject(domainEvent, JsonSettings.SerializerSettings)))
            .ToList();


        if (outboxMessages.Count == 0) return true;

        int affectedMessages = await _outboxRepository.AddRangeAsync(outboxMessages);

        return affectedMessages > 0;
    }

    public void Dispose()
    {
        _dbTransaction?.Rollback();
        _dbTransaction?.Dispose();
        _dbConnection.Close();
    }
}
