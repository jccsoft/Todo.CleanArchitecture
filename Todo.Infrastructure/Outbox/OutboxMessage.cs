namespace Todo.Infrastructure.Outbox;

public sealed class OutboxMessage(Guid id, DateTime occurredOnUtc, string type, string content)
{
    public Guid Id { get; init; } = id;
    public DateTime OccurredOnUtc { get; init; } = occurredOnUtc;
    public string Type { get; init; } = type; // fully qualified name del domain event
    public string Content { get; init; } = content; // json string representando la instancia del domain event
    public DateTime? ProcessedOnUtc { get; init; } // para comprobar si el mensaje se ha procesado o no
    public string? Error { get; init; }
}
