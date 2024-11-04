namespace Todo.Domain.TodoItems;

public static class TodoItemsDomainErrors
{
    public static readonly Error NotFound = Error.NotFound("TodoItem.NotFound", "Todo item not found");
    public static readonly Error MissingTitle = Error.Validation("TodoItem.MissingTitle", "Title can't be empty");
    public static readonly Error AlreadyCompleted = Error.Validation("TodoItem.AlreadyCompleted", "Todo item is already completed");
    public static readonly Error NoRowsAffected = Error.Failure("TodoItem.NoRowsAffected", "No rows affected");
    public static readonly Error UnknownError = Error.Failure("TodoItem.UnknownError", "Unknown error");
}
