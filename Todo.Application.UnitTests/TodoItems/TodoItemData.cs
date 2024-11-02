using Todo.Domain.TodoItems;

namespace Todo.Application.UnitTests.TodoItems;

internal class TodoItemData
{
    public static TodoItem Create() => TodoItem.Create(Title, CreatedOnUtc);
    public static TodoItem CreateEmpty() => TodoItem.Create(new(""), CreatedOnUtc);

    public static readonly Guid Id = new();
    public static readonly TodoItemTitle Title = new("Test Title");
    public static readonly DateTime CreatedOnUtc = DateTime.UtcNow;
    public static readonly DateTime CompletedOnUtc = DateTime.UtcNow.AddDays(1);

}
