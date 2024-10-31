namespace Todo.Application.TodoItems.Update;

public sealed record UpdateTodoItemCommand(Guid Id, string Title, bool IsCompleted) : ICommand;
