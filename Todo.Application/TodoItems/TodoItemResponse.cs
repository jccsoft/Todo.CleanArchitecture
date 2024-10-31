namespace Todo.Application.TodoItems;

public sealed record TodoItemResponse(Guid Id, string Title, bool IsCompleted, DateTime CreatedOnUtc, DateTime? CompletedOnUtc);
