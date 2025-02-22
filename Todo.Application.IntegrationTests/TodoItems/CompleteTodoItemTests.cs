﻿using Todo.Application.TodoItems.Complete;

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
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenTodoItemNotExistsInDatabase()
    {
        // Arrange
        CompleteTodoItemCommand command = new(Guid.NewGuid());

        // Act
        Result result = await Sender.Send(command);

        // Assert
        result.IsFailure.Should().BeTrue();
    }
}
