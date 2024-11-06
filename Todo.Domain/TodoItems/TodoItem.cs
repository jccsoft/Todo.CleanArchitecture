namespace Todo.Domain.TodoItems;

public sealed class TodoItem : Entity
{
    public TodoItemTitle? Title { get; set; }
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

    public Result Complete(DateTime utcNow)
    {
        if (CompletedOnUtc is not null) return Result.Failure(TodoItemsDomainErrors.AlreadyCompleted);

        CompletedOnUtc = utcNow;

        Raise(new TodoItemCompletedDomainEvent(Id));

        return Result.Success();
    }
}
