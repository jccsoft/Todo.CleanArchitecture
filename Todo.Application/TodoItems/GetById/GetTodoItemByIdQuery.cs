namespace Todo.Application.TodoItems.GetById;

public sealed record GetTodoItemByIdQuery(Guid Id) : ICachedQuery<TodoItemResponse>
{
    public string CacheKey => $"todoitem-by-id-{Id}";

    public TimeSpan? Expiration => null;
}
