namespace Todo.Application.IntegrationTests.TodoItems;

public class AddTodoItemTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Handle_Should_AddTodoItemToDatabase_WhenCommandIsValid()
    {
        // Arrange
        AddTodoItemCommand command = new(TodoItemData.Title.Value);

        // Act
        Result<Guid> result = await Sender.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var todoItem = await TodoItemsRepository.GetByIdAsync(result.Value);

        todoItem.Should().NotBeNull();
    }
}
