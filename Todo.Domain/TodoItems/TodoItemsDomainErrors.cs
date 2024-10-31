namespace Todo.Domain.TodoItems;

public static class TodoItemsDomainErrors
{
    public static readonly Error NotFound = Error.Problem("TodoItem.NotFound", "Todo item not found");
    public static readonly Error AlreadyCompleted = Error.Problem("TodoItem.AlreadyCompleted", "Todo item is already completed");
    public static readonly Error NotCompleted = Error.Problem("TodoItem.NotCompleted", "Todo item is not completed");
    public static readonly Error NoRowsAffected = Error.Problem("TodoItem.NoRowsAffected", "No rows affected");
}
