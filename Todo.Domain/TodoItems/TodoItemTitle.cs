namespace Todo.Domain.TodoItems;

public sealed record TodoItemTitle
{
    public TodoItemTitle(string? title)
    {
        Ensure.NotNullOrEmpty(title, nameof(title));

        Value = title;
    }

    public string Value { get; }
}
