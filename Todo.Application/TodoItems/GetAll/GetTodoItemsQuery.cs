namespace Todo.Application.TodoItems.GetAll;

public sealed record GetTodoItemsQuery(bool IncludeCompleted = false) : ICachedQuery<List<TodoItemResponse>>
{
    public string CacheKey => $"todoitems-{IncludeCompleted}";

    public TimeSpan? Expiration => null;
}
