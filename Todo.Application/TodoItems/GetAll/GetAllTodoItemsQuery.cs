namespace Todo.Application.TodoItems.GetAll;

public sealed record GetAllTodoItemsQuery(bool IncludeCompleted = false) : ICachedQuery<List<TodoItemResponse>>
{
    public string CacheKey => $"todoitems-{IncludeCompleted}";

    public TimeSpan? Expiration => null;
}
