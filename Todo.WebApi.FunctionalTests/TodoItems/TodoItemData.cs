namespace Todo.WebApi.FunctionalTests.TodoItems;

internal static class TodoItemData
{
    public static readonly string BaseUrl = "api/v1/todoitems";
    public static readonly string BaseUrlComplete = $"{BaseUrl}/complete";

    public static readonly Guid Sample1Id = new("7db5edbf-ddd5-416d-9724-16600672733d"); // for complete test
    public static readonly Guid Sample2Id = new("2631a5b0-4779-4a8a-9caf-25792fe37c17"); // for complete test
    public static readonly Guid Sample3Id = new("5dcafa62-3569-41c1-bfe5-ac2e7ec1b2b0"); // for delete test
    public static readonly Guid Sample4Id = new("27c5536d-f91e-45f6-8a72-1493322efe9b"); // for get by id
}
