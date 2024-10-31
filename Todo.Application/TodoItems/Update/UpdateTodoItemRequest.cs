namespace Todo.Application.TodoItems.Update;

public sealed record UpdateTodoItemRequest(Guid Id, string Title, bool IsCompleted);
