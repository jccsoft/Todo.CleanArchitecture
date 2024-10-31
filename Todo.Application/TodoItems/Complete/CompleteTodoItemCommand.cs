namespace Todo.Application.TodoItems.Complete;

public sealed record CompleteTodoItemCommand(Guid Id) : ICommand;
