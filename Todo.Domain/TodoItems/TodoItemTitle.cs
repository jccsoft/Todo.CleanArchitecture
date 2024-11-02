namespace Todo.Domain.TodoItems;

public sealed record TodoItemTitle
{
    public TodoItemTitle(string? title)
    {
        Ensure.NotNullOrEmpty(title, nameof(TodoItemTitle));

        Value = title;
    }

    public string Value { get; }
}
