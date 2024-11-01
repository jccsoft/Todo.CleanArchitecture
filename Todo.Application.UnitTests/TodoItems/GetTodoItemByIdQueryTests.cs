using FluentAssertions;
using NSubstitute;
using Todo.Application.TodoItems.GetById;
using Todo.Domain.TodoItems;

namespace Todo.Application.UnitTests.TodoItems;

public class GetTodoItemByIdQueryTests
{
    private readonly TodoItem _todoItem = TodoItemData.Create();
    private GetTodoItemByIdQuery _query;

    private readonly GetTodoItemByIdQueryHandler _handler;
    private readonly ITodoItemsRepository _todoItemsRepositoryMock;

    public GetTodoItemByIdQueryTests()
    {
        _todoItemsRepositoryMock = Substitute.For<ITodoItemsRepository>();

        _handler = new GetTodoItemByIdQueryHandler(_todoItemsRepositoryMock);
        _query = new(_todoItem.Id);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenTodoItemExists()
    {
        // Arrange
        _todoItemsRepositoryMock
            .GetByIdAsync(_todoItem.Id, Arg.Any<CancellationToken>())
            .Returns(_todoItem);

        // Act
        var result = await _handler.Handle(_query, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Equals(_todoItem);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenTodoItemNotFound()
    {
        // Arrange
        _todoItemsRepositoryMock
            .GetByIdAsync(_todoItem.Id, Arg.Any<CancellationToken>())
            .Returns((TodoItem?)null);

        // Act
        var result = await _handler.Handle(_query, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TodoItemsDomainErrors.NotFound);
    }
}
