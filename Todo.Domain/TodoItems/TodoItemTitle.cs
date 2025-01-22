namespace Todo.Domain.TodoItems;

public sealed record TodoItemTitle
{
    public TodoItemTitle(string? title)
    {
        Ensure.NotNullOrEmpty(title);

        Value = title;
    }

    public string Value { get; }
}
