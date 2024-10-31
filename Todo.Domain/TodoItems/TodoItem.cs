namespace Todo.Domain.TodoItems;

public sealed class TodoItem : Entity
{
    public TodoItemTitle? Title { get; set; }
    public bool IsCompleted { get; private set; } = false;
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? CompletedOnUtc { get; private set; }

    private TodoItem(Guid id, TodoItemTitle title, DateTime createdOnUtc) : base(id)
    {
        Id = id;
        Title = title;
        CreatedOnUtc = createdOnUtc;
    }
    private TodoItem()
    {
    }

    public static TodoItem Create(TodoItemTitle title, DateTime utcNow)
    {
        var todo = new TodoItem(Guid.NewGuid(), title, utcNow);

        todo.Raise(new TodoItemCreatedDomainEvent(todo.Id));

        return todo;
    }

    public Result MarkAsCompleted(DateTime utcNow)
    {
        if (IsCompleted) return Result.Failure(TodoItemsDomainErrors.AlreadyCompleted);

        CompletedOnUtc = utcNow;
        IsCompleted = true;

        Raise(new TodoItemCompletedDomainEvent(Id));

        return Result.Success();
    }

    public Result MarkAsNotCompleted()
    {
        if (IsCompleted == false) return Result.Failure(TodoItemsDomainErrors.NotCompleted);

        CompletedOnUtc = null;
        IsCompleted = false;

        Raise(new TodoItemNotCompletedDomainEvent(Id));

        return Result.Success();
    }
}
