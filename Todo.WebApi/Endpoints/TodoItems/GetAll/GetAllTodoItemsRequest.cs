namespace Todo.WebApi.Endpoints.TodoItems.GetAll;

public sealed record GetAllTodoItemsRequest(bool IncludeCompleted = false);