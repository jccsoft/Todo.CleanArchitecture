namespace Todo.Application.TodoItems.Add;

public sealed record AddTodoItemCommand(string Title) : ICommand<Guid>;

