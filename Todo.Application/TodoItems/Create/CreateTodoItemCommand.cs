namespace Todo.Application.TodoItems.Create;

public sealed record CreateTodoItemCommand(string Title) : ICommand<Guid>;

