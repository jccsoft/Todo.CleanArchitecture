using FluentAssertions;
using NSubstitute;
using Todo.Application.TodoItems.GetAll;
using Todo.Domain.TodoItems;

namespace Todo.Application.UnitTests.TodoItems;

public class GetAllTodoItemsQueryTests
{
    private readonly List<TodoItem> _todoItems;
    private readonly GetAllTodoItemsQuery _query = new();

    private readonly GetAllTodoItemsQueryHandler _handler;
    private readonly ITodoItemsRepository _todoItemsRepositoryMock;

    public GetAllTodoItemsQueryTests()
    {
        _todoItemsRepositoryMock = Substitute.For<ITodoItemsRepository>();

        _todoItems = [TodoItemData.Create(), TodoItemData.Create(), TodoItemData.Create()];
        _todoItems[0].Complete(DateTime.Now);

        _handler = new GetAllTodoItemsQueryHandler(_todoItemsRepositoryMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_Allways()
    {
        // Arrange
        _todoItemsRepositoryMock
            .GetAllAsync(Arg.Any<bool?>(), Arg.Any<CancellationToken>())
            .Returns(_todoItems);

        // Act
        var result = await _handler.Handle(_query, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Count.Should().Be(_todoItems.Count);
        for (int i = 0; i < _todoItems.Count; i++)
        {
            result.Value[i].Id.Should().Be(_todoItems[i].Id);
            result.Value[i].Title.Should().Be(_todoItems[i].Title!.Value);
            result.Value[i].CreatedOnUtc.Should().Be(_todoItems[i].CreatedOnUtc);
            result.Value[i].CompletedOnUtc.Should().Be(_todoItems[i].CompletedOnUtc);
        }
    }
}
