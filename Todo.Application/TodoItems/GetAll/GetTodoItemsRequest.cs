namespace Todo.Application.TodoItems.GetAll;

public sealed record GetTodoItemsRequest(bool IncludeCompleted = false);