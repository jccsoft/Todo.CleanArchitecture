using FluentAssertions;
using NSubstitute;
using Todo.Application.Abstractions.Data;
using Todo.Application.TodoItems.Add;
using Todo.Domain.TodoItems;
using Todo.SharedKernel;

namespace Todo.Application.UnitTests.TodoItems;

public class AddTodoItemCommandTests
{
    private static readonly AddTodoItemCommand _command = new(TodoItemData.Title.Value);

    private readonly AddTodoItemCommandHandler _handler;
    private readonly ITodoItemsRepository _todoItemsRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    public AddTodoItemCommandTests()
    {
        _todoItemsRepositoryMock = Substitute.For<ITodoItemsRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        IDateTimeProvider dateTimeProviderMock = Substitute.For<IDateTimeProvider>();
        dateTimeProviderMock.UtcNow.Returns(TodoItemData.CreatedOnUtc);

        _handler = new AddTodoItemCommandHandler(_todoItemsRepositoryMock, _unitOfWorkMock, dateTimeProviderMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenTodoItemIsCreated()
    {
        // Arrange
        _todoItemsRepositoryMock
            .AddAsync(Arg.Is<TodoItem>(t => t.Title!.Value == _command.Title),
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
    public async Task Handle_Should_ReturnFailure_WhenRepositoryFails()
    {
        // Arrange
        var todoItem = TodoItemData.Create();

        _todoItemsRepositoryMock
            .AddAsync(todoItem, Arg.Any<CancellationToken>())
            .Returns(0);

        // Act
        var result = await _handler.Handle(_command, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TodoItemsDomainErrors.NoRowsAffected);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenUnitOfWorkFails()
    {
        // Arrange
        var todoItem = TodoItemData.Create();

        _todoItemsRepositoryMock
            .AddAsync(todoItem, Arg.Any<CancellationToken>())
            .Returns(1);

        _unitOfWorkMock
            .SaveChangesAsync()
            .Returns(0);

        // Act
        var result = await _handler.Handle(_command, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TodoItemsDomainErrors.NoRowsAffected);
    }
}
