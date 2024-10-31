namespace Todo.Application.TodoItems.Delete;

public sealed record DeleteTodoItemCommand(Guid Id) : ICommand;
