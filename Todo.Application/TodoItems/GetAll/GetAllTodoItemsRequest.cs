namespace Todo.Application.TodoItems.GetAll;

public sealed record GetAllTodoItemsRequest(bool IncludeCompleted = false);