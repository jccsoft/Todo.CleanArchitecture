namespace Todo.Application.TodoItems;

public sealed record TodoItemResponse(Guid Id, string Title, DateTime CreatedOnUtc, DateTime? CompletedOnUtc);
