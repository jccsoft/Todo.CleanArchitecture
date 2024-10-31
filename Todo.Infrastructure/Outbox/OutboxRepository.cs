using Dapper;
using Microsoft.Extensions.Options;
using System.Data;
using Todo.Application.Abstractions.Data;

namespace Todo.Infrastructure.Outbox;

internal sealed class OutboxRepository(IDbConnectionFactory dbConnectionFactory,
                                       IOptions<OutboxOptions> outboxOptions,
                                       IDateTimeProvider dateTimeProvider) : IOutboxRepository
{
    private readonly IDbConnection _dbConnection = dbConnectionFactory.GetOpenConnection();
    private readonly OutboxOptions _outboxOptions = outboxOptions.Value;

    public async Task<int> AddRangeAsync(List<OutboxMessage>? messages)
    {
        int affectedRows = 0;

        if (messages is null || messages.Count == 0) return 0;

        foreach (var message in messages)
        {
            const string sql = """
                INSERT INTO outbox_messages (Id, OccurredOnUtc, Type, Content)
                VALUES (@Id, @OccurredOnUtc, @Type, @Content)
                """;

            affectedRows += await _dbConnection.ExecuteAsync(
                sql,
                new
                {
                    message.Id,
                    message.OccurredOnUtc,
                    message.Type,
                    message.Content
                },
                commandType: CommandType.Text);
        }

        return affectedRows;
    }

    public async Task<IReadOnlyList<OutboxMessageResponse>> GetAllAsync()
    {
        var sql = $"""                
            SELECT Id, Content
            FROM outbox_messages
            WHERE ProcessedOnUtc IS NULL
            ORDER BY OccurredOnUtc
            LIMIT {_outboxOptions.BatchSize}
            FOR UPDATE
            """;

        var outboxMessages = await _dbConnection.QueryAsync<OutboxMessageResponse>(sql);

        return outboxMessages.ToList();
    }

    public async Task UpdateAsync(IDbConnection newConnection, IDbTransaction newTransaction,
                                   OutboxMessageResponse outboxMessage, Exception? exception)
    {
        const string sql = @"
            UPDATE outbox_messages
            SET ProcessedOnUtc = @ProcessedOnUtc,
                Error = @Error
            WHERE Id = @Id";

        await newConnection.ExecuteAsync(
            sql,
            new
            {
                outboxMessage.Id,
                ProcessedOnUtc = dateTimeProvider.UtcNow,
                Error = exception?.ToString()
            },
            transaction: newTransaction);
    }
}
