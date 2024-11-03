using Todo.Application.TodoItems.Delete;

namespace Todo.Application.IntegrationTests.TodoItems;

public class DeleteTodoItemTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Handle_Should_DeleteTodoItemFromDatabase_WhenCommandIsValid()
    {
        // Arrange
        DeleteTodoItemCommand command = new(TodoItemData.Sample2Id);

        // Act
        Result result = await Sender.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var todoItem = await TodoItemsRepository.GetByIdAsync(TodoItemData.Sample2Id);

        todoItem.Should().BeNull();
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenTodoItemNotFoundInDatabase()
    {
        // Arrange
        DeleteTodoItemCommand command = new(Guid.NewGuid());

        // Act
        Result result = await Sender.Send(command);

        // Assert
        result.IsFailure.Should().BeTrue();
    }
}
