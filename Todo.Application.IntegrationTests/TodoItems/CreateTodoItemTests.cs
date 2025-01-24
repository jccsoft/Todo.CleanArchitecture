using Todo.Application.TodoItems.Create;

namespace Todo.Application.IntegrationTests.TodoItems;

public class CreateTodoItemTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Handle_Should_AddTodoItemToDatabase_WhenCommandIsValid()
    {
        // Arrange
        CreateTodoItemCommand command = new(TodoItemData.Title.Value);

        // Act
        Result<Guid> result = await Sender.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var todoItem = await TodoItemsRepository.GetByIdAsync(result.Value);

        todoItem.Should().NotBeNull();
    }
}
