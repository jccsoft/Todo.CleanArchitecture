namespace Todo.Application.IntegrationTests.TodoItems;
internal class TodoItemData
{
    public static readonly Guid Sample1Id = new("7db5edbf-ddd5-416d-9724-16600672733d"); // Sample 1 - init-db.sql
    public static readonly Guid Sample2Id = new("2631a5b0-4779-4a8a-9caf-25792fe37c17"); // Sample 2
    public static readonly Guid Sample3Id = new("5dcafa62-3569-41c1-bfe5-ac2e7ec1b2b0"); // Sample 3
    public static readonly Guid Sample4Id = new("27c5536d-f91e-45f6-8a72-1493322efe9b"); // Sample 4

    public static readonly TodoItemTitle Title = new("Test Title");
}
