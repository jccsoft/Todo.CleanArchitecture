namespace Todo.Infrastructure.Outbox;

public sealed class OutboxMessage(Guid id, DateTime occurredOnUtc, string type, string content)
{
    private OutboxMessage() : this(Guid.NewGuid(), DateTime.UtcNow, "", "") { } // para EF Core

    public Guid Id { get; init; } = id;
    public DateTime OccurredOnUtc { get; init; } = occurredOnUtc;
    public string MessageType { get; init; } = type;
    public string Content { get; init; } = content;
    public DateTime? ProcessedOnUtc { get; init; } // para comprobar si el mensaje se ha procesado o no
    public string? MessageError { get; init; }
}
