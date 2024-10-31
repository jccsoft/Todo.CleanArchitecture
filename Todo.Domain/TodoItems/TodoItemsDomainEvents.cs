namespace Todo.Domain.TodoItems;

public sealed record TodoItemCreatedDomainEvent(Guid TodoId) : IDomainEvent;
public sealed record TodoItemCompletedDomainEvent(Guid TodoId) : IDomainEvent;
public sealed record TodoItemNotCompletedDomainEvent(Guid TodoId) : IDomainEvent;