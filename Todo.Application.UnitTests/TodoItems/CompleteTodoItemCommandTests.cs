using FluentAssertions;
using NSubstitute;
using Todo.Application.Abstractions.Data;
using Todo.Application.TodoItems.Complete;
using Todo.Domain.TodoItems;
using Todo.SharedKernel;

namespace Todo.Application.UnitTests.TodoItems;

public class CompleteTodoItemCommandTests
{
    private readonly CompleteTodoItemCommandHandler _handler;
    private readonly ITodoItemsRepository _todoItemsRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    public CompleteTodoItemCommandTests()
    {
        _todoItemsRepositoryMock = Substitute.For<ITodoItemsRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        IDateTimeProvider dateTimeProviderMock = Substitute.For<IDateTimeProvider>();
        dateTimeProviderMock.UtcNow.Returns(TodoItemData.CreatedOnUtc);

        _handler = new CompleteTodoItemCommandHandler(_todoItemsRepositoryMock, _unitOfWorkMock, dateTimeProviderMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenTodoItemIsCompleted()
    {
        // Arrange
        var todoItem = TodoItemData.Create();
        var command = new CompleteTodoItemCommand(todoItem.Id);

        _todoItemsRepositoryMock
            .GetByIdAsync(todoItem.Id, Arg.Any<CancellationToken>())
            .Returns(todoItem);

        _todoItemsRepositoryMock
            .UpdateAsync(Arg.Any<TodoItem>(), Arg.Any<CancellationToken>())
            .Returns(1);

        _unitOfWorkMock
            .SaveChangesAsync(Arg.Any<List<Entity>>(), Arg.Any<CancellationToken>())
            .Returns(1);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenTodoItemNotFound()
    {
        // Arrange
        var todoItem = TodoItemData.Create();
        var command = new CompleteTodoItemCommand(todoItem.Id);

        _todoItemsRepositoryMock
            .GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((TodoItem?)null);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TodoItemsErrors.NotFound);
    }
    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenTodoItemIsAlreadyCompleted()
    {
        // Arrange
        var todoItem = TodoItemData.Create();
        var command = new CompleteTodoItemCommand(todoItem.Id);
        todoItem.Complete(TodoItemData.CompletedOnUtc);

        _todoItemsRepositoryMock
            .GetByIdAsync(todoItem.Id, Arg.Any<CancellationToken>())
            .Returns(todoItem);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TodoItemsErrors.AlreadyCompleted);
    }


    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenRepositoryFails()
    {
        // Arrange
        var todoItem = TodoItemData.Create();
        var command = new CompleteTodoItemCommand(todoItem.Id);

        _todoItemsRepositoryMock
            .GetByIdAsync(todoItem.Id, Arg.Any<CancellationToken>())
            .Returns(todoItem);

        _todoItemsRepositoryMock
            .UpdateAsync(Arg.Any<TodoItem>(), Arg.Any<CancellationToken>())
            .Returns(0);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TodoItemsErrors.NoRowsAffected);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenUnitOfWorkFails()
    {
        // Arrange
        var todoItem = TodoItemData.Create();
        var command = new CompleteTodoItemCommand(todoItem.Id);

        _todoItemsRepositoryMock
            .GetByIdAsync(todoItem.Id, Arg.Any<CancellationToken>())
            .Returns(todoItem);

        _todoItemsRepositoryMock
            .UpdateAsync(Arg.Any<TodoItem>(), Arg.Any<CancellationToken>())
            .Returns(1);

        _unitOfWorkMock
            .SaveChangesAsync()
            .Returns(0);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TodoItemsErrors.NoRowsAffected);
    }
}
