using Todo.Domain.TodoItems;

namespace Todo.Domain.UnitTests.TodoItems;
internal class TodoItemData
{
    public static readonly TodoItemTitle Title = new("Test Title");
    public static DateTime CreatedOnUtc = DateTime.UtcNow;
    public static DateTime CompletedOnUtc = DateTime.UtcNow.AddDays(1);

}
