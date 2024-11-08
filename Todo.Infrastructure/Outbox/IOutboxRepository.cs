using System.Data;

namespace Todo.Infrastructure.Outbox;

internal interface IOutboxRepository
{
    Task<int> AddRangeAsync(List<OutboxMessage> messages);
    Task<IReadOnlyList<OutboxMessageResponse>> GetAllAsync();
    Task UpdateAsync(IDbConnection newConnection, IDbTransaction newTransaction,
                                   OutboxMessageResponse outboxMessage, Exception? exception);
}
