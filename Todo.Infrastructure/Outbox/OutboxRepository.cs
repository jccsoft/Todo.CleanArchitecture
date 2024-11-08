using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Data;
using Todo.Application.Abstractions.Data;
using Todo.Infrastructure.Setup;

namespace Todo.Infrastructure.Outbox;

internal sealed class OutboxRepository(IDbConnectionFactory dbConnectionFactory,
                                       IOptions<OutboxOptions> outboxOptions,
                                       IDateTimeProvider dateTimeProvider,
                                       ILogger<OutboxRepository> logger) : IOutboxRepository
{
    private readonly IDbConnection _dbConnection = dbConnectionFactory.GetOpenConnection();
    private readonly OutboxOptions _outboxOptions = outboxOptions.Value;

    public async Task<int> AddRangeAsync(List<OutboxMessage> messages)
    {
        try
        {
            int affectedRows = 0;

            if (messages is null || messages.Count == 0) return 0;

            foreach (var message in messages)
            {
                string sql = """
                    INSERT INTO outbox_messages (Id, OccurredOnUtc, MessageType, Content)
                    VALUES (@Id, @OccurredOnUtc, @MessageType 
                """;

                sql += Config.IsDbMySQL ?
                    ", @Content)" :
                    ", CAST(@Content as jsonb))";

                affectedRows += await _dbConnection.ExecuteAsync(
                    sql,
                    new
                    {
                        message.Id,
                        message.OccurredOnUtc,
                        message.MessageType,
                        message.Content
                    },
                    commandType: CommandType.Text);
            }

            return affectedRows;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, nameof(AddRangeAsync));
            return 0;
        }
    }

    public async Task<IReadOnlyList<OutboxMessageResponse>> GetAllAsync()
    {
        try
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
        catch (Exception ex)
        {
            logger.LogError(ex, nameof(GetAllAsync));
            return [];
        }
    }

    public async Task UpdateAsync(IDbConnection newConnection, IDbTransaction newTransaction,
                                   OutboxMessageResponse outboxMessage, Exception? exception)
    {
        try
        {
            const string sql = """
                UPDATE outbox_messages
                SET ProcessedOnUtc = @ProcessedOnUtc, MessageError = @MessageError
                WHERE Id = @Id
            """;

            await newConnection.ExecuteAsync(
                sql,
                new
                {
                    outboxMessage.Id,
                    ProcessedOnUtc = dateTimeProvider.UtcNow,
                    MessageError = exception?.ToString()
                },
                transaction: newTransaction);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, nameof(UpdateAsync));
        }
    }
}
