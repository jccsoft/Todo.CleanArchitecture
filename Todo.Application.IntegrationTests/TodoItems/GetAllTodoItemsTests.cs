using Todo.Application.TodoItems;
using Todo.Application.TodoItems.GetAll;

namespace Todo.Application.IntegrationTests.TodoItems;

public class GetAllTodoItemsTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Handle_Should_GetTodoItemsFromDatabase_WhenCommandIsValid()
    {
        //Arrange
        GetAllTodoItemsQuery query = new(true);

        // Act
        Result<List<TodoItemResponse>> result = await Sender.Send(query);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Count.Should().NotBe(0);
    }
}
