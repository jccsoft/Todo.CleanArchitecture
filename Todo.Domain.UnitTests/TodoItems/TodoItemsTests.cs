using FluentAssertions;
using Todo.Domain.TodoItems;
using Todo.Domain.UnitTests.Infrastructure;

namespace Todo.Domain.UnitTests.TodoItems;

public class TodoItemsTests : BaseTest
{
    [Fact]
    public void Create_Should_SetPropertyValues()
    {
        // Act
        var todoItem = TodoItem.Create(TodoItemData.Title, TodoItemData.CreatedOnUtc);

        // Assert
        todoItem.Title.Should().Be(TodoItemData.Title);
        todoItem.CreatedOnUtc.Should().Be(TodoItemData.CreatedOnUtc);
        todoItem.Id.Should().NotBeEmpty();
        todoItem.CompletedOnUtc.Should().Be(null);
        todoItem.IsCompleted.Should().Be(false);
    }

    [Fact]
    public void Create_Should_RaiseTodoItemCreatedDomainEvent()
    {
        // Act
        var todoItem = TodoItem.Create(TodoItemData.Title, TodoItemData.CreatedOnUtc);

        // Assert
        var todoItemCreatedDomainEvent = AssertDomainEventWasPublished<TodoItemCreatedDomainEvent>(todoItem);

        todoItemCreatedDomainEvent.TodoId.Should().Be(todoItem.Id);
    }

    [Fact]
    public void Complete_Should_ReturnSuccess_WhenTodoItemIsCompleted()
    {
        // Arrange
        var todoItem = TodoItem.Create(TodoItemData.Title, TodoItemData.CreatedOnUtc);

        // Act
        var result = todoItem.Complete(TodoItemData.CompletedOnUtc);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Complete_Should_SetPropertyValues()
    {
        // Arrange
        var todoItem = TodoItem.Create(TodoItemData.Title, TodoItemData.CreatedOnUtc);

        // Act
        todoItem.Complete(TodoItemData.CompletedOnUtc);

        // Assert
        todoItem.CompletedOnUtc.Should().Be(TodoItemData.CompletedOnUtc);
        todoItem.IsCompleted.Should().Be(true);
    }

    [Fact]
    public void Complete_Should_RaiseTodoItemCompletedDomainEvent()
    {
        // Arrange
        var todoItem = TodoItem.Create(TodoItemData.Title, TodoItemData.CreatedOnUtc);

        // Act
        todoItem.Complete(TodoItemData.CompletedOnUtc);

        // Assert
        var todoItemCompletedDomainEvent = AssertDomainEventWasPublished<TodoItemCompletedDomainEvent>(todoItem);

        todoItemCompletedDomainEvent.TodoId.Should().Be(todoItem.Id);
    }

    [Fact]
    public void Complete_Should_ReturnFailure_WhenTodoItemAlreadyCompleted()
    {
        // Arrange
        var todoItem = TodoItem.Create(TodoItemData.Title, TodoItemData.CreatedOnUtc);
        todoItem.Complete(TodoItemData.CompletedOnUtc);

        // Act
        var result = todoItem.Complete(TodoItemData.CompletedOnUtc);

        // Assert
        result.IsFailure.Should().BeTrue();
    }
}
