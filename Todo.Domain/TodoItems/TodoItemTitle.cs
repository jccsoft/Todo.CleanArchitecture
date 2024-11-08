namespace Todo.Domain.TodoItems;

public sealed record TodoItemTitle
{
    public TodoItemTitle(string? value)
    {
        Ensure.NotNullOrEmpty(value, nameof(TodoItemTitle));

        Value = value;
    }

    public string Value { get; }
}
