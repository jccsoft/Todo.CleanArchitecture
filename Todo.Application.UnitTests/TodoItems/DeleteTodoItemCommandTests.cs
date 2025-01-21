using FluentAssertions;
using NSubstitute;
using Todo.Application.Abstractions.Data;
using Todo.Application.TodoItems.Delete;
using Todo.Domain.TodoItems;
using Todo.SharedKernel;

namespace Todo.Application.UnitTests.TodoItems;

public class DeleteTodoItemCommandTests
{
    private readonly TodoItem _todoItem = TodoItemData.Create();
    private readonly DeleteTodoItemCommand _command;

    private readonly DeleteTodoItemCommandHandler _handler;
    private readonly ITodoItemsRepository _todoItemsRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    public DeleteTodoItemCommandTests()
    {
        _todoItemsRepositoryMock = Substitute.For<ITodoItemsRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _handler = new DeleteTodoItemCommandHandler(_todoItemsRepositoryMock, _unitOfWorkMock);
        _command = new(_todoItem.Id);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenTodoItemIsCompleted()
    {
        // Arrange
        _todoItemsRepositoryMock
            .GetByIdAsync(_todoItem.Id, Arg.Any<CancellationToken>())
            .Returns(_todoItem);

        _todoItemsRepositoryMock
            .DeleteAsync(_todoItem.Id, Arg.Any<CancellationToken>())
            .Returns(1);

        _unitOfWorkMock
            .SaveChangesAsync(Arg.Any<List<Entity>>(), Arg.Any<CancellationToken>())
            .Returns(1);

        // Act
        var result = await _handler.Handle(_command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenTodoItemNotFound()
    {
        // Arrange
        _todoItemsRepositoryMock
            .GetByIdAsync(_todoItem.Id, Arg.Any<CancellationToken>())
            .Returns((TodoItem?)null);

        // Act
        var result = await _handler.Handle(_command, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TodoItemsErrors.NotFound);
    }


    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenRepositoryFails()
    {
        // Arrange
        _todoItemsRepositoryMock
            .GetByIdAsync(_todoItem.Id, Arg.Any<CancellationToken>())
            .Returns(_todoItem);

        _todoItemsRepositoryMock
            .DeleteAsync(_todoItem.Id, Arg.Any<CancellationToken>())
            .Returns(0);

        // Act
        var result = await _handler.Handle(_command, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TodoItemsErrors.NoRowsAffected);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenUnitOfWorkFails()
    {
        // Arrange
        _todoItemsRepositoryMock
            .GetByIdAsync(_todoItem.Id, Arg.Any<CancellationToken>())
            .Returns(_todoItem);

        _todoItemsRepositoryMock
            .DeleteAsync(_todoItem.Id, Arg.Any<CancellationToken>())
            .Returns(1);

        _unitOfWorkMock
            .SaveChangesAsync()
            .Returns(0);

        // Act
        var result = await _handler.Handle(_command, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TodoItemsErrors.NoRowsAffected);
    }
}
