namespace Todo.Domain.TodoItems;

public sealed record TodoItemTitle
{
    public TodoItemTitle(string? value)
    {
        Ensure.NotNullOrEmpty(value, nameof(value));

        Value = value;
    }

    public string Value { get; }
}
