using Todo.Application.TodoItems.Complete;

namespace Todo.Application.IntegrationTests.TodoItems;

public class CompleteTodoItemTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Handle_Should_CompleteTodoItemInDatabase_WhenCommandIsValid()
    {
        // Arrange
        CompleteTodoItemCommand command = new(TodoItemData.Sample1Id);

        // Act
        Result result = await Sender.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var todoItem = await TodoItemsRepository.GetByIdAsync(TodoItemData.Sample1Id);

        todoItem.Should().NotBeNull();
        todoItem!.IsCompleted.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenTodoItemNotFoundInDatabase()
    {
        // Arrange
        CompleteTodoItemCommand command = new(Guid.NewGuid());

        // Act
        Result result = await Sender.Send(command);

        // Assert
        result.IsFailure.Should().BeTrue();
    }
}
