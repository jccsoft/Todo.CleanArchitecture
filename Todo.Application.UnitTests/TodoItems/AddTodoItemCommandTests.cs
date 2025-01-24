using FluentAssertions;
using NSubstitute;
using Todo.Application.Abstractions.Data;
using Todo.Application.TodoItems.Create;
using Todo.Domain.TodoItems;
using Todo.SharedKernel;

namespace Todo.Application.UnitTests.TodoItems;

public class AddTodoItemCommandTests
{
    private static readonly CreateTodoItemCommand _command = new(TodoItemData.Title.Value);

    private readonly CreateTodoItemCommandHandler _handler;
    private readonly ITodoItemsRepository _todoItemsRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    public AddTodoItemCommandTests()
    {
        _todoItemsRepositoryMock = Substitute.For<ITodoItemsRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        IDateTimeProvider dateTimeProviderMock = Substitute.For<IDateTimeProvider>();
        dateTimeProviderMock.UtcNow.Returns(TodoItemData.CreatedOnUtc);

        _handler = new CreateTodoItemCommandHandler(_todoItemsRepositoryMock, _unitOfWorkMock, dateTimeProviderMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenTodoItemIsCreated()
    {
        // Arrange
        _todoItemsRepositoryMock
            .CreateAsync(Arg.Is<TodoItem>(t => t.Title!.Value == _command.Title),
                      Arg.Any<CancellationToken>())
            .Returns(1);

        _unitOfWorkMock
            .SaveChangesAsync(Arg.Is<List<Entity>>(l => ((TodoItem)l.FirstOrDefault()!).Title!.Value == _command.Title),
                              Arg.Any<CancellationToken>())
            .Returns(1);

        // Act
        var result = await _handler.Handle(_command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenTitleEmpty()
    {
        // Arrange
        var emptyCommand = new CreateTodoItemCommand("");

        // Act
        var result = await _handler.Handle(emptyCommand, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TodoItemsErrors.MissingTitle);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenRepositoryFails()
    {
        // Arrange
        var todoItem = TodoItemData.Create();

        _todoItemsRepositoryMock
            .CreateAsync(todoItem, Arg.Any<CancellationToken>())
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
        var todoItem = TodoItemData.Create();

        _todoItemsRepositoryMock
            .CreateAsync(todoItem, Arg.Any<CancellationToken>())
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
