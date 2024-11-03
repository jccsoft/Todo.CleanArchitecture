using Todo.Application.TodoItems;
using Todo.Application.TodoItems.GetById;

namespace Todo.Application.IntegrationTests.TodoItems;

public class GetByIdTodoItemTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Handle_Should_GetTodoItemFromDatabase_WhenCommandIsValid()
    {
        // Arrange 
        GetTodoItemByIdQuery query = new(TodoItemData.Sample1Id);

        // Act
        Result<TodoItemResponse> result = await Sender.Send(query);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenTodoItemNotFoundInDatabase()
    {
        // Arrange 
        GetTodoItemByIdQuery query = new(Guid.NewGuid());

        // Act
        Result<TodoItemResponse> result = await Sender.Send(query);

        // Assert
        result.IsFailure.Should().BeTrue();
    }
}
